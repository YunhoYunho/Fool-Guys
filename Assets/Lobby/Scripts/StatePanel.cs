using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatePanel : MonoBehaviour
{
    public static StatePanel Instance { get; private set; }

    private Photon.Realtime.ClientState state;

    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private TMP_Text textPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (state == PhotonNetwork.NetworkClientState)
            return;

        state = PhotonNetwork.NetworkClientState;

        TMP_Text text = Instantiate(textPrefab, content);
        text.text = string.Format(string.Format("[Photon NetworkState] {0}", state.ToString()));
        Debug.Log(string.Format("[Photon NetworkState] {0}", state.ToString()));
    }

    public void AddMessage(string message)
    {
        TMP_Text text = Instantiate(textPrefab, content);
        text.text = string.Format(string.Format("[Photon NetworkState] {0}", message));
        Debug.Log(string.Format("[Photon NetworkState] {0}", message));
    }

}
