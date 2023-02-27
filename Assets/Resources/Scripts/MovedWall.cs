using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedWall : MonoBehaviour
{

    private Animator anim;

    private PhotonView pv;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        StartCoroutine(DelaySet());
    }

    [PunRPC]
    public void StartAnim()
    {
        anim.SetBool("Start", true);
    }

    IEnumerator DelaySet()
    {
        yield return new WaitForSeconds(3f);
        if (PhotonNetwork.IsMasterClient) 
        {
            pv.RPC("StartAnim", RpcTarget.All);
        }
    }

}
