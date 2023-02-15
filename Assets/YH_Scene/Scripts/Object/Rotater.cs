using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed;

    void FixedUpdate()
    {
        transform.Rotate(rotationSpeed);
    }
}
