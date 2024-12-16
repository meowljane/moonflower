using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SmartStoreAPI : MonoBehaviour
{
    private string apiEndpoint = "https://dev.apis.naver.com/naverpay-partner/naverpay/payments/v2/list/history"; // 올바른 주문 조회 API 엔드포인트
    private string clientId = "SJgdEpK3MKw1TpfW5VOA"; // 네이버 애플리케이션에서 발급받은 Client ID
    private string clientSecret = "WeadatlQqZ"; // 네이버 애플리케이션에서 발급받은 Client Secret
    private string accessToken = "AAAAOge7bK5qDY6fafNr1lON2pWERh8nZWc30J-8g9hUF5pD6gLLkcz6DukF_09DDwcRk1i6NdjIy_bY_8NCOsqW0QE"; // 발급받은 Access Token

    //void Start()
    //{
    //    string buyerId = "vinu2"; // 스마트스토어의 구매자 ID
    //    CheckPurchaseByBuyerId(buyerId);
    //}

    public void CheckPurchaseByBuyerId(string buyerId)
    {
        StartCoroutine(CheckPurchaseByBuyerIdCoroutine(buyerId));
    }

    private IEnumerator CheckPurchaseByBuyerIdCoroutine(string buyerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("buyerId", buyerId); // 구매자 ID 추가
        form.AddField("startDate", "20241201"); // 조회 시작 날짜 (YYYYMMDD)
        form.AddField("endDate", "20241212");   // 조회 종료 날짜 (YYYYMMDD)

        UnityWebRequest request = UnityWebRequest.Post(apiEndpoint, form);

        // 요청 헤더에 인증 정보 추가
        request.SetRequestHeader("X-Naver-Client-Id", clientId);
        request.SetRequestHeader("X-Naver-Client-Secret", clientSecret);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken.Trim());

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Purchase Response: " + jsonResponse);

            // 응답 데이터에서 특정 조건 검색
            if (jsonResponse.Contains("결제완료")) // "결제완료" 상태 검색
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