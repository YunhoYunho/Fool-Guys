using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CycleRotator : MonoBehaviour
{
    [SerializeField] private float limitAngle;
    [SerializeField] private float speed;
    [SerializeField] private float rand;

    [Space]
    [SerializeField] private Vector3 rotationAxis = new Vector3(0, 0, 1);
    
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
        //lerpAngle = Mathf.Lerp(-1 * limitAngle, limitAngle, GetSinCycle());
        sinAngle = limitAngle * (Mathf.Sin((lerpTime + rand) * speed));

        transform.localRotation = Quaternion.Euler(startEuler + rotationAxis * sinAngle);
    }

}
