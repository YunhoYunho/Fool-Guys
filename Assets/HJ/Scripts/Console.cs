using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HJ
{
    public class Console : MonoBehaviour, IInteractable
    {
        [SerializeField] private float duration;
        [SerializeField] private float coolTime;

        public UnityEvent<float, float> OnActivate;

        public void InterAction(PlayerController player)
        {
            Activate();
        }

        public void Activate()
        {
            Debug.Log("µø¿€");
            OnActivate?.Invoke(duration, coolTime);
        }
    }
}

