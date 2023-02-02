using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("À¸¿¨");
            collision.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(100,0,0), ForceMode.Impulse);
        }
    }
}
