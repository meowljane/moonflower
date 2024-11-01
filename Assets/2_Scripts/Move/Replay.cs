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
            // ��� ������ status�� NotHave�� ����
            foreach (ItemInfo item in DatabaseManager.instance.itemInfos)
            {
                item.status = ItemStatus.NotHave;
            }

            // ��� ����Ʈ status�� NotSeen���� ����
            foreach (QuestInfo quest in DatabaseManager.instance.questInfos)
            {
                quest.status = QuestStatus.NotSeen;
            }

            // ��� �ι� isActive�� false�� ����
            foreach (PersonInfo person in DatabaseManager.instance.personInfos)
            {
                person.isActive = false;
            }

            DatabaseManager.instance.minutes = 0;
            DatabaseManager.instance.seconds = 0;
        }

        //����ʱ�ȭ
        thePM.isSceneCount = 0;
        thePM.isTransfer = true;

        //thePM.playerSpeed = 270f;
        //�� �ʱ�ȭ
        thePM.lastMapName = "P4";
        thePM.currentMapName = "P2";
        SceneManager.LoadScene("P2");

        //endingBtn ���İ� 0���� �ʱ�ȭ 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // f0 �������� �۵��ϵ��� ����
        if (scene.name == "P2")
        {
            // Player �±׸� ���� ������Ʈ���� ã��
            GameObject[] objectsToHandle = GameObject.FindGameObjectsWithTag("Player");

            // �� ������Ʈ�� �����ϰų� ��Ȱ��ȭ
            foreach (GameObject obj in objectsToHandle)
            {
                //���� �÷��̾�� �ٸ� player������Ʈ�� ã�Ƽ� ����
                if(obj != thePlayer)
                    obj.SetActive(false);
            }
        }
    }
}