using System;
using System.Collections;
using System.Collections.Generic;
using Silly;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace gunggme
{
    public class GUINetwork : MonoBehaviourPunCallbacks
    {
        public GUISkin skin;
        public string netWorkStatus = "netWorkStatus";
        public float guiWidth = 1000f;
        public float guiHeight = 500.0f;
        public Transform[] wayPoint;

        private string[] ServerStatus = new string[]
        {
            "ServerStatus",
            "Connected",
            "JoinLobby",
            "Disconnected",
            "CreateRoom",
            "FailedCreateRoom",
            "JoinRoom",
            "LeaveRobby"
        };

        private enum EnumStatus
        {
            ServerStatus,
            Connected,
            JoinLobby,
            Disconnected,
            CreateRoom,
            FailedCreateRoom,
            JoinRoom,
            LeaveRobby
        }

        private EnumStatus statusNum = EnumStatus.ServerStatus;
        private List<string> player = new List<string>();
        private bool isOpen = false;
        private readonly string version = "1.0f";
        private string userId = "user1";
        private string roomName = "room0";
        private string chatData = "";
        private string stringToEdit = "";
        private RoomOptions roomOptions = new RoomOptions();
        private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

        /// <summary>
        /// 포톤 접속 정보 세팅
        /// </summary>
        void SetPhotonInfo()
        {
            // 같은 룸의 유저들에게 자동 씬 로딩
            PhotonNetwork.AutomaticallySyncScene = true;
            //같은 버전 접속 허용
            PhotonNetwork.GameVersion = version;
            // 아이디 할당
            PhotonNetwork.NickName = userId;
            // 룸 옵션
            // 최대 접속자 수, 무료 버전 최대 접속자 20명
            roomOptions.MaxPlayers = 2;
            // 룸을 열어놓을 것인지
            roomOptions.IsOpen = true;
            // 로비에서 홈 목록 노출
            roomOptions.IsVisible = true;
        }

        private void OnGUI()
        {
            GUI.skin = skin;
            if (isOpen)
            {
                GUI.BeginGroup(new Rect((Screen.width - guiWidth) / 2, (Screen.height - guiHeight) / 2, guiWidth, guiHeight));
                GUI.Box(new Rect(0, 0, guiWidth, guiHeight), "Network GUI");

                if (GUI.Button(new Rect(20, 20, 220, 40), "서버생성"))
                {
                    SetPhotonInfo();
                }

                userId = GUI.TextField(new Rect(20, 120, 220, 40), userId);
                GUI.Button(new Rect(guiWidth - 240, 70, 220, 40), netWorkStatus);
                roomName = GUI.TextField(new Rect(guiWidth - 240, 120, 220, 40), roomName);

                if (GUI.Button(new Rect(25, 180, 110, 30), "서버 접속"))
                {
                    PhotonNetwork.ConnectUsingSettings();
                }
                if (GUI.Button(new Rect(140, 180, 110, 30), "서버 접속 헤제"))
                {
                    PhotonNetwork.Disconnect();
                }
                if (GUI.Button(new Rect(255, 180, 110, 30), "로비 접속"))
                {
                    PhotonNetwork.JoinLobby();
                }
                if (GUI.Button(new Rect(370, 180, 110, 30), "로비 접속 헤제"))
                {
                    PhotonNetwork.LeaveLobby();
                }

                if (GUI.Button(new Rect(25, 230, 110, 35), "방 만들기"))
                {
                    if (roomName != null)
                    {
                        PhotonNetwork.CreateRoom(roomName, roomOptions);
                    }
                }
                if (GUI.Button(new Rect(140, 230, 110, 35), "방 참가"))
                {
                    PhotonNetwork.JoinRoom(roomName);
                }
                if (GUI.Button(new Rect(255, 230, 110, 35), "방 랜덤 참가"))
                {
                    PhotonNetwork.JoinRandomRoom();
                }
                if (GUI.Button(new Rect(370, 230, 110, 35), "방 나가기"))
                {
                    PhotonNetwork.LeaveRoom();
                }

                if (GUI.Button(new Rect((guiWidth - 90), 10, 40, 40), "X"))
                {
                    isOpen = false;
                }
                GUI.EndGroup();
            }
            else
            {
                if (GUI.Button(new Rect(Screen.width - 60, 45, 45, 40), "메뉴"))
                {
                    isOpen = true;
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("마스터에 접속");
            statusNum = EnumStatus.Connected;
            netWorkStatus = ServerStatus[(int)statusNum];
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("로비에 접속하였습니다");
            statusNum = EnumStatus.JoinLobby;
            netWorkStatus = ServerStatus[(int)statusNum];

            GameObject obj = PhotonNetwork.Instantiate("Player",
                wayPoint[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, Quaternion.identity);

            if (obj.GetPhotonView().IsMine)
            {
                
            }
            
            cachedRoomList.Clear();
        }

        public override void OnLeftLobby()
        {
            Debug.Log("로비에 접속을 헤제하였습니다");
            
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("룸이 생성되었습니다.");
            Debug.Log($"생성된 룸 이름 : {PhotonNetwork.CurrentRoom.Name}");
            statusNum = EnumStatus.CreateRoom;
            netWorkStatus = ServerStatus[(int)statusNum];
        }

        public void UpdateCachedRoom(List<RoomInfo> roomList)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                if (info.RemovedFromList)
                {
                    cachedRoomList.Remove(info.Name);
                }
                else
                {
                    cachedRoomList[info.Name] = info;
                }
            }
        }
        
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            UpdateCachedRoom(roomList);
            foreach (RoomInfo roomInfo in roomList)
            {
                Debug.Log($"Room Name : {roomInfo.Name} IsOpen : {roomInfo.IsOpen} Player Count : {roomInfo.PlayerCount} / {roomInfo.MaxPlayers}");
            }
            {
                
            }
        }
    }
}