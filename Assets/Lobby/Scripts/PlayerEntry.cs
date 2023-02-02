using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField]
    private TMP_Text playerReady;
    [SerializeField]
    private Button playerReadyButton;

    //private bool ready;
    private int ownerId;

    public void Initialize(int id, string name)
    {
        ownerId = id;
        playerName.text = name;
        playerReady.text = "";
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            playerReadyButton.gameObject.SetActive(false);
        }

    }

    public void OnReadyButtonClicked()
    {
        object isPlayerReady;
        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out isPlayerReady))
            isPlayerReady = false;

        bool ready = (bool)isPlayerReady;
        SetPlayerReady(!ready);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { "Ready", !ready } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);


        //object isPlayerReady;

        //if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out isPlayerReady))
        //    isPlayerReady = false;


        //bool ready = (bool)isPlayerReady;
        //SetPlayerReady(!ready);
    }

    public void SetPlayerReady(bool ready)
    {
        playerReady.text = ready ? "Ready" : "";
        PhotonNetwork.AutomaticallySyncScene = ready;

        //ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable()
        //{
        //    { "Ready", ready }
        //};

        //PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        //this.ready = ready;
        //playerReady.text = ready ? "Ready" : "";
    }
}
