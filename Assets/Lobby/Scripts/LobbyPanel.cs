using JetBrains.Annotations;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private GameObject createRoomPanel;
    [SerializeField]
    private GameObject customizePanel;
    [SerializeField]
    private GameObject roomEntryPrefab;
    [SerializeField]
    private RectTransform roomContent;
    [SerializeField]
    private TMP_InputField roomNameInputField;
    [SerializeField]
    private TMP_InputField MaxPlayerInputField;

    [SerializeField]
    private TMP_Text nickName;

    [SerializeField]
    private Toggle VisibleCheckToggle;

    public Dictionary<string, RoomInfo> cachedRoomList;
    public Dictionary<string, GameObject> roomListEntries;

    private void Awake()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    private void OnEnable()
    {
        createRoomPanel.SetActive(false);
        nickName.text = PhotonNetwork.LocalPlayer.NickName;
    }

    public void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry);
        }

        roomListEntries.Clear();
    }

    public void UpdateCachedRoomList(List<RoomInfo> roomList)
    {

        foreach (RoomInfo info in roomList)
        {
            // Open 상태가 바뀐 경우 + Visible이 바뀐경우 + 룸 리스트에서 제거된 경우
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // 캐싱된 룸 인포 업데이트
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            
            // 새로운 방이면 리스트에 추가
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }

    }

    public void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(roomEntryPrefab, roomContent);
            
            entry.GetComponent<RoomEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }


    public void OnRandomMatchButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLobbyButtonClicked()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnLogoutButtonClicked()
    {
        PhotonNetwork.Disconnect();
    }

    public void OnCreateRoomButtonClicked()
    {
        createRoomPanel.SetActive(true);
    }

    public void OnCreateRoomCancelButtonClicked()
    {
        roomNameInputField.text = "";
        MaxPlayerInputField.text = "";
        VisibleCheckToggle.isOn = false;
        createRoomPanel.SetActive(false);
    }

    public void OnCustomizeButtonClicked()
    {
        customizePanel.SetActive(true);
    }


    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = string.Format("{0}님의 방", PhotonNetwork.LocalPlayer.NickName);

        int maxPlayer = MaxPlayerInputField.text == "" ? 4 : int.Parse(MaxPlayerInputField.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 4);

        bool Visible = VisibleCheckToggle.isOn == true ? false : true;

        RoomOptions options = new RoomOptions() { MaxPlayers = (byte)maxPlayer, IsVisible = Visible };
        PhotonNetwork.CreateRoom(roomName, options, null);
        roomNameInputField.text = "";
        MaxPlayerInputField.text = "";
        VisibleCheckToggle.isOn = false;
        createRoomPanel.SetActive(false);
        
    }

}
