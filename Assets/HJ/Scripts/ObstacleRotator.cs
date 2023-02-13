using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : ControlableObstacle
{
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

        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].transform.Rotate(Vector3.up * rotationVelocity);
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