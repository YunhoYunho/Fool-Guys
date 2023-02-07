using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolObject : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private bool isHorizontal = true;
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float speed = 10.0f;

    private bool isForward = true;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        if (isHorizontal)
            transform.position += Vector3.right;
        else
            transform.position += Vector3.forward;
    }

    private void Update()
    {
        if (isHorizontal)
        {
            if (isForward)
            {
                if (transform.position.x < startPosition.x + distance)
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }

                else
                    isForward = false;
            }

            else
            {
                if (transform.position.x > startPosition.x)
                {
                    transform.position -= Vector3.right * speed * Time.deltaTime;
                }

                else
                    isForward = true;
            }
        }

        else
        {
            if (isForward)
            {
                if (transform.position.z < startPosition.z + distance)
                {
                    transform.position += Vector3.forward * speed * Time.deltaTime;
                }

                else
                    isForward = false;
            }

            else
            {
                if (transform.position.z > startPosition.z)
                {
                    transform.position -= Vector3.forward * speed * Time.deltaTime;
                }

                else
                    isForward = true;
            }
        }
    }
}
