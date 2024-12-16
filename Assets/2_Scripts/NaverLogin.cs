using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NaverLogin : MonoBehaviour
{
    private string tokenEndpoint = "https://nid.naver.com/oauth2.0/token";
    private string clientId = "SJgdEpK3MKw1TpfW5VOA"; // 복사해둔 Client ID
    private string clientSecret = "WeadatlQqZ";
    
    public InputField authorizationCode;
    public InputField returnedState;


    private string redirectUri = "http://localhost:3000/callback"; // 설정한 Redirect URI
    private string state = System.Guid.NewGuid().ToString(); // 보안용 랜덤 문자열


    public void OpenNaverLoginPage()
    {
        string loginUrl = $"https://nid.naver.com/oauth2.0/authorize?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&state={state}";
        Application.OpenURL(loginUrl);
    }

    // Access Token 요청
    public void CallRequestAccessToken()
    {
        RequestAccessToken("ZAjpnC6mQVn0rexApO", "910cacf1-5a27-4e2e-a018-8ac135d0d2b6");
    }
    public void RequestAccessToken(string authorizationCode, string state)
    {
        StartCoroutine(RequestAccessTokenCoroutine());
    }

    private IEnumerator RequestAccessTokenCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "authorization_code");
        form.AddField("client_id", clientId);
        form.AddField("client_secret", clientSecret);
        form.AddField("redirect_uri", redirectUri);
        form.AddField("scope", "order.read payment.read"); // 필요한 스코프 추가

        UnityWebRequest request = UnityWebRequest.Post(tokenEndpoint, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Access Token Response: " + request.downloadHandler.text);
            // Access Token을 저장 및 사용
        }
        else
        {
            Debug.LogError("Error requesting Access Token: " + request.error);
        }
    }
}