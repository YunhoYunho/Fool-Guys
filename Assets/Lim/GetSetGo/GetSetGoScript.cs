using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSetGoScript : MonoBehaviour
{
    private Animator anim;
    private PhotonView pv;
    private bool startCount = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void StartAnimation()
    {
        startCount = true;
        anim.SetBool("Go", startCount);
        StartCoroutine(ResetAnimation());
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(4.5f);
        startCount= false;
        anim.SetBool("Go", startCount);
    }
}
