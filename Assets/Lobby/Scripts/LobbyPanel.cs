using JetBrains.Annotations;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private GameObject createRoomPanel;
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

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;

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
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }

        #region
        //StartCoroutine(DelayUpdateRoomList(roomList));

        //for (int i = 0; i < roomEntries.Count; i++)
        //{
        //    RoomEntry room = roomEntries[i];
        //    Destroy(room.gameObject);
        //}

        //roomEntries.Clear();

        //for (int i = 0; i < roomList.Count; i++)
        //{
        //    RoomInfo room = roomList[i];
        //    RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
        //    entry.Initialized(roomEntries.Count + 1, room.Name, room.PlayerCount, room.MaxPlayers);
        //    roomEntries.Add(entry);
        //}



        //foreach (RoomEntry room in roomEntries)
        //{
        //    Destroy(room.gameObject);
        //}

        //roomEntries.Clear();

        //foreach (RoomInfo room in roomList)
        //{
        //    RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
        //    entry.Initialized(roomEntries.Count + 1, room.Name, room.PlayerCount, room.MaxPlayers);
        //    roomEntries.Add(entry);
        //}

        //StartCoroutine(DelayUpdateRoomList(roomList));
        #endregion
    }

    public void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            //RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
            //entry.transform.localScale = Vector3.one;
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
        //Panel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    public void OnCreateRoomCancelButtonClicked()
    {
        //Panel.SetActive(true);
        createRoomPanel.SetActive(false);
    }

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = string.Format("{0}¥‘¿« πÊ", PhotonNetwork.LocalPlayer.NickName);

        int maxPlayer = MaxPlayerInputField.text == "" ? 8 : int.Parse(MaxPlayerInputField.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);

        RoomOptions options = new RoomOptions() { MaxPlayers = (byte)maxPlayer, IsVisible = true };
        PhotonNetwork.CreateRoom(roomName, options, null);
        createRoomPanel.SetActive(false);
    }

    //private IEnumerator DelayUpdateRoomList(List<RoomInfo> roomList)
    //{
    //    yield return new WaitForSeconds(1f);

    //    for (int i = 0; i < roomEntries.Count; i++)
    //    {
    //        RoomEntry room = roomEntries[i];
    //        Destroy(room.gameObject);
    //    }

    //    roomEntries.Clear();

    //    for (int i = 0; i < roomList.Count; i++)
    //    {
    //        RoomInfo room = roomList[i];
    //        RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
    //        entry.Initialized(roomEntries.Count + 1, room.Name, room.PlayerCount, room.MaxPlayers);
    //        roomEntries.Add(entry);
    //    }
    //}
}
