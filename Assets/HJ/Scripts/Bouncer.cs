using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private float bouncePower;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Vector3 normal = collision.contacts[0].normal;
            collision.transform.GetComponent<Rigidbody>().AddForce(-normal * bouncePower, ForceMode.Impulse);
        }
    }
}
