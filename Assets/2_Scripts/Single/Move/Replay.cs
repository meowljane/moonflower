using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    public GameObject thePlayer;
    private PlayerManager thePM;

    public GameObject button;

    void Awake()
    {
        if (thePlayer == null)
        {
            thePlayer = GameObject.FindGameObjectWithTag("Player");
        }

        if(thePlayer != null)
        {
            thePM = thePlayer.GetComponent<PlayerManager>();
        }

        button = GameObject.Find("Button");
    }

    public void ReplayGame()
    {
        if (DatabaseManager.instance != null)
        {
            foreach (ItemInfo itemInfo in DatabaseManager.instance.itemInfos)
            {
                for (int i = 0; i < itemInfo.items.Count; i++)
                {
                    ItemInfo.ItemData itemData = itemInfo.items[i];
                    itemData.status = false;
                    itemInfo.items[i] = itemData;
                }
            }
        }

        //브금초기화
        thePM.isSceneCount = 0;
        thePM.isTransfer = true;

        //thePM.playerSpeed = 270f;
        //맵 초기화
        thePM.lastMapName = "P4";
        thePM.currentMapName = "P2";
        SceneManager.LoadScene("P2");

        //endingBtn 알파값 0으로 초기화 
    }
}