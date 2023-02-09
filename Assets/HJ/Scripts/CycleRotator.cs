using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CycleRotator : MonoBehaviour, IControllable
{
    [SerializeField] private float limitAngle = 30f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rand;

    [Space]
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 0, 1);

    [Header("Control")]
    [SerializeField] private float controlledSpeed = 30f;

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
        sinAngle = Mathf.PingPong((lerpTime + rand) * speed, limitAngle * 2) - limitAngle;

        transform.localRotation = Quaternion.Euler(startEuler + rotationAxis * sinAngle);
        //transform.localEulerAngles +=  rotationAxis * sinAngle * Time.deltaTime;
        //transform.localEulerAngles = startEuler + rotationAxis * (Mathf.PingPong(speed * lerpTime, limitAngle * 2) - limitAngle);
        //transform.localEulerAngles += rotationAxis * lerpTime * speed;
        //transform.localEulerAngles = startEuler + rotationAxis * Mathf.PingPong(lerpTime * speed, limitAngle * 2);
    }

    public void Control(float duration, float coolTime)
    {
        StartCoroutine(ControlCoroutine(duration, coolTime));
    }

    private IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        float originalSpeed = speed;
        speed = controlledSpeed;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;

    }

}
