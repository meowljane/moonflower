using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Respawn : MonoBehaviour
{
    public PlayerManager thePlayer;
    public GameObject startPoint;

    private Transform resetPos;

    public void RespawnBtn()
    {
        thePlayer.transform.position = resetPos.position;
    }
    private void OnEnable()
    {
        startPoint = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "StartPoint");
        resetPos = startPoint.transform;
    }
}