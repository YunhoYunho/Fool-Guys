using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Console : MonoBehaviour, IInteractable
{
    [SerializeField] private float duration;
    [SerializeField] private float coolTime;

    public UnityEvent<float, float> OnActivate;
    public void Activate()
    {
        //Debug.Log("¿€µø");
        OnActivate?.Invoke(duration, coolTime);
    }

    public void InterAction(PlayerController player)
    {
        Debug.Log(player);
        Activate();
    }
}
