using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemStateManager : MonoBehaviour
{
    public GameObject roomContent;
    public GameObject roomPrefab;

    public GameObject itemContent;
    public GameObject itemPrefab;
    public Image itemDetail;
    public List<Sprite> itemList;
    public GameObject btnLeft;
    public GameObject btnRight;

    public int currentItemIndex = 0;


    private void OnEnable()
    {
        UpdateButtonState();
    }

    private void OnDisable()
    {
        Debug.Log("���ı�");
        foreach (Transform child in itemContent.transform)
        {
            Destroy(child.gameObject);
            SetImageAlpha(0f);
            itemDetail.sprite = null;
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
        }
    }

    private void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;

        // �������� �����ϴ� �� ����
        var roomsWithItems = databaseManager.itemInfos
            .Where(itemInfo => itemInfo.items.Any(item => item.status))
            .ToList();
        int roomCount = roomsWithItems.Count;

        // roomContent ����
        foreach (Transform child in roomContent.transform)
        {
            Destroy(child.gameObject);
        }

        // roomCount��ŭ �ڳ� ������Ʈ ����
        Vector2 initialPosition = new Vector2(-250, 125);
        float offsetX = 150f;
        for (int i = 0; i < roomCount; i++)
        {
            // Room ������Ʈ ����
            GameObject roomObject = Instantiate(roomPrefab, roomContent.transform);
            RectTransform rectTransform = roomObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = initialPosition + new Vector2(offsetX * i, 0);     
            }

            // roomName�� ǥ��
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
        //������ ���� ����
        var trueStatusItems = itemInfo.items.Where(item => item.status).ToList();
        int itemCount = trueStatusItems.Count;

        foreach (Transform child in itemContent.transform)
        {
            Destroy(child.gameObject);
            SetImageAlpha(0f);
            itemDetail.sprite = null;
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
        }

        Vector2 initialPosition = new Vector2(-230, 0);
        float offsetX = 150f;
        for (int i = 0; i < itemCount; i++)
        {
            // ������ ������Ʈ ��ġ
            GameObject roomObject = Instantiate(itemPrefab, itemContent.transform);
            RectTransform rectTransform = roomObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = initialPosition + new Vector2(offsetX * i, 0);
            }

            //�̹��� ǥ��
            Image roomImage = roomObject.GetComponent<Image>();
            if (roomImage != null)
            {
                roomImage.sprite = trueStatusItems[i].itemImg;
            }

            //��ư ��� �߰�
            var itemDetailImg = trueStatusItems[i].itemDetailImg;
            Button roomButton = roomObject.GetComponent<Button>();
            if (roomButton != null)
            {
                roomButton.onClick.AddListener(() => ShowItemInfo(itemDetailImg));
            }
        }
    }

    public void ShowItemInfo(List<Sprite> itemDetailImg)
    {
        currentItemIndex = 0;
        itemList = itemDetailImg;
        itemDetail.sprite = itemDetailImg[0];
        if(itemDetailImg.Count > 1)
        {
            btnLeft.SetActive(true);
            btnRight.SetActive(true);
        }
        else
        {
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
        }
        SetImageAlpha(255.0f);
    }
    public void NextImgaeDetail()
    {
        if (itemList != null && currentItemIndex < itemList.Count - 1)
        {
            currentItemIndex++;
            itemDetail.sprite = itemList[currentItemIndex];
        }
        else
        {
            currentItemIndex = 0;
            itemDetail.sprite = itemList[currentItemIndex];
        }
    }

    public void BackImgaeDetail()
    {
        if (itemList != null && currentItemIndex > 0)
        {
            currentItemIndex--;
            itemDetail.sprite = itemList[currentItemIndex];
        }
        else
        {
            currentItemIndex = itemList.Count - 1;
            itemDetail.sprite = itemList[currentItemIndex];
        }
    }

    public void SetImageAlpha(float alpha)
    {
        Color color = itemDetail.color;
        color.a = alpha;
        itemDetail.color = color;
    }
}
