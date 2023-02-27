using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverInput : MonoBehaviour
{

    private void Start()
    {
        object isPlayerReady;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Ready", out isPlayerReady))
            isPlayerReady = false;

        bool ready = (bool)isPlayerReady;

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { "Ready", ready } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(DelayOutGame());
    }

    private void Escape()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.LoadLevel("LobbyScene");
        }

        Destroy(this);
    }

    private IEnumerator DelayOutGame()
    {
        yield return new WaitForSeconds(15f);
        Escape();
    }
}
