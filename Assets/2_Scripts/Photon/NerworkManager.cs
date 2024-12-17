using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_Text StatusText;
    public TMP_Text InfoText;
    public TMP_InputField roomInput, NickNameInput;


    void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }

    void Update() => StatusText.text = "Log : " + PhotonNetwork.NetworkClientState.ToString();



    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        print("�������ӿϷ�");
        if (!string.IsNullOrEmpty(NickNameInput.text))
        {
            PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = "�̽��� ����û��"; // �⺻ �г��� ����
        }
    }

    public void Disconnect()
    {
        NickNameInput.text = "";
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause) => print("�������");



    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("�κ����ӿϷ�");



    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 3 });

    public void CreateRoom2() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });

    public void CreateRoom3() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 3 });

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 3 }, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        roomInput.text = "";
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom() => print("�游���Ϸ�");

    public override void OnJoinedRoom()
    {
        print("�������Ϸ�");
        //Player -> �����ؾ��ϴ� �������� �̸�
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游������");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("����������");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("�淣����������");



    [ContextMenu("����")]
    public void Info()
    {
        InfoText.text = "";

        if (PhotonNetwork.InRoom)
        {
            InfoText.text += "�÷��̾� �̸� : " + NickNameInput.text;
            InfoText.text += "\n\n���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name;
            InfoText.text += "\n\n���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount;
            InfoText.text += "\n\n���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers;

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            InfoText.text += "\n\n" + playerStr;
        }
        else
        {
            InfoText.text += "�÷��̾� �̸� : " + NickNameInput.text;
            InfoText.text += "\n\n������ �ο� �� : " + PhotonNetwork.CountOfPlayers;
            InfoText.text += "\n\n�� ���� : " + PhotonNetwork.CountOfRooms;
            InfoText.text += "\n\n��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms;
            InfoText.text += "\n\n�κ� �ִ���? : " + PhotonNetwork.InLobby;
            InfoText.text += "\n\n����ƴ���? : " + PhotonNetwork.IsConnected;
        }
    }
}