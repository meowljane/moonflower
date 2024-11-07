using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemStateManager : MonoBehaviour
{
    public GameObject roomContent;
    public GameObject roomPrefab;

    public GameObject itemContent;
    public GameObject itemPrefab;
    public Image itemDetail;


    private void OnEnable()
    {
        UpdateButtonState();
    }

    private void OnDisable()
    {
        foreach (Transform child in itemContent.transform)
        {
            Destroy(child.gameObject);
            SetImageAlpha(0f);
            itemDetail.sprite = null;
        }
    }

    private void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;

        // 아이템이 존재하는 방 갯수
        var roomsWithItems = databaseManager.itemInfos
            .Where(itemInfo => itemInfo.items.Any(item => item.status))
            .ToList();
        int roomCount = roomsWithItems.Count;

        // roomContent 비우기
        foreach (Transform child in roomContent.transform)
        {
            Destroy(child.gameObject);
        }

        // roomCount만큼 자녀 오브젝트 생성
        Vector2 initialPosition = new Vector2(-250, 125);
        float offsetX = 150f;
        for (int i = 0; i < roomCount; i++)
        {
            // Room 오브젝트 생성
            GameObject roomObject = Instantiate(roomPrefab, roomContent.transform);
            RectTransform rectTransform = roomObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = initialPosition + new Vector2(offsetX * i, 0);     
            }

            // roomName을 표시
            ItemInfo itemInfo = roomsWithItems[i]; 
            Text roomText = roomObject.GetComponentInChildren<Text>();
            if (roomText != null)
            {
                roomText.text = itemInfo.roomName;
            }
            Button roomButton = roomObject.GetComponent<Button>();
            if (roomButton != null)
            {
                roomButton.onClick.AddListener(() => ShowRoomInfo(itemInfo));
            }
        }
    }


    public void ShowRoomInfo(ItemInfo itemInfo)
    {
        //아이템 갯수 세기
        var trueStatusItems = itemInfo.items.Where(item => item.status).ToList();
        int itemCount = trueStatusItems.Count;

        foreach (Transform child in itemContent.transform)
        {
            Destroy(child.gameObject);
            SetImageAlpha(0f);
            itemDetail.sprite = null;
        }

        Vector2 initialPosition = new Vector2(-230, 0);
        float offsetX = 150f;
        for (int i = 0; i < itemCount; i++)
        {
            // 아이템 오브젝트 설치
            GameObject roomObject = Instantiate(itemPrefab, itemContent.transform);
            RectTransform rectTransform = roomObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = initialPosition + new Vector2(offsetX * i, 0);
            }

            //이미지 표시
            Image roomImage = roomObject.GetComponent<Image>();
            if (roomImage != null)
            {
                roomImage.sprite = trueStatusItems[i].itemImg;
            }

            //버튼 기능 추가
            var itemDetailImg = trueStatusItems[i].itemDetailImg;
            Button roomButton = roomObject.GetComponent<Button>();
            if (roomButton != null)
            {
                roomButton.onClick.AddListener(() => ShowItemInfo(itemDetailImg));
            }
        }
    }

    public void ShowItemInfo(Sprite itemDetailImg)
    {
        itemDetail.sprite = itemDetailImg;
        SetImageAlpha(255.0f);
    }

    public void SetImageAlpha(float alpha)
    {
        Color color = itemDetail.color;
        color.a = alpha;
        itemDetail.color = color;
    }
}
