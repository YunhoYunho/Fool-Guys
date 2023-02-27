using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetSetGoScript : MonoBehaviour
{
    private Animator anim;
    private PhotonView pv;
    private bool startCount = false;

    [SerializeField] private AudioSource Count_Sound;
    [SerializeField] private AudioClip Three;
    [SerializeField] private AudioClip Two;
    [SerializeField] private AudioClip One;
    [SerializeField] private AudioClip Start;

    //public UnityEvent Sound_1;
    //public UnityEvent Sound_2;
    //public UnityEvent Sound_3;


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


    public void play_3()
    {
        Count_Sound.clip = Three;
        Count_Sound.Play();
    }

    public void play_2()
    {
        Count_Sound.clip = Two;
        Count_Sound.Play();
    }

    public void play_1()
    {
        Count_Sound.clip = One;
        Count_Sound.Play();
    }

    public void play_Start()
    {
        Count_Sound.clip = Start;
        Count_Sound.Play();
    }

}
