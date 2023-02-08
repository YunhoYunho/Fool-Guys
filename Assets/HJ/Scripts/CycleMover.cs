using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleMover : MonoBehaviour
{
    [SerializeField]
    [Range(-1f, 1f)]
    private float sin;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float speed = 1;

    private float offset;
    private float time;

    private void Start()
    {
        offset = distance / 180f;
    }
    void Update()
    {
        time += Time.deltaTime * speed;
        float x = Mathf.PingPong(time, 360f);
        sin = Mathf.Sin(x);
        transform.Translate(Vector3.left * sin * offset);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * distance);
    }
}
