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

            if (pc != null)
            {
                //pc.OnHit();
            }

            if (rigid != null)
            {
                Debug.Log("폭발");
                rigid.AddExplosionForce(Force, transform.position, radius);
            }

            //Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    Debug.Log("Player충돌판정 돌입");
            //    PlayerController pc = colliders[i].gameObject.GetComponent<PlayerController>();
            //    Rigidbody rigid = colliders[i].gameObject.GetComponent<Rigidbody>();

            //    if (pc != null)
            //    {
            //        Debug.Log("OnHit 호출");
            //        pc.OnHit();
            //    }

            //    if (rigid != null)
            //    {
            //        rigid.AddExplosionForce(Force, transform.position, radius);
            //    }
            //}
        }
    }
}
