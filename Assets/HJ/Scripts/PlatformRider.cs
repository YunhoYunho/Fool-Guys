using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRider : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        if(collision.gameObject.GetComponentInParent<Platform>())
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        if (collision.gameObject.GetComponentInParent<Platform>())
        {
            transform.parent = null;
        }
    }
}
