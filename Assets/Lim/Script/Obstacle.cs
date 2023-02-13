using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float Force, radius;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();
            
            //Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();

            if (pc != null)
            {
                pc.gameObject.GetComponent<PhotonView>().RPC("OnHit", RpcTarget.All);
            }

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

    IEnumerator DelayAddForce(Rigidbody target)
    {
        yield return new WaitForSeconds(0.1f);
        target.AddExplosionForce(Force, transform.position, radius);
    }
}
