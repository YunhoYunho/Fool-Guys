using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleObject : MonoBehaviourPun, IControllable
{
    protected Coroutine controlling;
    public virtual void Control(float duration, float coolTime)
    {
        if (controlling != null)
            return;

        controlling = StartCoroutine(ControlCoroutine(duration, coolTime));
    }

    protected abstract IEnumerator ControlCoroutine(float duration, float coolTime);
}
