using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoingTrigger : MonoBehaviour
{
    public enum Type { Random, Trigger }
    public Type type;

    //public UnityEvent OnCollision;

    private Animator anim;

    private PhotonView pv;

    private Coroutine randomCoroutine;
    [SerializeField] private float checkTime = 2f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (type != Type.Random)
            return;

        StartCoroutine(DetectMClient());
    }


    private IEnumerator DetectMClient()
    {
        yield return new WaitForSeconds(3f);
        if (PhotonNetwork.IsMasterClient)
            DelayRandomActivate();
    }

    public void DelayRandomActivate()
    {
        randomCoroutine = StartCoroutine(RandomActivate());
    }

    [PunRPC]
    public void ActivateFunc()
    {
        anim.SetTrigger("Activate");

    }

    private IEnumerator RandomActivate()
    {
        while(true)
        {
            checkTime = Random.Range(0.5f, 4f);
            yield return new WaitForSeconds(checkTime);

            int rand = Random.Range(0, 2);
            //Debug.Log("·£´ý°ª: " + rand);
            if (rand == 0)
            {
                pv.RPC("ActivateFunc", RpcTarget.All);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (type != Type.Trigger)
            return;

        if(collision.transform.CompareTag("Player"))
        {
            anim.SetTrigger("Active");
        }    
    }
}
