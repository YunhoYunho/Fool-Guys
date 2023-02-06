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
    private TMP_Text roomName;
    [SerializeField]
    private TMP_Text currentPlayer;
    [SerializeField]
    private Button joinRoomButton;

    public void Initialized(string name, int currentPlayers, byte maxPlayers)
    {
        roomName.text = name;
        currentPlayer.text = string.Format("{0} / {1}", currentPlayers, maxPlayers);
        joinRoomButton.interactable = currentPlayers < maxPlayers;
    }

    public void OnJoinButtonClicked()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(roomName.text);
    }

}
