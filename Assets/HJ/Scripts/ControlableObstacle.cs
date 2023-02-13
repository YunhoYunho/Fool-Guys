using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlableObstacle : MonoBehaviour, IControllable
{
    protected Coroutine controlling;
    public virtual void Control(float duration, float coolTime)
    {
        if (controlling != null)
            return;

        controlling = StartCoroutine(ControlCoroutine(duration, coolTime));
    }

    protected virtual IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        yield return null;
    }
}
