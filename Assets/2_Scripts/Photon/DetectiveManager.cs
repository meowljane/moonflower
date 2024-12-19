using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DetectiveManager : MonoBehaviourPunCallbacks
{
    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public TMP_Text StatusText;
    public TMP_InputField roomCode_Input;

    public GameObject Room;
    public GameObject Server;

    public List<TMP_Text> WaitList;
    public List<TMP_Text> RoomInfo;

    public string roomCode;

    void Awake() => Screen.SetResolution(960, 540, false);

    void Update()
    {
        StatusText.text = "Log : " + PhotonNetwork.NetworkClientState.ToString();

        //RoomUpdate();
    }


    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        print("�������ӿϷ�");
        int randomNum = Random.Range(100, 1000);

        PhotonNetwork.LocalPlayer.NickName = "Player" + randomNum;
    }



    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("�������");



    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("�κ����ӿϷ�");

    public void CreateRoom2P()
    {
        roomCode = GenerateRoomCode();

        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 });
        Debug.Log($"Room Created with Code : {roomCode}");
    }

    public void CreateRoom3P()
    {
        roomCode = GenerateRoomCode();

        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 3 });
        Debug.Log($"Room Created with Code : {roomCode}");
    }

    public void CreateRandomRoom() => PhotonNetwork.CreateRoom(roomCode_Input.text, new RoomOptions { MaxPlayers = 3 });

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomCode_Input.text);
    }

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomCode_Input.text, new RoomOptions { MaxPlayers = 3 }, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        if (photonView.IsMine)
        {
            roomCode_Input.text = "";
        }
        RoomUpdate();
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        print("�游���Ϸ�");

        RoomUpdate();
        ServerOnOff();
    }

    public override void OnJoinedRoom()
    {
        print("�������Ϸ�");

        RoomUpdate();
        ServerOnOff();
        //Player -> �����ؾ��ϴ� �������� �̸�
        //PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Room creation failed : {message}");
        //CreateRoom();
    }
    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("�淣����������");

    private string GenerateRoomCode()
    {
        System.Random random = new System.Random();

        return new string(Enumerable.Repeat(characters, 6).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private void RoomUpdate()
    {
        RoomInfoUpdate();
        WaitRoomUpdate();
    }

    private void RoomInfoUpdate()
    {
        RoomInfo[0].text = "�� �ڵ� : " + roomCode;
        RoomInfo[1].text = "�÷��̾� : " + PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    private void WaitRoomUpdate()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (i == 0)
            {
                WaitList[i].text = "M - " + PhotonNetwork.PlayerList[i].NickName;
            }
            else
            {
                WaitList[i].text = "P - " + PhotonNetwork.PlayerList[i].NickName;
            }
        }
    }

    private void ServerOnOff()
    {
        Room.SetActive(true);
        Server.SetActive(false);
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