using System.Collections;
using UnityEngine;

public class SelectDoor : MonoBehaviour
{
    private bool isColliding = false;
    private bool isDelay = false;

    [Header("Select Door Type...")]
    [Tooltip("시작 대기문")]
    public bool prepareDoor;

    [Tooltip("스킵하고 나가는 문")]
    public bool skipDoor;

    [Tooltip("시간 지나면 나갈 수 있어지는 문")]
    public bool delayDoor;

    [Tooltip("딜레이 시간")]
    public int delayTime;

    [Header("Using Door...")]
    [Tooltip("사용될 문 오브젝트")]
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
