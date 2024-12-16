using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class NaverUserInfo : MonoBehaviour
{
    private string userInfoEndpoint = "https://openapi.naver.com/v1/nid/me";

    public string token;
    public InputField accessToken;

    //private void Start()
    //{
    //    token = "AAAAOge7bK5qDY6fafNr1lON2pWERh8nZWc30J-8g9hUF5pD6gLLkcz6DukF_09DDwcRk1i6NdjIy_bY_8NCOsqW0QE";
    //    RequestUserInfo(token);
    //}
    public void CallRequestUserInfo()
    {
        RequestUserInfo(accessToken.text);
    }

    public void RequestUserInfo(string accessToken)
    {
        StartCoroutine(RequestUserInfoCoroutine(accessToken));
    }

    private IEnumerator RequestUserInfoCoroutine(string accessToken)
    {
        UnityWebRequest request = new UnityWebRequest(userInfoEndpoint, "GET");
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("User Info Response: " + request.downloadHandler.text);
            // JSON         Ľ                Ȱ  
        }
        else
        {
            Debug.LogError("Error fetching user info: " + request.error);
        }
    }
}

[System.Serializable]
public class NaverUserResponse
{
    public NaverUser response;
}

[System.Serializable]
public class NaverUser
{
    public string id;  
    public string name;    
    public string email;  
}