using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPage : MonoBehaviour
{
    public Replay theReplay;
    public PlayerManager thePlayer;
    public WindowManager windowManager;
    public GameObject QuestTextManager;
    public GameObject CloseButton;
    public GameObject ConfirmButton;
    public Text text;
    public GameObject panel;

    void Awake()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(ExitDialogue);
        ConfirmButton.GetComponent<Button>().onClick.AddListener(ResetGame);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowDialogue();
            thePlayer.canMove = false;
        }
    }
        public void ShowDialogue()
    {
        thePlayer.canMove = false;
        windowManager.OpenWindow(QuestTextManager);
        text.text = "キャラクターの位置を初期化したい場合は左ボタン、" +
            "\r\nゲーム初期化をしたい場合は右ボタンを押してください。";
        CloseButton.SetActive(true);
        ConfirmButton.SetActive(true);
        panel.SetActive(true);
        text.gameObject.SetActive(true);
    }

    public void ExitDialogue()
    {
        thePlayer.canMove = true;
        text.text = "";
        QuestTextManager.SetActive(false);
        CloseButton.SetActive(false);
        ConfirmButton.SetActive(false);
        panel.SetActive(false);

        windowManager.CloseWindow();
    }

    public void ResetGame()
    {
        theReplay.ReplayGame();
        ExitDialogue();
    }

}
