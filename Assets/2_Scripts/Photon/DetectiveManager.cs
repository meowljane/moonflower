using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
//�⺻ ���ӽ����̽� 6��

public class DetectiveManager : MonoBehaviourPunCallbacks
{
    // �� �ڵ� ���Ǵ� ���� ��, �ҹ��� �� ����
    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    // ���� ���� ���¸� �˷��ִ� �ؽ�Ʈ �� �� �ڵ带 �Է� �޴� ������Ʈ
    public TMP_Text StatusText;
    public TMP_InputField roomCode_Input;

    // Button �̺�Ʈ�� �ﰢ������ ������Ʈ�� ������ ���� �ƴ� ��Ȳ�� �°� ON/OFF �����ϰ� ĳ��
    public GameObject Room;
    public GameObject Server;

    // ������� ���� ������ ��Ÿ���ִ� ������Ʈ
    public List<GameObject> WaitList;
    public List<TMP_Text> RoomInfo;

    // �� �ڵ�
    private string roomCode;

    void Awake() => Screen.SetResolution(1920,1080, false);

    void Update()
    {
        //�޼���� �и� �� ��� Title�� ��ư ��ɿ� �Ҵ��ϱ�
        StatusText.text = "Log : " + PhotonNetwork.NetworkClientState.ToString();
    }

    #region ��ü �Լ� ����

    /// <summary>
    /// ���� ���� ��� �� �г��� ���� ex) Player312
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();

        int randomNum = Random.Range(100, 1000);
        PhotonNetwork.LocalPlayer.NickName = "Player" + randomNum;
    }

    /// <summary>
    /// ���� ���� ���� �� �г��� �ʱ�ȭ
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LocalPlayer.NickName = "";
    }

    /// <summary>
    /// ���� 6�ڸ� �ڵ带 ����Ͽ� �� ����
    /// </summary>
    /// <param name="num">�� �ִ� �ο���</param>
    public void CreateRoom(int num)
    {
        roomCode = GenerateRoomCode();

        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = num });

        Debug.Log($"Room Created with Code : {roomCode}");
    }

    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomCode_Input.text);

    /// <summary>
    /// �� �ڵ� ���� / WebGL���� �� �Ǵ��� �׽�Ʈ �Ұ�
    /// </summary>
    public void CopyToClipBoard() => GUIUtility.systemCopyBuffer = roomCode; 

    /// <summary>
    /// ���� ���� �� ��ǲ �ʵ� �� �ʱ�ȭ
    /// </summary>
    public void LeaveRoom()
    {
        roomCode_Input.text = "";
        RoomUpdate();

        SetWaitList(true);

        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// ���� ����Ʈ �� �ִ� �ο����� ���� ������Ʈ Ȱ��ȭ
    /// </summary>
    /// <param name="isTrue"> Ȱ������ �ʴ� ������ ON/OFF ���� ���� ���� </param>
    public void SetWaitList(bool isTrue)
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers < 5)
        {
            for (int i = PhotonNetwork.CurrentRoom.MaxPlayers; i < 5; i++)
            {
                WaitList[i].SetActive(isTrue);
            }
        }
    }

    /// <summary>
    /// ���� -> �� �̵� �� ���ǿ� ���� ����Ǵ� �ż���
    /// </summary>
    private void ServerOnOff()
    {
        Room.SetActive(true);
        Server.SetActive(false);
    }

    /// <summary>
    /// �� ������Ʈ �ż��� ����
    /// </summary>
    private void RoomUpdate()
    {
        RoomInfoUpdate();
        WaitRoomUpdate();
    }

    /// <summary>
    /// �� �ڵ�� �÷��̾� �� ������ ������Ʈ �����ִ� �ż���
    /// </summary>
    private void RoomInfoUpdate()
    {
        RoomInfo[0].text = "�� �ڵ� : " + PhotonNetwork.CurrentRoom.Name;
        RoomInfo[1].text = "�÷��̾� : " + PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    /// <summary>
    /// ���� �̸� ��� ������Ʈ �� ���� �̸� �� ��ȯ
    /// </summary>
    private void WaitRoomUpdate()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

            // ���� �г������� Ȯ��
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
            {
                if (i == 0)
                    childText.text = $"M - {PhotonNetwork.PlayerList[i].NickName}";

                else
                    childText.text = $"P - {PhotonNetwork.PlayerList[i].NickName}";

                childText.color = Color.red; // ���� �̸��� ������
            }
            else
            {
                if (i == 0)
                {
                    childText.text = $"M - {PhotonNetwork.PlayerList[i].NickName}";
                }
                else
                {
                    childText.text = $"P - {PhotonNetwork.PlayerList[i].NickName}";
                }

                childText.color = Color.black; // �ٸ� �÷��̾� �̸��� ������
            }
        }

        if (PhotonNetwork.PlayerList.Length - PhotonNetwork.CurrentRoom.MaxPlayers <= -1)
        {
            for (int i = PhotonNetwork.PlayerList.Length; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

                childText.text = "��� ��...";
                childText.color = Color.gray; // ��� �� �޽����� ȸ��
            }
        }
    }


    #endregion

    public override void OnConnectedToMaster() => print("�������ӿϷ�");

    public override void OnDisconnected(DisconnectCause cause) => print("�������");

    public override void OnJoinedLobby() => print("�κ����ӿϷ�");
    
    public override void OnCreatedRoom()
    {
        print("�游���Ϸ�");

        ServerOnOff();
        SetWaitList(false);
    }

    public override void OnJoinedRoom()
    {
        print("�������Ϸ�");

        RoomUpdate();
        ServerOnOff();
        SetWaitList(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) => RoomUpdate();

    public override void OnPlayerLeftRoom(Player otherPlayer) => RoomUpdate();

    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log($"Room creation failed : {message}");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("�淣����������");

    private string GenerateRoomCode()
    {
        System.Random random = new System.Random();

        return new string(Enumerable.Repeat(characters, 6).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }

        else
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            print("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }
}