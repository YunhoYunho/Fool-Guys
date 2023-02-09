using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolObject : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float speed = 1.0f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 pos = startPosition;
        pos.x = distance * Mathf.Sin(Time.time * speed);
        transform.position = pos;
    }
}
