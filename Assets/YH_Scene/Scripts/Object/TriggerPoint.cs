using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPoint : MonoBehaviour
{
    public UnityEvent OnTouch; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTouch?.Invoke();
        }
    }
}
