using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingPlatform : ControlableObstacle
{
    protected override IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(coolTime);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        controlling = null;
    }
}
