using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : MonoBehaviour
{
    [SerializeField]
    private float rate;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rigid = other.attachedRigidbody;

        if (rigid != null)
        {
            // 속도 느려지게 하기
            rigid.velocity *= rate;
            //Debug.Log("속도크기: " + rigid.velocity.sqrMagnitude);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        Rigidbody rigid = other.attachedRigidbody;

        if (rigid != null)
        {
            rigid.velocity /= rate;
        }
    }*/
}
