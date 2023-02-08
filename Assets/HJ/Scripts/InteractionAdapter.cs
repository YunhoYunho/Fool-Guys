using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HJ
{
    public class InteractionAdapter : MonoBehaviour, IInteractable
    {
        public UnityEvent<PlayerController> OnInteract;
        public void Interaction(PlayerController player)
        {
            //Debug.Log("인터렉션");
            OnInteract?.Invoke(player);
        }
    }
}

