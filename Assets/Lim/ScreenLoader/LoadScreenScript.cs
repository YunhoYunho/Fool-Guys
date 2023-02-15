using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenScript : MonoBehaviour
{
    private Animator anim;
    private Canvas canvas;
    private PhotonView pv;
    private string Loaded = "Loaded";
    private string Started = "Start";
    public bool isStarted;
    public bool isInGame;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!isStarted && !isInGame)
            SetOpenDoor();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
             LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if (!isStarted)
        {
            pv.RPC("SetOpenDoor", RpcTarget.All);
        }
        else
        {
            pv.RPC("SetCloseDoor", RpcTarget.All);
        }

        //SceneManager.LoadScene();
        //TODO : 로드 해야 할 씬 입력
    }

    [PunRPC]
    public void SetCloseDoor()
    {
        canvas.planeDistance = 1;
        canvas.sortingLayerName = "On";
        anim.SetTrigger(Started);
        anim.ResetTrigger(Loaded);
        isStarted = !isStarted;
    }

    [PunRPC]
    public void SetOpenDoor()
    {
        if (!isInGame)
        {
            canvas.planeDistance = 110;
            canvas.sortingLayerName = "Default";
        }

        anim.SetTrigger(Loaded);
        anim.ResetTrigger(Started);
        isStarted = !isStarted;
    }


}