using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float fallingDelay = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                StartCoroutine(Fall(fallingDelay));
            }
        }
    }

    private IEnumerator Fall(float time)
    {
        yield return new WaitForSeconds(time);
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
}
