using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CycleRotator : MonoBehaviour, IControllable
{
    [SerializeField] private float limitAngle;
    [SerializeField] private float speed;
    [SerializeField] private float rand;

    [Space]
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 0, 1);

    [Header("Control")]
    [SerializeField] private float controlledAngle;
    [SerializeField] private float controlledSpeed;
    
    private Vector3 startEuler;
    private float sinAngle;
    private float lerpTime = 0;

    private void Start()
    {
        startEuler = transform.rotation.eulerAngles;
    }
    private void Update()
    {
        lerpTime += Time.deltaTime;
        sinAngle = Mathf.Sin((lerpTime + rand) * speed);

        //transform.localRotation = Quaternion.Euler(startEuler + rotationAxis * (limitAngle * sinAngle));
        //transform.localEulerAngles = startEuler + rotationAxis * limitAngle * sinAngle;
        transform.localEulerAngles = startEuler + rotationAxis * Mathf.PingPong(lerpTime * speed, limitAngle * 2);
    }

    public void Control(float duration, float coolTime)
    {
        StartCoroutine(ControlCoroutine(duration, coolTime));
    }

    private IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        float originalAngle = limitAngle;
        float originalSpeed = speed;

        limitAngle = controlledAngle;
        speed = controlledSpeed;
        yield return new WaitForSeconds(duration);
        limitAngle = originalAngle;
        speed = originalSpeed;
    }

}
