using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [SerializeField]
    private CinemachineVirtualCamera freeCam;
    //private CinemachineFreeLook freeCam;

    private PhotonView pv;

    public GameObject NowRespawnArea;
    public GameObject TestRespawnArea;

    public LoadScreenScript loadscreen;

    public WinAndLoseScript winAndLoseScreen;

    public GetSetGoScript countdown;

    public GameObject GoToTheEnd_UI;

    public enum Team { None, Red, Blue };
    public Team team;
    public string myTeam;

    public List<string> Red_TeamList;
    public List<string> Blue_TeamList;

    public int RedTeamNeedGoalPoint = 0;
    public int BlueTeamNeedGoalPoint = 0;

    public bool onGameStart;

    public bool isTestGame;

    public int GoalInCount;

    public string WinTeam;  

    public GameObject GameOverSTAGE;
    public GameObject Winner1;
    public TMP_Text Winner1_Team;
    public TMP_Text Winner1_Nickname;
    public GameObject Winner2;
    public TMP_Text Winner2_Team;
    public TMP_Text Winner2_Nickname;

    public GameObject GameBGM;
    public GameObject VictoryBGM;

    private PlayerNumbering playerNumbering;

    public List<string> Winner;
    public List<string> Loser;

    private GameObject myPlayer; 

    Vector3 spawn_1p_Pos, spawn_2p_Pos, spawn_3p_Pos, spawn_4p_Pos;

    //public int PlayerNum;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        //PlayerNum = (int)PhotonNetwork.CurrentRoom.PlayerCount;

    }

    private void Start()
    {

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

        spawn_1p_Pos = GameObject.Find("Start Point 1P").transform.position;
        spawn_2p_Pos = GameObject.Find("Start Point 2P").transform.position;
        spawn_3p_Pos = GameObject.Find("Start Point 3P").transform.position;
        spawn_4p_Pos = GameObject.Find("Start Point 4P").transform.position;
        playerNumbering = GameObject.Find("PlayerNumbering").GetComponent<PlayerNumbering>();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestRespawnPointSet(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestRespawnPointSet(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TestRespawnPointSet(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TestRespawnPointSet(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TestRespawnPointSet(5);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        }
    }

    public void TestRespawnPointSet(int area)
    {
        GameObject Left_1 = GameObject.Find("RespawnArea1_Left");
        GameObject Left_2 = GameObject.Find("RespawnArea2_Left");
        GameObject Left_3 = GameObject.Find("RespawnArea3");
        GameObject Left_4 = GameObject.Find("RespawnArea4");
        GameObject Left_5 = GameObject.Find("RespawnArea5");


        switch (area)
        {
            case 1: TestRespawnArea = Left_1; break;
            case 2: TestRespawnArea = Left_2; break;
            case 3: TestRespawnArea = Left_3; break;
            case 4: TestRespawnArea = Left_4; break;
            case 5: TestRespawnArea = Left_5; break;
            default: break;
        }
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("TestRoom" + Random.Range(1000,10000), new RoomOptions() { MaxPlayers = 4 }, null);
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
                StartCoroutine(AllReady());
            }

            else
            {
                Debug.Log("Waiting others");
                //PrintInfo("Waiting Other Players...");
            }
        }
    }

    IEnumerator AllReady()
    {
        yield return new WaitForSeconds(1f);
        freeCam.gameObject.SetActive(false);
        loadscreen.gameObject.GetComponent<PhotonView>().RPC("SetOpenDoor", RpcTarget.All);
        yield return new WaitForSeconds(1f);
        StartCoroutine(CountDown());

    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        countdown.gameObject.GetComponent<PhotonView>().RPC("StartAnimation", RpcTarget.All);
        yield return new WaitForSeconds(3f);
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("GameStartSend", RpcTarget.All);
        }
    }


    private void GameStart()
    {
        PrintInfo("");

        //int PlayerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        //int PlayerNum = PlayerNumbering.RoomPlayerIndexedProp;
        //string PlayerNum = PlayerNumbering.RoomPlayerIndexedProp;
        int PlayerNum = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        

        Vector3 position;
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);


        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            switch (PlayerNum)
            {
                case 0: position = spawn_2p_Pos; team = Team.Red; myTeam = "Red"; break;
                case 1: position = spawn_3p_Pos; team = Team.Blue; myTeam = "Blue"; break;
                default: position = spawn_2p_Pos; break;
            }
        }

        else
        {
            switch (PlayerNum)
            {
                case 0: position = spawn_1p_Pos; team = Team.Red; myTeam = "Red"; break;
                case 1: position = spawn_2p_Pos; team = Team.Red; myTeam = "Red"; break;
                case 2: position = spawn_3p_Pos; team = Team.Blue; myTeam = "Blue"; break;
                case 3: position = spawn_4p_Pos; team = Team.Blue; myTeam = "Blue"; break;
                default: position = spawn_1p_Pos; break;
            }
        }

        GameObject Player = PhotonNetwork.Instantiate("Player", position, rotation, 0);
        //pv.RPC("playerNumCount", RpcTarget.All, -1);
        playerCam.LookAt = Player.transform;
        playerCam.Follow = Player.transform;

        Player.gameObject.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.All);
        Player.gameObject.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.All);

        if (team == Team.Red)
        {
            Player.gameObject.GetComponent<PhotonView>().RPC("SetTeam", RpcTarget.All, "Red");

        }
        else
        {
            Player.gameObject.GetComponent<PhotonView>().RPC("SetTeam", RpcTarget.All, "Blue");
        }

        myPlayer = Player;

    }

    //[PunRPC]
    //public void playerNumCount(int val)
    //{
    //    PlayerNum += val;
    //}


    private void TestGameStart()
    {
        PrintInfo("");

        int PlayerNum = PhotonNetwork.LocalPlayer.GetPlayerNumber();

        Vector3 position;
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            switch (PlayerNum)
            {
                case 0: position = spawn_2p_Pos; team = Team.Red; myTeam = "Red"; break;
                case 1: position = spawn_3p_Pos; team = Team.Blue; myTeam = "Blue"; break;
                default: position = spawn_3p_Pos; break;
            }
        }
        else
        {
            switch (PlayerNum)
            {
                case 0: position = spawn_1p_Pos; team = Team.Red; myTeam = "Red"; break;
                case 1: position = spawn_2p_Pos; team = Team.Red; myTeam = "Red"; break;
                case 2: position = spawn_3p_Pos; team = Team.Blue; myTeam = "Blue"; break;
                case 3: position = spawn_4p_Pos; team = Team.Blue; myTeam = "Blue"; break;
                default: position = spawn_3p_Pos; break;
            }
        }

        GameObject Player = PhotonNetwork.Instantiate("Player", position, rotation, 0);
        //pv.RPC("playerNumCount", RpcTarget.All, -1);
        playerCam.LookAt = Player.transform;
        playerCam.Follow = Player.transform;

        Player.gameObject.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.All);
        Player.gameObject.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.All);

        if (team == Team.Red)
        {
            Player.gameObject.GetComponent<PhotonView>().RPC("SetTeam", RpcTarget.All, "Red");

        }
        else
        {
            Player.gameObject.GetComponent<PhotonView>().RPC("SetTeam", RpcTarget.All, "Blue");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            isTestGame = true;
            //pv.RPC("GameStartSend", RpcTarget.All);
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

    [PunRPC]
    public void GoalPointChange(string team, int score)
    {
        switch (team)
        {
            case "Red": RedTeamNeedGoalPoint += score; break;
            case "Blue": BlueTeamNeedGoalPoint += score; break;
            default: break;
        }

        DetectGameOver();
    }

    [PunRPC]
    public void AddTeamList(string team, string nick)
    {
        switch (team)
        {
            case "Red": Red_TeamList.Add(nick); break;
            case "Blue": Blue_TeamList.Add(nick); break;
            default: break;
        }

    }

    private void DetectGameOver()
    {
        if (!PhotonNetwork.IsMasterClient || !onGameStart) return;

        if (!isTestGame)
        {
            if (GoalInCount != 0)
            {
                if (RedTeamNeedGoalPoint <= 0)
                {
                    WinTeam = "Red";
                    GameOverFunc();

                }

                else if (BlueTeamNeedGoalPoint <= 0)
                {
                    WinTeam = "Blue";
                    GameOverFunc();

                }
            }
        }

        else
        {
            if (RedTeamNeedGoalPoint <= 0)
            {
                WinTeam = "Red";
                GameOverFunc();
            }
        }

    }

    [PunRPC]
    public void GameStartSend()
    {
        onGameStart = true;
    }

    public void GameOverFunc()
    {
        
        StartCoroutine(GameOverCloseDoor());

        //pv.RPC("GameOverCoroutineOn", RpcTarget.All);
    }

    [PunRPC]
    public void GameOverCoroutineOn()
    {
        StartCoroutine(GameOverCloseDoor());
    }


    [PunRPC]
    public void GameOverFirst(string Winteam)
    {
        
        switch (Winteam)
        {
            case "Red":

                for (int i = 0; i < Red_TeamList.Count; i++)
                {
                    if (PhotonNetwork.LocalPlayer.NickName == Red_TeamList[i])
                    {
                        // RPC win animation
                        //winAndLoseScreen.GetComponent<PhotonView>().RPC("PlayWinAnim", RpcTarget.All);
                        winAndLoseScreen.PlayWinAnim();
                    }

                    else
                    {
                        // RPC lose animation
                        //winAndLoseScreen.GetComponent<PhotonView>().RPC("PlayLoseAnim", RpcTarget.All);
                        winAndLoseScreen.PlayLoseAnim();
                    }
                }
                break;

            case "Blue":

                for (int i = 0; i < Blue_TeamList.Count; i++)
                {
                    if (PhotonNetwork.LocalPlayer.NickName == Blue_TeamList[i])
                    {
                        // RPC win animation
                        //winAndLoseScreen.GetComponent<PhotonView>().RPC("PlayWinAnim", RpcTarget.All);
                        winAndLoseScreen.PlayWinAnim();
                    }

                    else
                    {
                        // RPC lose animation
                        //winAndLoseScreen.GetComponent<PhotonView>().RPC("PlayLoseAnim", RpcTarget.All);
                        winAndLoseScreen.PlayLoseAnim();
                    }
                }

                break;

            default: break;

        }

        if (myPlayer != null)
            myPlayer.GetComponent<PlayerController>().DestroyPlayer();

        onGameStart = false;
        //Time.timeScale = 0;
    }


    [PunRPC]
    public void GameOver(string team)
    {
        GoToTheEnd_UI.SetActive(false);
        freeCam.enabled = false;
        playerCam.gameObject.SetActive(false);
        GameOverSTAGE.SetActive(true);
        Debug.Log("GameOver 접근");

        switch (team)
        {
            case "Red":
                Winner1_Team.text = "<color=red>[RED]</color>";
                Winner1_Nickname.text = Red_TeamList[0] + "\n<size=0.1>▼";

                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    Debug.Log(player.NickName);
                    if (player.NickName == Red_TeamList[0])
                    {
                        Debug.Log("접근됐어용");
                        object R, G, B;

                        if (player.CustomProperties.TryGetValue("R", out R) &&
                            player.CustomProperties.TryGetValue("G", out G) &&
                            player.CustomProperties.TryGetValue("B", out B))
                        {
                            SkinnedMeshRenderer[] Skincolor = Winner1.GetComponentsInChildren<SkinnedMeshRenderer>();
                            Color playercolor = new Color((float)R, (float)G, (float)B);
                            for (int i = 0; i < Skincolor.Length; i++)
                            {
                                Skincolor[i].material.color = playercolor;
                            }
                        }
                    }
                }

                if (Red_TeamList.Count >= 2)
                {
                    Winner2_Team.text = "<color=red>[RED]</color>";
                    Winner2_Nickname.text = Red_TeamList[1] + "\n<size=0.1>▼";

                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        if (player.NickName == Red_TeamList[1])
                        {
                            object R, G, B;

                            if (player.CustomProperties.TryGetValue("R", out R) &&
                                player.CustomProperties.TryGetValue("G", out G) &&
                                player.CustomProperties.TryGetValue("B", out B))
                            {
                                SkinnedMeshRenderer[] Skincolor = Winner2.GetComponentsInChildren<SkinnedMeshRenderer>();
                                Color playercolor = new Color((float)R, (float)G, (float)B);
                                for (int i = 0; i < Skincolor.Length; i++)
                                {
                                    Skincolor[i].material.color = playercolor;
                                }
                            }
                        }
                    }


                }
                else Winner2.SetActive(false);

                break;

            case "Blue":
                Winner1_Team.text = "<color=blue>[BLUE]</color>";
                Winner1_Nickname.text = Blue_TeamList[0] + "\n<size=0.1>▼";
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.NickName == Blue_TeamList[0])
                    {
                        object R, G, B;

                        if (player.CustomProperties.TryGetValue("R", out R) &&
                            player.CustomProperties.TryGetValue("G", out G) &&
                            player.CustomProperties.TryGetValue("B", out B))
                        {
                            SkinnedMeshRenderer[] Skincolor = Winner1.GetComponentsInChildren<SkinnedMeshRenderer>();
                            Color playercolor = new Color((float)R, (float)G, (float)B);
                            for (int i = 0; i < Skincolor.Length; i++)
                            {
                                Skincolor[i].material.color = playercolor;
                            }
                        }
                    }
                }

                if (Blue_TeamList.Count >= 2)
                {
                    Winner2_Team.text = "<color=blue>[BLUE]</color>";
                    Winner2_Nickname.text = Blue_TeamList[1] + "\n<size=0.1>▼";

                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        if (player.NickName == Blue_TeamList[1])
                        {
                            object R, G, B;

                            if (player.CustomProperties.TryGetValue("R", out R) &&
                                player.CustomProperties.TryGetValue("G", out G) &&
                                player.CustomProperties.TryGetValue("B", out B))
                            {
                                SkinnedMeshRenderer[] Skincolor = Winner2.GetComponentsInChildren<SkinnedMeshRenderer>();
                                Color playercolor = new Color((float)R, (float)G, (float)B);
                                for (int i = 0; i < Skincolor.Length; i++)
                                {
                                    Skincolor[i].material.color = playercolor;
                                }
                            }
                        }
                    }

                }
                else Winner2.SetActive(false);

                break;
        }

        freeCam.enabled = false;
        //Time.timeScale = 1;

    }

    [PunRPC]
    public void AddWinnerNicknameList(string name)
    {
        Winner.Add(name);
    }

    public void SetCheckPoint(GameObject area)
    {
        NowRespawnArea = area;
    }

    [PunRPC]
    public void CheckGoalIn(int count)
    {
        GoalInCount += count;
        DetectGameOver();
    }


    private void PrintInfo(string info)
    {
        Debug.Log(info);
        infoText.text = info;
    }

    [PunRPC]
    public void BGM_CHANGE()
    {
        GameBGM.SetActive(false);
        VictoryBGM.SetActive(true);
    }

    private IEnumerator DelayGameStart()
    {
        yield return new WaitForSeconds(0.1f);
        TestGameStart();
        StartCoroutine(AllReady());
    }

    private IEnumerator GameOverCloseDoor()
    {
        pv.RPC("GameOverFirst", RpcTarget.All, WinTeam);
        //GameOverFirst(WinTeam);
        yield return new WaitForSecondsRealtime(2f);
        loadscreen.gameObject.GetComponent<PhotonView>().RPC("SetCloseDoor", RpcTarget.All);
        //loadscreen.SetCloseDoor();
        yield return new WaitForSecondsRealtime(2f);
        //GameOver(WinTeam);
        pv.RPC("GameOver", RpcTarget.All, WinTeam);
        yield return new WaitForSecondsRealtime(3f);
        //loadscreen.SetOpenDoor();
        pv.RPC("BGM_CHANGE", RpcTarget.All);
        loadscreen.gameObject.GetComponent<PhotonView>().RPC("SetOpenDoor", RpcTarget.All);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
