using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour, IControllable
{
    [SerializeField] private Vector3 rotationVelocity;
    [SerializeField] private Vector3 controlledVelocity;
    private Vector3 curVelocity;
    private Coroutine controlling;

    [SerializeField]
    private float forceOffset = 1f;

    private void Start()
    {
        curVelocity = rotationVelocity;
        controlling = null;
    }

    private void FixedUpdate()
    {
        transform.Rotate(curVelocity);
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        rotationVelocity = newVelocity;
    }

    public void Control(float duration, float coolTime)
    {
        if (controlling != null)
            return;

        controlling = StartCoroutine(ControlCoroutine(duration, coolTime));      
    }

    private IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        curVelocity = controlledVelocity;

        yield return new WaitForSeconds(duration);
        curVelocity = rotationVelocity;

        yield return new WaitForSeconds(coolTime);
        controlling = null;
    }
}
