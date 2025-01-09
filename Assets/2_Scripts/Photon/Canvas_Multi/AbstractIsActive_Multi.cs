using Photon.Pun;
using UnityEngine;

public abstract class AbstractIsActive_Multi : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    protected static bool isActive = false;

    protected void IsActiveTrue()
    {
        isActive = true;
    }

    protected void IsActiveFalse()
    {
        isActive = false; 
    }

    protected bool ReturnIsActive()
    {
        return isActive;
    }
}
