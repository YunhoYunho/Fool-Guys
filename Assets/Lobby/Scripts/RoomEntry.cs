using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    //public void Initialize(string name, byte currentPlayers, byte maxPlayers)
    //{
    //    roomName = name;

    //    RoomNameText.text = name;
    //    RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
    //}




    [SerializeField]
    private TMP_Text roomNumber;
    [SerializeField]
    private TMP_Text roomName;
    [SerializeField]
    private TMP_Text currentPlayer;
    [SerializeField]
    private Button joinRoomButton;

    public void Initialize(/*int num, */string name, int currentPlayers, byte maxPlayers)
    {
        //roomNumber.text = string.Format("# {0}", num);
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
