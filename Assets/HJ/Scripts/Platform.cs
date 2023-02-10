using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class Platform : MonoBehaviour
    {
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.parent = transform;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.transform.parent = null;
            }
        }
    }
}

