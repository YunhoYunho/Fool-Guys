using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowZone : MonoBehaviour
{
    [SerializeField]
    private float power;

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * power, ForceMode.Force);
        }
    }
}
