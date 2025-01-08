using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionPanel_Multi : AbstractInteraction
{    
    //스크립트 캐싱
    public PanelManager_Multi panelManager_Multi;

    //게임 중 변하는 불값
    public bool isTouch = false;

    // Sprite 배열 미리 받아두는곳
    public List<Sprite> spriteData;

    void Awake()
    {
        //thePM = FindFirstObjectByType<PanelManager>();
    }

    void Update()
    {
        UpdateMethod();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        panelManager_Multi = playerManager_Multi.canvas.GetComponent<PanelManager_Multi>();
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    public override void UpdateMethod()
    {
        if (isColliding && !isActive)
        {
            confirmBtn.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                if (spriteData != null && spriteData.Count > 0)
                {
                    panelManager_Multi.ShowPanel(spriteData, false);
                }
                webglBtn.isClick = false;
            }

            if (isTouch)
            {
                panelManager_Multi.ShowPanel(spriteData, false);
            }
        }
    }

    public void ClickPanelEvent()
    {
        panelManager_Multi.ShowPanel(spriteData, false);
    }
}
