using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionDoor_Multi : AbstractInteraction
{
    public PanelManager_Multi panelManager_Multi;

    //활성화, 비활성화할 오브젝트들
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    // Sprite 배열 미리 받아두는곳
    public List<Sprite> spriteData;

    //게임 중 변하는 불값
    public bool isTouch = false;

    void Update()
    {
        UpdateMethod();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        panelManager_Multi = playerManager_Multi.canvas.GetComponent<PanelManager_Multi>();

        panelManager_Multi.objectsToDisable = objectsToDisable;
        panelManager_Multi.objectsToEnable = objectsToEnable;
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
                    panelManager_Multi.ShowPanel(spriteData, true);
                }
                else
                {
                    UpdateObjects();
                }
                webglBtn.isClick = false;
            }
        }
    }

    public void ClickPanelEvent()
    {
        panelManager_Multi.ShowPanel(spriteData, false);
    }

    private void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
