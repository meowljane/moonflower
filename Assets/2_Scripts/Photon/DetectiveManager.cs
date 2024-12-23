using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
//기본 네임스페이스 6줄

public class DetectiveManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV; 
    // 방 코드 사용되는 영어 대, 소문자 및 숫자
    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    // 현재 서버 상태를 알려주는 텍스트 및 방 코드를 입력 받는 오브젝트
    public TMP_Text StatusText;
    public TMP_InputField roomCode_Input;

    // Button 이벤트로 즉각적으로 오브젝트가 꺼지는 것이 아닌 상황에 맞게 ON/OFF 가능하게 캐싱
    public GameObject Room;
    public GameObject Server;
    public GameObject GameStartBtn;
    public GameObject CenterLabel;

    // 유저명과 서버 정보를 나타내주는 오브젝트
    public List<GameObject> WaitList;
    public List<TMP_Text> RoomInfo;

    // 방 코드
    private string roomCode;

    // 센터라벨 텍스트
    private TMP_Text CenterLabelText;

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        CenterLabelText = CenterLabel.GetComponentInChildren<TMP_Text>();
    }
    #region 자체 함수 모음

    /// <summary>
    /// 서버 연결 기능 및 닉네임 생성 ex) Player312
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 서버 연결 해제 및 닉네임 초기화
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LocalPlayer.NickName = "";
    }

    /// <summary>
    /// 랜덤 6자리 코드를 사용하여 방 생성
    /// </summary>
    /// <param name="num"> 방 최대 인원수 </param>
    public void CreateRoom(int num)
    {
        roomCode = GenerateRoomCode();

        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = num });

        Debug.Log($"Room Created with Code : {roomCode}");
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomCode_Input.text);
    }

    /// <summary>
    /// 방 코드 복사 / WebGL에서 잘 되는지 테스트 할것
    /// </summary>
    public void CopyToClipBoard()
    {
        GUIUtility.systemCopyBuffer = PhotonNetwork.CurrentRoom.Name;
    }

    /// <summary>
    /// 방을 떠날 때 인풋 필드 값 초기화
    /// </summary>
    public void LeaveRoom()
    {
        roomCode_Input.text = "";
        RoomUpdate();

        SetWaitList(true);

        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// 대기실 리스트 방 최대 인원수에 따라 오브젝트 활성화
    /// </summary>
    /// <param name="isTrue"> 활용하지 않는 대기실을 ON/OFF 할지 선택 가능 </param>
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

    public void LoadNextScene2P_3P()
    {
        if (PhotonNetwork.PlayerList.Length <= PhotonNetwork.CurrentRoom.MaxPlayers - 1)
        {
            PhotonNetwork.LoadLevel("Multi_F0");
        }
        else
        {
            CenterLabelOn("현재 모드는 2, 3인 플레이만 지원합니다. 로비 화면에서 1인을 선택해주세요.");
        }
        
    }

    /// <summary>
    /// 서버 -> 방 이동 시 조건에 따라 실행되는 매서드
    /// </summary>
    private void ServerOnOff()
    {
        Room.SetActive(true);
        Server.SetActive(false);
    }

    /// <summary>
    /// 방 업데이트 매서드 종합
    /// </summary>
    private void RoomUpdate()
    {
        RoomInfoUpdate();
        WaitRoomUpdate();
        MasterStartBtnOnOff();
    }

    /// <summary>
    /// 방 코드와 플레이어 수 정보를 업데이트 시켜주는 매서드
    /// </summary>
    private void RoomInfoUpdate()
    {
        RoomInfo[0].text = "방 코드 : " + PhotonNetwork.CurrentRoom.Name;
        RoomInfo[1].text = "플레이어 : " + PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    private void CenterLabelOn(string labelText)
    {
        CenterLabelText.text = labelText;
        CenterLabel.SetActive(true);

        Invoke("CenterLabelOff", 2f);
    }

    private void CenterLabelOff()
    {
        CenterLabelText.text = "";
        CenterLabel.SetActive(false);
    }

    /// <summary>
    /// 마스터한테만 게임 스타트 버튼 표시
    /// </summary>
    private void MasterStartBtnOnOff()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameStartBtn.SetActive(true);
        }
        else
        {
            GameStartBtn.SetActive(false);
        }
    }

    /// <summary>
    /// 대기실 이름 명단 업데이트 및 본인 이름 색 변환
    /// </summary>
    private void WaitRoomUpdate()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

            // 플레이어 닉네임과 역할(M 또는 P) 설정
            childText.text = (i == 0) ? $"{PhotonNetwork.PlayerList[i].NickName = "Master"}" : $"{PhotonNetwork.PlayerList[i].NickName = "Player" + i}";

            // 본인 닉네임인지 확인하여 색상 설정
            childText.color = (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                ? Color.red
                : Color.black;
        }

        if (PhotonNetwork.PlayerList.Length - PhotonNetwork.CurrentRoom.MaxPlayers <= -1)
        {
            for (int i = PhotonNetwork.PlayerList.Length; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                TMP_Text childText = WaitList[i].GetComponentInChildren<TMP_Text>();

                childText.text = "대기 중...";
                childText.color = Color.gray; // 대기 중 메시지는 회색
            }
        }
    }

    private void LeaveRoomCenterLabel()
    {

    }

    /// <summary>
    /// 6자리 방코드 만들어주는 곳
    /// </summary>
    /// <returns> 방 코드 6자리 </returns>
    private string GenerateRoomCode()
    {
        System.Random random = new System.Random();

        return new string(Enumerable.Repeat(characters, 6).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// 게임 상태 Log로 확인
    /// </summary>
    private void LogUpdate()
    {
        //메서드로 분리 후 모든 Title씬 버튼 기능에 할당하기
        StatusText.text = "Log : " + PhotonNetwork.NetworkClientState.ToString();
    }

    #endregion

    #region 콜백 함수 모음
    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
        LogUpdate();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결끊김");
        LogUpdate();
    }

    public override void OnJoinedLobby()
    {
        print("로비접속완료");
        LogUpdate();
    }

    public override void OnCreatedRoom()
    {
        print("방만들기완료");

        ServerOnOff();
        SetWaitList(false);
        LogUpdate();
    }

    public override void OnJoinedRoom()
    {
        print("방참가완료");

        RoomUpdate();
        ServerOnOff();
        SetWaitList(false);
        LogUpdate();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomUpdate();
        print("안녕");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomUpdate();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Room creation failed : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방참가실패");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("방랜덤참가실패");
    }

    #endregion

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