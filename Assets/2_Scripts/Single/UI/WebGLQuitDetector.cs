using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebGLQuitDetector : MonoBehaviour
{
    private float playTime;

    void Start()
    {
        // 브라우저 종료 이벤트 등록 (WebGL에서만)
#if UNITY_WEBGL && !UNITY_EDITOR
        RegisterUnloadEvent();
#endif
    }

    // JavaScript 함수 호출을 통해 브라우저 종료 이벤트를 등록
    private void RegisterUnloadEvent()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval(@"
            window.addEventListener('beforeunload', function (event) {
                var unityInstance = UnityLoader.instances[0];
                if (unityInstance) {
                    unityInstance.SendMessage('WebGLQuitDetector', 'OnBrowserQuit');
                }
            });
        ");
#endif
    }
    // 브라우저가 닫힐 때 호출되는 함수
    // 메서드를 호출하면 디스코드 링크로 playTime을 보내줌
    // 
    public void OnBrowserQuit()
    {
        playTime = Time.time; // 게임 시작 후 경과한 시간 (초)

        string message = $"현재 플레이 타임: {playTime}초";
        string webhookURL = "https://discord.com/api/webhooks/1283687957395537931/DS0oOye1dnbaSQDNrpSwlVbefs621nA1zJq27Buifs5vMkgRGz0ltB25leLgsfPW0B9L";

        StartCoroutine(UploadToDiscord(webhookURL, message));
    }

    IEnumerator UploadToDiscord(string URL, string message)
    {
        // 디스코드 웹훅에 맞는 JSON 형식으로 메시지를 생성
        string jsonPayload = "{\"content\": \"" + message + "\"}";

        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
        }
    }
}