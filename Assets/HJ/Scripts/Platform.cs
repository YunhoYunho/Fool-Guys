using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class Platform : MonoBehaviour
    {
        public ObstacleRotator obstacleRotator;
        private PlayerController pc = null;

        private void Awake()
        {
            obstacleRotator = GetComponentInParent<ObstacleRotator>();
        }

        private void OnCollisionStay(Collision collision)
        {
            pc = collision.gameObject.GetComponent<PlayerController>();
            float rag = pc == null ? 0.5f : 1f;

            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collision.transform.RotateAround(transform.position, Vector3.up,
                    obstacleRotator.rotationVelocity * rag);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            pc = null;
        }
    }
}






