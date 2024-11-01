using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RespawnSet : MonoBehaviour
{
    public PlayerManager thePlayer;
    public GameObject startPoint;

    public GameObject boundObject;
    public GameObject vcamObject;

    public Collider2D bound;

    private Transform resetPos;
    public void Respawn()
    {
        thePlayer.transform.position = resetPos.position;
    }

    void OnEnable()
    {
        startPoint = GameObject.Find("StartPoint");
        if (resetPos == null)
        {
            resetPos = startPoint.transform;
        }

/*        boundObject = GameObject.Find("Bound");
        if (bound == null)
        {
            bound = boundObject.GetComponent<Collider2D>();
        }*/
    }
}