using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionAdapter : MonoBehaviour, IInteractable
{
    public UnityEvent<PlayerController> OnInterAction;

    public void InterAction(PlayerController player)
    {
        OnInterAction?.Invoke(player);
    }
}
