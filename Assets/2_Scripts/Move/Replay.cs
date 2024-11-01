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
            // 모든 아이템 status를 NotHave로 설정
            foreach (ItemInfo item in DatabaseManager.instance.itemInfos)
            {
                item.status = ItemStatus.NotHave;
            }

            // 모든 퀘스트 status를 NotSeen으로 설정
            foreach (QuestInfo quest in DatabaseManager.instance.questInfos)
            {
                quest.status = QuestStatus.NotSeen;
            }

            // 모든 인물 isActive를 false로 설정
            foreach (PersonInfo person in DatabaseManager.instance.personInfos)
            {
                person.isActive = false;
            }

            DatabaseManager.instance.minutes = 0;
            DatabaseManager.instance.seconds = 0;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // f0 씬에서만 작동하도록 설정
        if (scene.name == "P2")
        {
            // Player 태그를 가진 오브젝트들을 찾음
            GameObject[] objectsToHandle = GameObject.FindGameObjectsWithTag("Player");

            // 각 오브젝트를 삭제하거나 비활성화
            foreach (GameObject obj in objectsToHandle)
            {
                //현재 플레이어와 다른 player오브젝트를 찾아서 삭제
                if(obj != thePlayer)
                    obj.SetActive(false);
            }
        }
    }
}