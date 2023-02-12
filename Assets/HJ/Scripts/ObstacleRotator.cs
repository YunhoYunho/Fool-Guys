using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : ControlableObstacle
{
    [SerializeField] private Vector3 rotationVelocity;
    [SerializeField] private float changed;

    [SerializeField] private bool isOpposite;
    [SerializeField] private bool startRandom;
    [SerializeField] private List<Transform> evenList = new List<Transform>();
    [SerializeField] private List<Transform> oddList = new List<Transform>();

    private void Start()
    {
        controlling = null;

        if(startRandom)
        {
            int randAngle = Random.Range(0, 360);
            transform.localEulerAngles = rotationVelocity.normalized * randAngle;
            Debug.Log("·£´ý°ª: " + randAngle);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < evenList.Count; i++)
        {
            evenList[i].transform.Rotate(-rotationVelocity);
        }

        for (int j = 0; j < oddList.Count; j++)
        {
            oddList[j].transform.Rotate(rotationVelocity);
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
