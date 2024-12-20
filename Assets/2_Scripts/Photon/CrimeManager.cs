using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using static Unity.Burst.Intrinsics.X86.Avx;

public class CrimeManager : MonoBehaviourPunCallbacks
{
    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public TMP_Text StatusText;
    public TMP_InputField roomCode_Input;

    public GameObject Room;
    public GameObject Server;

    public List<GameObject> WaitList;
    public List<TMP_Text> RoomInfo;

    public string roomCode;

    void Awake() => Screen.SetResolution(1920,1080, false);

    void Update()
    {
        StatusText.text = "Log : " + PhotonNetwork.NetworkClientState.ToString();

        //RoomUpdate();
    }


    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();

        int randomNum = Random.Range(100, 1000);
        PhotonNetwork.LocalPlayer.NickName = "Player" + randomNum;
    }

    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LocalPlayer.NickName = "";
    }

    public override void OnDisconnected(DisconnectCause cause) => print("연결끊김");

    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => print("로비접속완료");

    public void CreateRoom(int num)
    {
        roomCode = GenerateRoomCode();

        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = num });

        Debug.Log($"Room Created with Code : {roomCode}");
    }

    //public void CreateRandomRoom() => PhotonNetwork.CreateRoom(roomCode_Input.text, new RoomOptions { MaxPlayers = 3 });

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

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomCode_Input.text);
    }

    //public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomCode_Input.text, new RoomOptions { MaxPlayers = 3 }, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        if (photonView.IsMine)
        {
            roomCode_Input.text = "";
        }
        RoomUpdate();

        SetWaitList(true);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        print("방만들기완료");

        ServerOnOff();
        SetWaitList(false);
    }

    public override void OnJoinedRoom()
    {
        print("방참가완료");

        RoomUpdate();
        ServerOnOff();
        SetWaitList(false);
        //Player -> 생성해야하는 프리팹의 이름
        //PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomUpdate();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomUpdate();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Room creation failed : {message}");
        //CreateRoom();
    }
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("방랜덤참가실패");

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
        RoomInfo[0].text = "방 코드 : " + PhotonNetwork.CurrentRoom.Name;
        RoomInfo[1].text = "플레이어 : " + PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    private void WaitRoomUpdate()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

            if (i == 0)
            {
                childText.text = "M - " + PhotonNetwork.PlayerList[i].NickName;
            }
            else
            {
                childText.text = "P - " + PhotonNetwork.PlayerList[i].NickName;
            }
        }

        if(PhotonNetwork.PlayerList.Length - PhotonNetwork.CurrentRoom.MaxPlayers <= -1)
        {
            for (int i = PhotonNetwork.PlayerList.Length; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

                childText.text = "대기 중...";
            }
        }
    }

    private void ServerOnOff()
    {
        Room.SetActive(true);
        Server.SetActive(false);
    }

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}