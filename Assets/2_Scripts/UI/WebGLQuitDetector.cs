using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebGLQuitDetector : MonoBehaviour
{
    private float playTime;

    void Start()
    {
        // ������ ���� �̺�Ʈ ��� (WebGL������)
#if UNITY_WEBGL && !UNITY_EDITOR
        RegisterUnloadEvent();
#endif
    }

    // JavaScript �Լ� ȣ���� ���� ������ ���� �̺�Ʈ�� ���
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
    // �������� ���� �� ȣ��Ǵ� �Լ�
    // �޼��带 ȣ���ϸ� ���ڵ� ��ũ�� playTime�� ������
    // 
    public void OnBrowserQuit()
    {
        playTime = Time.time; // ���� ���� �� ����� �ð� (��)

        string message = $"���� �÷��� Ÿ��: {playTime}��";
        string webhookURL = "https://discord.com/api/webhooks/1283687957395537931/DS0oOye1dnbaSQDNrpSwlVbefs621nA1zJq27Buifs5vMkgRGz0ltB25leLgsfPW0B9L";

        StartCoroutine(UploadToDiscord(webhookURL, message));
    }

    IEnumerator UploadToDiscord(string URL, string message)
    {
        // ���ڵ� ���ſ� �´� JSON �������� �޽����� ����
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