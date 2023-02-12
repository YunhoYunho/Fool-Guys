using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolObject : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float distance = 7.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private bool isDirX = true;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        Patrolling();
    }

    private void Patrolling()
    {
        Vector3 dir = isDirX ? Vector3.right : Vector3.forward;
        Vector3 pos = distance * Mathf.Sin(Time.time * speed) * dir;

        transform.localPosition = pos + startPos;
    }
}
