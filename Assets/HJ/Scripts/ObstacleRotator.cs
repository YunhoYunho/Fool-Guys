using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : ControlableObstacle
{
    [SerializeField] private Vector3 rotationVelocity;
    [SerializeField] private float changed;

    [SerializeField] private bool isOpposite;
    [SerializeField] private bool startRandom;

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
        int rotationDir = isOpposite ? -1 : 1;
        transform.Rotate(rotationDir * rotationVelocity);
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
