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
            Knockback();
        }
    }

    private void Knockback()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rigid = colliders[i].GetComponent<Rigidbody>();

            if (rigid != null)
            {
                Debug.Log("충돌 발동!");
                rigid.AddExplosionForce(Force, transform.position, radius);
                Destroy(gameObject);
            }
        }
    }
}
