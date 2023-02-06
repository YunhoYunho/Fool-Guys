using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationVelocity;

    [SerializeField]
    private float forceOffset = 1f;

    private void FixedUpdate()
    {
        transform.Rotate(rotationVelocity);
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        rotationVelocity = newVelocity;
    }

    /*private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 cross = -Vector3.Cross(rotationVelocity.normalized, normal);
            collision.transform.GetComponent<Rigidbody>().velocity += (cross * forceOffset);
            Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + cross, Color.green);
        }
    }*/



}
