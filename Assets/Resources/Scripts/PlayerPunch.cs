using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();

        if (!pv.IsMine) Destroy(this);
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        //if (pv.IsMine) return;

        if (collision.gameObject.GetComponent<PhotonView>().IsMine) return;
        else if (collision.gameObject.GetComponent<PhotonView>().IsMine == null) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();

            if (pc != null)
            {
                pc.photonView.RPC("OnHit", RpcTarget.All);

                if (rigid != null)
                {
                    Rigidbody[] rig = collision.gameObject.GetComponentsInChildren<Rigidbody>();

                    foreach (Rigidbody r in rig)
                    {
                        StartCoroutine(DelayAddForce(r));
                    }
                }
            }

        }
    }

    IEnumerator DelayAddForce(Rigidbody target)
    {
        int Force = 2000, radius = 20;
        yield return new WaitForSeconds(0.1f);
        target.AddExplosionForce(Force, transform.position, radius);


    }

}
