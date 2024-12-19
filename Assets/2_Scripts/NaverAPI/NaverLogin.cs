using System;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class NaverLoginWithLocalServer : MonoBehaviour
{
    private string clientId = "SJgdEpK3MKw1TpfW5VOA";
    private string clientSecret = "WeadatlQqZ";
    private string redirectUri = "http://localhost:3000/callback";
    private string loginUrl;
    private HttpListener httpListener;
    private string state; // CSRF 방지를 위한 state 값

    public string accessToken; // 발급받은 Access Token 저장

    private void Start()
    {
        // 고유한 state 값 생성
        state = Guid.NewGuid().ToString();

        // OAuth 로그인 URL 생성
        loginUrl = $"https://nid.naver.com/oauth2.0/authorize?client_id={clientId}&response_type=code&redirect_uri={redirectUri}&state={state}";
        StartLocalServer();
        OpenLoginWebPage();
    }

    private void StartLocalServer()
    {
        try
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:3000/callback/");
            httpListener.Start();
            Debug.Log("Local server started. Waiting for redirect...");
            httpListener.BeginGetContext(OnRedirectReceived, httpListener);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error starting local server: " + ex.Message);
        }
    }


    private void OpenLoginWebPage()
    {
        Application.OpenURL(loginUrl);
    }

    private void OnRedirectReceived(IAsyncResult result)
    {
        try
        {
            var context = httpListener.EndGetContext(result);
            var request = context.Request;
            string url = request.Url.ToString();

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log("Redirected URL: " + url);

                // URL에서 code와 state 값 추출
                string codePattern = @"[?&]code=([^&]+)";
                string statePattern = @"[?&]state=([^&]+)";
                var codeMatch = Regex.Match(url, codePattern);
                var stateMatch = Regex.Match(url, statePattern);

                if (codeMatch.Success && stateMatch.Success)
                {
                    string authCode = codeMatch.Groups[1].Value;
                    string returnedState = stateMatch.Groups[1].Value;

                    Debug.Log("Auth Code: " + authCode);
                    Debug.Log("Returned State: " + returnedState);

                    if (returnedState == state)
                    {
                        StartCoroutine(GetAccessToken(authCode, returnedState));
                    }
                    else
                    {
                        Debug.LogError("State mismatch! Possible CSRF attack.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to extract code or state.");
                }
            });

            // 사용자에게 응답 전송
            var response = context.Response;
            string responseString = "<html><body>로그인 성공! 창을 닫아주세요.</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();

            StopLocalServer();
        }
        catch (Exception e)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.LogError("Error handling redirect: " + e.Message);
            });
        }
    }


    private IEnumerator GetAccessToken(string authCode, string stateValue)
    {
        string tokenUrl = $"https://nid.naver.com/oauth2.0/token?grant_type=authorization_code&client_id={clientId}&client_secret={clientSecret}&code={authCode}&state={stateValue}";

        UnityWebRequest request = UnityWebRequest.Get(tokenUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Access Token Response: " + request.downloadHandler.text);

            // JSON에서 accessToken 추출
            string response = request.downloadHandler.text;
            int startIndex = response.IndexOf("\"access_token\":\"") + 16;
            int endIndex = response.IndexOf("\"", startIndex);
            accessToken = response.Substring(startIndex, endIndex - startIndex);

            Debug.Log("Access Token: " + accessToken);

            // 이후 API 호출을 위한 accessToken 사용 가능
        }
        else
        {
            Debug.LogError("Failed to get access token: " + request.error);
        }
    }

    private void StopLocalServer()
    {
        if (httpListener != null && httpListener.IsListening)
        {
            httpListener.Stop();
            httpListener.Close();
            Debug.Log("Local server stopped.");
        }
    }

    private void OnApplicationQuit()
    {
        StopLocalServer();
    }
}
