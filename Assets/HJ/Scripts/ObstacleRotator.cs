using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : ConsoleObject
{
    public enum Type { Stick, Fan, Circle }
    public Type type;
    public float rotationVelocity;
    [SerializeField] private float changed;

    [SerializeField] private bool isOpposite;
    [SerializeField] private bool startRandom;
    [SerializeField] private List<Transform> objList = new List<Transform>();

    private void Start()
    {
        controlling = null;
    }

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        switch (type)
        {
            case Type.Stick:
                rotateStick();
                break;
            case Type.Fan:
                rotateFan();
                break;
            case Type.Circle:
                rotateCircle();
                break;
            default:
                break;
        }

        
    }

    private void rotateStick()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            var stick = objList[i].Find("Stick");
            var platform = objList[i].Find("Platform");

            if (stick)
            {
                stick.gameObject.transform.Rotate(Vector3.down * rotationVelocity);
            }

            if (platform)
            {
                platform.gameObject.transform.Rotate(Vector3.up * rotationVelocity);
            }
        }
    }

    private void rotateFan()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            var fan = objList[i].Find("Fan");

            if (fan)
            {
                fan.gameObject.transform.Rotate(Vector3.up * rotationVelocity);
            }
        }
    }

    private void rotateCircle()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            Transform circle = objList[i];

           circle.Rotate(Vector3.up * rotationVelocity);
        }
    }

    protected override IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        rotationVelocity *= changed;

        yield return new WaitForSeconds(duration);
        rotationVelocity /= changed;

        yield return new WaitForSeconds(coolTime);
        controlling = null;
    }
}
