using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionText_Multi : AbstractInteraction
{
    //스크립트 캐싱
    private TextManager_Multi textManager_Multi;

    //출력할 텍스트
    [TextArea]
    public string sentences;

    //대화창 잔류 시간 나타내는 float값
    public float closeCount = 2.0f;

    //인스펙터로 지정해주는 불값
    public bool isTouch = true;

    void Update()
    {
        UpdateMethod();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        textManager_Multi = playerManager_Multi.canvas.GetComponent<TextManager_Multi>();
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    public override void UpdateMethod()
    {
        if (isColliding && !isActive) //EventCollider에 닿아서 true가 돼었을때
        {
            if (isTouch) //Inspector창에서 true값으로 고정 / 스크립트에서 따로 조절하는 부분이 없음.
            {
                ActiveText(sentences);
            }
            else
            {
                confirmBtn.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
                {
                    ActiveText(sentences);
                }
            }
        }
    }

    private void ActiveText(string sentences)
    {
        textManager_Multi.ShowText(sentences);
        isActive = true;
        Invoke("CloseTMText", closeCount);
    }

    private void CloseTMText()
    {
        textManager_Multi.CloseText();
        isActive = false;
    }
}
