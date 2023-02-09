using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField]
    private Image panelImage;
    [SerializeField]
    private TMP_Text roomState;
    [SerializeField]
    private TMP_Text roomName;
    [SerializeField]
    private TMP_Text currentPlayer;
    [SerializeField]
    private Button joinRoomButton;

    public void Initialize(string name, int currentPlayers, byte maxPlayers)
    {
        roomName.text = name;
        currentPlayer.text = string.Format("{0} / {1}", currentPlayers, maxPlayers);
        joinRoomButton.interactable = currentPlayers < maxPlayers;
        string state = joinRoomButton.interactable ? "<color=white>[대기]" : "<color=black>[포화]";
        roomState.text = string.Format(state);
    }

    public void OnJoinButtonClicked()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(roomName.text);
    }

}
