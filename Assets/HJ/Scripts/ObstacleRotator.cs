using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    public Vector3 rotationVelocity;
    public Vector3 controlledVelocity;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rigid.angularVelocity = rotationVelocity;
    }

    public void SetVelocity(float power)
    {
        rotationVelocity = controlledVelocity;
    }


}
