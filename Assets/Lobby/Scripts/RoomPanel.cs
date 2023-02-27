using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    [SerializeField]
    private PlayerEntry playerEntryPrefab;

    [SerializeField]
    private RectTransform playerContent;

    [SerializeField]
    private TMP_Text NickName;

    [SerializeField]
    private TMP_Text RoomName;

    [SerializeField]
    private Button startButton;

    private List<PlayerEntry> playerEntries;

    private LoadScreenScript loadScene;


    private void Awake()
    {
        playerEntries = new List<PlayerEntry>();
        loadScene = GameObject.Find("LoadingCanvas").GetComponent<LoadScreenScript>();
    }

    private void Start()
    {
        UpdateRoomState();
    }

    public void UpdateRoomState()
    {
        foreach (PlayerEntry entry in playerEntries) 
        {
            Destroy(entry.gameObject);
        }

        playerEntries.Clear();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
            entry.Initialize(player.ActorNumber, player.NickName);

            object isPlayerReady;
            if (player.CustomProperties.TryGetValue("Ready", out isPlayerReady))
            {
                entry.SetPlayerReady((bool)isPlayerReady);
            }
            //entry.SetPlayerColor();
            playerEntries.Add(entry);
        }

    }

    public void UpdateLocalPlayerPropertiesUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        startButton.gameObject.SetActive(CheckPlayerReady());
    }

    public bool CheckPlayerReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue("Ready", out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                    return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void RoomNameSet(string roomname, string nickname)
    {
        RoomName.text = roomname;
        NickName.text = nickname;
    }

    public void OnStartButtonClicked()
    {
        //PhotonNetwork.CurrentRoom.IsVisible = false;
        //PhotonNetwork.LoadLevel("SW_Scene");
        //PhotonNetwork.LoadLevel("Stage1");
        //SceneManager.LoadScene

        StartCoroutine(DelayStart());
    }

    public void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
        //PhotonNetwork.JoinLobby();
    }

    public IEnumerator DelayStart()
    {
        loadScene.LoadNextLevel();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        yield return new WaitForSeconds(3f);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Test_Map");

    }


}
