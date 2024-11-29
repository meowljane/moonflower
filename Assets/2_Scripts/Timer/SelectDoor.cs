using System.Collections;
using UnityEngine;

public class SelectDoor : MonoBehaviour
{
    private bool isColliding = false;
    private bool isDelay = false;

    [Header("Select Door Type...")]
    [Tooltip("���� ��⹮")]
    public bool prepareDoor;

    [Tooltip("��ŵ�ϰ� ������ ��")]
    public bool skipDoor;

    [Tooltip("�ð� ������ ���� �� �־����� ��")]
    public bool delayDoor;

    [Tooltip("������ �ð�")]
    public int delayTime;

    [Header("Using Door...")]
    [Tooltip("���� �� ������Ʈ")]
    public GameObject doorObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if ((prepareDoor && delayTime != null) || (delayDoor || delayTime != null))
        {
            return;
        }

        StartCoroutine(DoorDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isColliding || !skipDoor)
        {
            return;
        }

        DoorOpen();
    }

    private void DoorOpen()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            doorObject.SetActive(false);
        }
    }

    private IEnumerator DoorDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelay = true;
        doorObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
}
