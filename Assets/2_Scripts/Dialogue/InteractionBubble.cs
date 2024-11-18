using System.Linq;
using UnityEngine;

public class InteractionBubble : MonoBehaviour
{
    //스크립트 캐싱
    private TextManager theTM;

    //오브젝트 캐싱
    public WebGLBtn webglBtn;
    public GameObject confirmOn;

    //출력할 텍스트
    [TextArea]
    public string sentences;

    //대화창 잔류 시간 나타내는 float값
    public float closeCount = 2.0f;

    //인스펙터로 지정해주는 불값
    public bool isAlways = true;

    //게임 중 변하는 불값
    private bool isColliding = false;
    private bool isActive = false;

    void Awake()
    {
        theTM = FindFirstObjectByType<TextManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }


    private void Update()
    {
        if (isColliding && !isActive) //EventCollider에 닿아서 true가 돼었을때
        {

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        confirmOn.SetActive(false);
    }

    void CloseTMText()
    {
        theTM.CloseText();
        isActive = false;
    }
}
