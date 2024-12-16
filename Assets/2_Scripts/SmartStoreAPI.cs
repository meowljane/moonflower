using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SmartStoreAPI : MonoBehaviour
{
    private string apiEndpoint = "https://dev.apis.naver.com/naverpay-partner/naverpay/payments/v2/list/history"; // �ùٸ� �ֹ� ��ȸ API ��������Ʈ
    private string clientId = "SJgdEpK3MKw1TpfW5VOA"; // ���̹� ���ø����̼ǿ��� �߱޹��� Client ID
    private string clientSecret = "WeadatlQqZ"; // ���̹� ���ø����̼ǿ��� �߱޹��� Client Secret
    private string accessToken = "AAAAOge7bK5qDY6fafNr1lON2pWERh8nZWc30J-8g9hUF5pD6gLLkcz6DukF_09DDwcRk1i6NdjIy_bY_8NCOsqW0QE"; // �߱޹��� Access Token

    //void Start()
    //{
    //    string buyerId = "vinu2"; // ����Ʈ������� ������ ID
    //    CheckPurchaseByBuyerId(buyerId);
    //}

    public void CheckPurchaseByBuyerId(string buyerId)
    {
        StartCoroutine(CheckPurchaseByBuyerIdCoroutine(buyerId));
    }

    private IEnumerator CheckPurchaseByBuyerIdCoroutine(string buyerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("buyerId", buyerId); // ������ ID �߰�
        form.AddField("startDate", "20241201"); // ��ȸ ���� ��¥ (YYYYMMDD)
        form.AddField("endDate", "20241212");   // ��ȸ ���� ��¥ (YYYYMMDD)

        UnityWebRequest request = UnityWebRequest.Post(apiEndpoint, form);

        // ��û ����� ���� ���� �߰�
        request.SetRequestHeader("X-Naver-Client-Id", clientId);
        request.SetRequestHeader("X-Naver-Client-Secret", clientSecret);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken.Trim());

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Purchase Response: " + jsonResponse);

            // ���� �����Ϳ��� Ư�� ���� �˻�
            if (jsonResponse.Contains("�����Ϸ�")) // "�����Ϸ�" ���� �˻�
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
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
    }
}