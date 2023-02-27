using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviourPun
{
    [SerializeField]
    private float speed;

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(4f);
        PhotonNetwork.Destroy(gameObject);
    }
}
