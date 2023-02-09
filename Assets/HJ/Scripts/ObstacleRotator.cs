using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour, IControllable
{
    [SerializeField] private Vector3 rotationVelocity;
    [SerializeField] private float changed;
    private Coroutine controlling;

    private void Start()
    {
        controlling = null;
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotationVelocity);
    }

    public void Control(float duration, float coolTime)
    {
        if (controlling != null)
            return;

        controlling = StartCoroutine(ControlCoroutine(duration, coolTime));      
    }

    private IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        rotationVelocity *= changed;

        yield return new WaitForSeconds(duration);
        rotationVelocity /= changed;

        yield return new WaitForSeconds(coolTime);
        controlling = null;
    }
}
