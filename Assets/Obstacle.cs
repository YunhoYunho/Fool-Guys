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
                pc.EnterRagdoll();
                Debug.Log("PlayerController 감지 후 OnHit 실행");
            }

            if (rigid != null)
            {
                rigid.AddExplosionForce(Force, transform.position, radius);
                Debug.Log("rigidbody 감지 후 힘을 가함");
            }
        }
    }

    private void Knockback()
    {
        
    }
}
