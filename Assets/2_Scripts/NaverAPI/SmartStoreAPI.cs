using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SmartStoreAPI : MonoBehaviour
{
    // 네이버 스마트스토어 API 엔드포인트
    private string apiEndpoint = "https://dev.apis.naver.com/naverpay-partner/naverpay/payments/v2/list/history";

    public NaverLoginWithLocalServer loginServer; // NaverLoginWithLocalServer 참조

    private void Start()
    {
        // `accessToken`이 준비되면 구매내역 확인
        StartCoroutine(WaitForAccessTokenAndCheckPurchase("piseinstar"));
    }

    private IEnumerator WaitForAccessTokenAndCheckPurchase(string buyerId)
    {
        // AccessToken이 설정될 때까지 대기
        while (string.IsNullOrEmpty(loginServer.accessToken))
        {
            yield return null;
        }

        // AccessToken으로 구매내역 확인
        CheckPurchaseByBuyerId(buyerId, loginServer.accessToken);
    }

    public void CheckPurchaseByBuyerId(string buyerId, string accessToken)
    {
        StartCoroutine(CheckPurchaseByBuyerIdCoroutine(buyerId, accessToken));
    }

    private IEnumerator CheckPurchaseByBuyerIdCoroutine(string buyerId, string accessToken)
    {
        WWWForm form = new WWWForm();
        form.AddField("buyerId", buyerId);
        form.AddField("startDate", "20241201"); // 조회 시작 날짜 (YYYYMMDD)
        form.AddField("endDate", "20241212"); // 조회 종료 날짜 (YYYYMMDD)

        UnityWebRequest request = UnityWebRequest.Post(apiEndpoint, form);

        // 인증 헤더 추가
        request.SetRequestHeader("Authorization", "Bearer " + accessToken);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Purchase Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error fetching purchase data: " + request.error);
        }
    }
}
