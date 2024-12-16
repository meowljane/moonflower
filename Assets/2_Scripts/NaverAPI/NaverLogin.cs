using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class NaverLogin : MonoBehaviour
{
    private string loginUrl = "https://nid.naver.com/oauth2.0/authorize";
    private string tokenUrl = "https://nid.naver.com/oauth2.0/token";
    private string apiEndpoint = "https://dev.apis.naver.com/naverpay-partner/naverpay/payments/v2/list/history";

    private string clientId = "SJgdEpK3MKw1TpfW5VOA";
    private string clientSecret = "WeadatlQqZ";
    private string redirectUri = "http://localhost:3000/callback";

    private string accessToken;
    private string state;

    private void Start()
    {
        // 자동으로 로그인 페이지를 엽니다.
        state = System.Guid.NewGuid().ToString();
        OpenNaverLoginPage();
    }

    /// <summary>
    /// 네이버 로그인 페이지를 엽니다.
    /// </summary>
    private void OpenNaverLoginPage()
    {
        string url = $"{loginUrl}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&state={state}";
        Application.OpenURL(url);
    }

    /// <summary>
    /// 토큰 발급을 위한 메서드입니다.
    /// </summary>
    /// <param name="code">네이버에서 반환한 인증 코드</param>
    public void RequestAccessToken(string code)
    {
        StartCoroutine(RequestAccessTokenCoroutine(code));
    }

    private IEnumerator RequestAccessTokenCoroutine(string code)
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "authorization_code");
        form.AddField("client_id", clientId);
        form.AddField("client_secret", clientSecret);
        form.AddField("code", code);
        form.AddField("state", state);
        form.AddField("redirect_uri", redirectUri);

        UnityWebRequest request = UnityWebRequest.Post(tokenUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            accessToken = ExtractToken(response, "access_token");
            Debug.Log("Access Token: " + accessToken);
            CheckPurchaseByBuyerId("piseinstar");
        }
        else
        {
            Debug.LogError("Error fetching token: " + request.error);
        }
    }

    /// <summary>
    /// 구매 내역 확인을 호출합니다.
    /// </summary>
    /// <param name="buyerId">구매자의 ID</param>
    public void CheckPurchaseByBuyerId(string buyerId)
    {
        StartCoroutine(CheckPurchaseByBuyerIdCoroutine(buyerId));
    }

    private IEnumerator CheckPurchaseByBuyerIdCoroutine(string buyerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("buyerId", buyerId);
        form.AddField("startDate", "20241201");
        form.AddField("endDate", "20241212");

        UnityWebRequest request = UnityWebRequest.Post(apiEndpoint, form);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Purchase Response: " + jsonResponse);
            // 구매 상태 확인 로직
            if (jsonResponse.Contains("네이버웹툰 원작 타인은 지옥이다 온라인 방탈출게임"))
            {
                Debug.Log("Purchase found!");
            }
            else
            {
                Debug.Log("No purchase found.");
            }
        }
        else
        {
            Debug.LogError("Error fetching purchase data: " + request.error);
        }
    }

    /// <summary>
    /// 응답에서 특정 토큰을 추출합니다.
    /// </summary>
    private string ExtractToken(string response, string key)
    {
        var match = Regex.Match(response, $"\"{key}\":\\\"(.*?)\\\"");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}
