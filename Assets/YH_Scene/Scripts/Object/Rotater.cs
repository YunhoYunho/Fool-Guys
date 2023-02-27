using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : ControlableObstacle
{
    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private Vector3 controlSpeed;

    public Vector3 RotationSpeed { get { return rotationSpeed; } set { rotationSpeed = value; } }

    void FixedUpdate()
    {
        transform.Rotate(rotationSpeed);
    }

    protected override IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        Vector3 prevSpeed = rotationSpeed;
        rotationSpeed = controlSpeed;

        yield return new WaitForSeconds(duration);
        rotationSpeed = prevSpeed;

        yield return new WaitForSeconds(coolTime);
        controlling = null;
    }
}
