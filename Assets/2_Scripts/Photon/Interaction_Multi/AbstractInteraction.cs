using UnityEngine;
using Photon.Pun;

public abstract class AbstractInteraction : AbstractIsActive_Multi, ITriggerEnter, ITriggerExit
{
    [HideInInspector]
    public WebGLBtn_Multi webglBtn;

    [HideInInspector]
    public GameObject confirmBtn;

    [HideInInspector]
    public bool isColliding = false;

    [HideInInspector]
    public PlayerManager_Multi playerManager_Multi;
    
    [HideInInspector]
    public PhotonView PV;

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        GetPVComponent(other);

        if (CheckIsMine(PV)) {
            webglBtn = playerManager_Multi.canvas.GetComponent<WebGLBtn_Multi>();
            confirmBtn = playerManager_Multi.ConfirmOn.gameObject;

            isColliding = true;
        }
    }
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        GetPVComponent(other);

        if (CheckIsMine(PV))
        {
            isColliding = false;
            confirmBtn.SetActive(false);
        }
    }

    public abstract void UpdateMethod();

    public void GetPVComponent(Collider2D other)
    {
        playerManager_Multi = other.GetComponent<PlayerManager_Multi>();

        PV = playerManager_Multi.GetComponent<PhotonView>();
    }
    public bool CheckIsMine(PhotonView PV)
    {
        if (PV.IsMine)
        {
            return true;
        }

        return false;
    }
}