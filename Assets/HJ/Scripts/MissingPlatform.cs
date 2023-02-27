using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingPlatform : ConsoleObject
{
    private Transform[] Platforms;

    private void Awake()
    {
        Platforms = GetComponentsInChildren<Transform>();
    }

    private void SetFalse()
    {
        for (int index = 1; index < Platforms.Length; index++)
        {
            Platforms[index].gameObject.SetActive(false);
        }
    }

    private void SetTrue()
    {
        for (int index = 1; index < Platforms.Length; index++)
        {
            Platforms[index].gameObject.SetActive(true);
        }
    }

    protected override IEnumerator ControlCoroutine(float duration, float coolTime)
    {
        SetFalse();
        yield return new WaitForSeconds(duration);
        SetTrue();
        yield return new WaitForSeconds(coolTime);
        controlling = null;
    }
}