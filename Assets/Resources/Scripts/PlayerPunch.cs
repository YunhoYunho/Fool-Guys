using HJ;
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

        //if (!pv.IsMine) Destroy(this);
    }


    private void OnTriggerEnter(Collider collision)
    {
        PhotonView otherPV = collision.gameObject.GetComponent<PhotonView>();

        if (otherPV != null)
        {
            if (otherPV != pv)
            {
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
        }

        if (collision.gameObject.CompareTag("Console"))
        {
            Debug.Log("¡¯¿‘");
            Console console = collision.gameObject.GetComponentInParent<Console>();
            console.InterAction(null);
        }
    }

    IEnumerator DelayAddForce(Rigidbody target)
    {
        int Force = 2000, radius = 20;
        yield return new WaitForSeconds(0.1f);
        target.AddExplosionForce(Force, transform.position, radius);

    }

}
