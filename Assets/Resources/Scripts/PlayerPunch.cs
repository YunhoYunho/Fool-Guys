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

                    Debug.Log(collision.gameObject.GetComponent<PhotonView>().Owner);

                    if (pc != null)
                    {

                        if (rigid != null)
                        {
                            //Vector3 punchDir = 
                            float punchDirX = this.gameObject.transform.parent.transform.position.x;
                            float punchDirY = this.gameObject.transform.parent.transform.position.y;
                            float punchDirZ = this.gameObject.transform.parent.transform.position.z;

                            pc.photonView.RPC("OnHit", RpcTarget.All);
                            otherPV.RPC("BlowAway", RpcTarget.All, punchDirX, punchDirX, punchDirZ);

                            //int Force = 10000, radius = 50;
                            //rigid.AddExplosionForce(Force, transform.position, radius);
                            //Rigidbody[] rig = collision.gameObject.GetComponentsInChildren<Rigidbody>();

                            //foreach (Rigidbody r in rig)
                            //{
                            //    StartCoroutine(DelayAddForce(r));
                            //    //r.AddExplosionForce(Force, transform.position, radius);
                            //}
                        }


                    }


                }
            }

        }

        if (collision.gameObject.CompareTag("Console"))
        {
            Debug.Log("¡¯¿‘");
            Console console = collision.gameObject.GetComponentInParent<Console>();
            //console.InterAction(null);
            StartCoroutine(DelayInterAction(console));
        }
    }
    IEnumerator DelayInterAction(IInteractable interactable)
    {
        yield return new WaitForSeconds(0.1f);
        interactable.InterAction(null);

    }

    IEnumerator DelayAddForce(Rigidbody target)
    {
        int Force = 2000, radius = 20;
        yield return new WaitForSeconds(0.1f);
        target.AddExplosionForce(Force, transform.position, radius);

    }

    IEnumerator DelayOnHit()
    {
        yield return new WaitForSeconds(0.2f);
    }

}
