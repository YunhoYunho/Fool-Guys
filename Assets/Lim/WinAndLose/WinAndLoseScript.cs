using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAndLoseScript : MonoBehaviour
{
    private Animator anim;
    private string win = "Win";
    private string lose = "Lose";

    private PhotonView pv;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void PlayWinAnim()
    {
        anim.SetTrigger(win);
    }

    [PunRPC]
    public void PlayLoseAnim()
    {
        anim.SetTrigger(lose);
    }
}
