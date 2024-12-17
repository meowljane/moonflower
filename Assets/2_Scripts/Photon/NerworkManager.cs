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
        print("서버접속완료");
        if (!string.IsNullOrEmpty(NickNameInput.text))
        {
            PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = "이승현 개멍청이"; // 기본 닉네임 설정
        }
    }

    public void Disconnect()
    {
        NickNameInput.text = "";
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause) => print("연결끊김");



    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("로비접속완료");



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

    public override void OnCreatedRoom() => print("방만들기완료");

    public override void OnJoinedRoom()
    {
        print("방참가완료");
        //Player -> 생성해야하는 프리팹의 이름
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("방랜덤참가실패");



    [ContextMenu("정보")]
    public void Info()
    {
        InfoText.text = "";

        if (PhotonNetwork.InRoom)
        {
            InfoText.text += "플레이어 이름 : " + NickNameInput.text;
            InfoText.text += "\n\n현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name;
            InfoText.text += "\n\n현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount;
            InfoText.text += "\n\n현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers;

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            InfoText.text += "\n\n" + playerStr;
        }
        else
        {
            InfoText.text += "플레이어 이름 : " + NickNameInput.text;
            InfoText.text += "\n\n접속한 인원 수 : " + PhotonNetwork.CountOfPlayers;
            InfoText.text += "\n\n방 개수 : " + PhotonNetwork.CountOfRooms;
            InfoText.text += "\n\n모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms;
            InfoText.text += "\n\n로비에 있는지? : " + PhotonNetwork.InLobby;
            InfoText.text += "\n\n연결됐는지? : " + PhotonNetwork.IsConnected;
        }
    }
}