using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;

public class SmartStoreAPI : MonoBehaviour
{
    // 네이버 스마트스토어 API 엔드포인트 및 인증 토큰
    private string apiEndpoint = "https://dev.apis.naver.com/naverpay-partner/naverpay/payments/v2/list/history";
    private string clientId = "SJgdEpK3MKw1TpfW5VOA"; // 클라이언트 ID
    private string clientSecret = "WeadatlQqZ"; // 클라이언트 Secret
    private string accessToken = "AAAAOge7bK5qDY6fafNr1lON2pWERh8nZWc30J-8g9hUF5pD6gLLkcz6DukF_09DDwcRk1i6NdjIy_bY_8NCOsqW0QE"; // 유효한 Access Token


    private void Start()
    {
        CheckPurchaseByBuyerId("piseinstar");
    }
    /// <summary>
    /// 특정 구매자의 ID를 기준으로 구매 내역을 확인합니다.
    /// </summary>
    /// <param name="buyerId">확인할 구매자의 ID</param>
    public void CheckPurchaseByBuyerId(string buyerId)
    {
        StartCoroutine(CheckPurchaseByBuyerIdCoroutine(buyerId));
    }

    /// <summary>
    /// 네이버 스마트스토어 API를 호출하여 구매 여부를 확인하는 코루틴입니다.
    /// </summary>
    /// <param name="buyerId">확인할 구매자의 ID</param>
    private IEnumerator CheckPurchaseByBuyerIdCoroutine(string buyerId)
    {
        WWWForm form = new WWWForm();
        form.AddField("buyerId", buyerId); // 구매자 ID 추가
        form.AddField("startDate", "20241201"); // 조회 시작 날짜 (YYYYMMDD)
        form.AddField("endDate", "20241212"); // 조회 종료 날짜 (YYYYMMDD)

        UnityWebRequest request = UnityWebRequest.Post(apiEndpoint, form);

        // 인증 헤더 추가
        request.SetRequestHeader("X-Naver-Client-Id", clientId);
        request.SetRequestHeader("X-Naver-Client-Secret", clientSecret);
        request.SetRequestHeader("Authorization", "Bearer " + accessToken.Trim());
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Purchase Response: " + jsonResponse);

            // JSON 데이터를 파싱하여 구매 상태 확인
            var purchaseData = SimpleJsonParser(jsonResponse);
            bool purchaseFound = purchaseData != null && purchaseData.Any(order => order.buyerId == buyerId && order.productName.Contains("네이버웹툰 원작 타인은 지옥이다 온라인 방탈출게임"));

            if (purchaseFound)
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

    /// <summary>
    /// 간단한 JSON 파서를 사용하여 데이터를 파싱합니다.
    /// </summary>
    /// <param name="jsonData">JSON 형식의 문자열 데이터</param>
    /// <returns>Order 리스트 반환</returns>
    private List<Order> SimpleJsonParser(string jsonData)
    {
        var orders = new List<Order>();
        try
        {
            // 간단한 수작업 JSON 파싱
            string[] items = jsonData.Split(new string[] { "{" }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                if (item.Contains("buyerId") && item.Contains("productName"))
                {
                    var order = new Order();
                    string[] fields = item.Split(',');
                    foreach (string field in fields)
                    {
                        if (field.Contains("buyerId"))
                        {
                            order.buyerId = field.Split(':')[1].Trim('"');
                        }
                        else if (field.Contains("productName"))
                        {
                            order.productName = field.Split(':')[1].Trim('"');
                        }
                    }
                    orders.Add(order);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error parsing JSON: " + ex.Message);
        }
        return orders;
    }

    /// <summary>
    /// 구매 내역을 나타내는 클래스
    /// </summary>
    private class Order
    {
        public string buyerId;
        public string productName;
    }
}