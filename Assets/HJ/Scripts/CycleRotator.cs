using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CycleRotator : ConsoleObject
{
    [SerializeField] private float limitAngle = 45f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float changeRate = 1.5f;
    [SerializeField] private float rand;

    [Space]
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 0, 1); 

    private Vector3 startEuler;
    private float sinAngle;
    private float lerpTime = 0;

    private void Start()
    {
        startEuler = transform.rotation.eulerAngles;
        rotationAxis.Normalize();
    }
    private void Update()
    {
        lerpTime += Time.deltaTime;
        sinAngle = limitAngle * Mathf.Sin((lerpTime + rand) * speed);

        transform.localRotation = Quaternion.Euler(startEuler + rotationAxis * sinAngle);
    }

    protected override IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        speed *= changeRate;

        yield return new WaitForSeconds(duration);
        speed /= changeRate;

        yield return new WaitForSeconds(coolTime);
    }

}
