using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotationObstacle : MonoBehaviour
{
    public Vector3 rotationSpeed;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigid.AddTorque(rotationSpeed);
        //transform.Rotate(rotationSpeed);
    }
}
