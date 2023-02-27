using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowZone : MonoBehaviour
{
    [SerializeField]
    private float power;
    public float Power { get { return power; } set { power = value; } }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * power, ForceMode.Force);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            Rigidbody rigid = other.gameObject.GetComponent<Rigidbody>();


            //if (pc != null)
            //{
            //    if (pc.getUp == true) return;

            //    //pc.gameObject.GetComponent<PhotonView>().RPC("OnHit", RpcTarget.All);
            //}

            if (rigid != null)
            {
                PlayerController pc = other.gameObject.GetComponent<PlayerController>();

                if (pc != null)
                {
                    if (pc.getUp) return;
                }

                Rigidbody[] rig = other.gameObject.GetComponentsInChildren<Rigidbody>();

                foreach (Rigidbody r in rig)
                {
                    StartCoroutine(DelayAddForce(r));
                }

            }
        }

        IEnumerator DelayAddForce(Rigidbody target)
        {
            yield return new WaitForSeconds(0.1f);
            target.AddForce(transform.up * power / 3, ForceMode.Force);
        }
    }
}
