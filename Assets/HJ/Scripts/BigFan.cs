using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BigFan : ConsoleObject
{
    private ParticleSystem particle;
    private FlowZone flowZone;
    private Rotater rotater;

    [Space]
    [SerializeField] private float controlRate;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        flowZone = GetComponentInChildren<FlowZone>();
        rotater = GetComponentInChildren<Rotater>();
    }

    protected override IEnumerator ControlCoroutine(float duration, float coolTime)
    {

        Vector3 prevRotation = rotater.RotationSpeed;
        rotater.RotationSpeed *= controlRate;

        if(controlRate == 0)
            particle.gameObject.SetActive(false);

        float prevPower = flowZone.Power;
        flowZone.Power *= controlRate;


        yield return new WaitForSeconds(duration);
        rotater.RotationSpeed = prevRotation;
        flowZone.Power = prevPower;
        particle.gameObject.SetActive(true);

        yield return new WaitForSeconds(coolTime);
        controlling = null;
    }
}
