using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviourPun
{
    [SerializeField]
    private GameObject PlayerModel;
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField]
    private TMP_Text playerReady;
    [SerializeField]
    private Button playerReadyButton;

    private SkinnedMeshRenderer[] Skincolor;

    private Color playerColor;

    private GameObject lobbyManager;

    private int ownerId;

    private void Awake()
    {
        lobbyManager = GameObject.Find("LobbyManager");
    }

    public void Initialize(int id, string name)
    {
        ownerId = id;
        playerName.text = name;
        playerReady.text = "";

        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            playerReadyButton.gameObject.SetActive(false);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == id)
            {
                object R, G, B;

                if (player.CustomProperties.TryGetValue("R", out R) &&
                    player.CustomProperties.TryGetValue("G", out G) &&
                    player.CustomProperties.TryGetValue("B", out B))
                {
                    Skincolor = PlayerModel.GetComponentsInChildren<SkinnedMeshRenderer>();
                    Color playercolor = new Color((float)R, (float)G, (float)B);
                    for (int i = 0; i < Skincolor.Length; i++)
                    {
                        Skincolor[i].material.color = playercolor;
                    }
                }
            }
        }

        //if (PhotonNetwork.LocalPlayer.ActorNumber == ownerId)
        //{

        //    Player playerMe = PhotonNetwork.LocalPlayer;

        //    object R, G, B;

        //    if (playerMe.CustomProperties.TryGetValue("R", out R) &&
        //        playerMe.CustomProperties.TryGetValue("G", out G) &&
        //        playerMe.CustomProperties.TryGetValue("B", out B))

        //    {
        //        Skincolor = PlayerModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        //        Color playercolor = new Color((float)R, (float)G, (float)B);
        //        for (int i = 0; i < Skincolor.Length; i++)
        //        {
        //            Skincolor[i].material.color = playercolor;
        //        }
        //    }
        //}

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

    }

    public void SetPlayerReady(bool ready)
    {
        playerReady.text = ready ? "<color=#32CD32>준비 완료</color>" : "<color=#8FBC8F>대기 중</color>";
        PhotonNetwork.AutomaticallySyncScene = ready;

    }


}
