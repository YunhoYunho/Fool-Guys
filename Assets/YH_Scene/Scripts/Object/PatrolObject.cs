using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolObject : ControlableObstacle
{
    [Header("Patrol")]
    [SerializeField] private float distance = 7.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float controlSpeed = 2.5f;
    [SerializeField] private bool isDirX = true;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        Patrolling();
    }

    private void Patrolling()
    {
        Vector3 dir = isDirX ? Vector3.right : Vector3.forward;
        Vector3 pos = distance * Mathf.Sin(Time.time * speed) * dir;

        transform.localPosition = pos + startPos;
    }

    protected override IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        float prevSpeed = speed;
        speed = controlSpeed;

        yield return new WaitForSeconds(duration);
        speed = prevSpeed;

        yield return new WaitForSeconds(coolTime);
        controlling = null;
    }
}
