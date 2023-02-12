using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TMP_Text infoText;

    [SerializeField]
    private CinemachineFreeLook playerCam;

    private SkinnedMeshRenderer[] Skincolor;

    private Color playerColor;

    private PhotonView pv;

    private void Awake()
    {
    }

    private void Start()
    {
        pv = GetComponent<PhotonView>();

        if (PhotonNetwork.InRoom)
        {
            Hashtable props = new Hashtable { { "Load", true } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            //StartCoroutine(DelayGameStart());
            //TestGameStart();
        }


    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //StartCoroutine(SpawnStone());
        }
    }


    public override void OnJoinedRoom()
    {
        // 테스트 게임 시작
        StartCoroutine(DelayGameStart());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(string.Format("Disconnected : {0}", cause.ToString()));
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        Debug.Log(string.Format("OnLeftRoom"));
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Load"))
        {
            if (AllPlayersLoadLevel())
            {
                GameStart();
            }

            else
            {
                PrintInfo("Waiting Other Players...");
            }
        }
    }

    private void GameStart()
    {
        PrintInfo("Game Start");
        float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
        float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
        float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
        //Vector3 position = new Vector3(x, 0.0f, z);
        //Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);
        Vector3 position = new Vector3(0, 10f, 0);
        Quaternion rotation = Quaternion.Euler(0.0f, 0f, 0.0f);

        GameObject Player = PhotonNetwork.Instantiate("Player", position, rotation, 0);
        playerCam.LookAt = Player.transform;
        playerCam.Follow = Player.transform;

        Player.gameObject.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllViaServer);

        if (PhotonNetwork.IsMasterClient)
        {
            //StartCoroutine(SpawnStone());
        }
        
    }


    private void TestGameStart()
    {
        PrintInfo("");
        float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
        float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
        float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
        //Vector3 position = new Vector3(x, 0.0f, z);
       // Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);
        Vector3 position = new Vector3(0, 15f, 0);
        Quaternion rotation = Quaternion.Euler(0.0f, 0f, 0.0f);

        GameObject Player = PhotonNetwork.Instantiate("Player", position, rotation, 0);
        playerCam.LookAt = Player.transform;
        playerCam.Follow = Player.transform;

        Player.gameObject.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllViaServer);

        if (PhotonNetwork.IsMasterClient)
        {
            //StartCoroutine(SpawnStone());
        }
        
    }


    private bool AllPlayersLoadLevel()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object playerLoaded;
            if (player.CustomProperties.TryGetValue("Load", out playerLoaded))
            {
                if ((bool)playerLoaded)
                {
                    continue;
                }
                else
                    return false;
            }
            else
                return false;
        }

        return true;
    }

    private void PrintInfo(string info)
    {
        Debug.Log(info);
        infoText.text = info;
    }

    private IEnumerator DelayGameStart()
    {
        yield return new WaitForSeconds(0.1f);
        TestGameStart();
    }

    private IEnumerator SpawnStone()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));

            Vector2 direction = Random.insideUnitCircle;
            Vector3 position = Vector3.zero;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Make it appear on the left/right side
                position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
            }
            else
            {
                // Make it appear on the top/bottom
                position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
            }

            // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
            position -= position.normalized * 0.1f;


            Vector3 force = -position.normalized * 1000.0f;
            Vector3 torque = Random.insideUnitSphere * Random.Range(100.0f, 300.0f);
            object[] instantiationData = { force, torque };

            if (Random.Range(0, 10) < 5)
            {
                PhotonNetwork.InstantiateRoomObject("BigStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
            }
            else
            {
                PhotonNetwork.InstantiateRoomObject("SmallStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
            }
        }
    }
}
