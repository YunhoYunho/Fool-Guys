using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviourPun
{
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        if (photonView.InstantiationData != null)
        {
            rigid.AddForce((Vector3)photonView.InstantiationData[0]);
            rigid.AddTorque((Vector3)photonView.InstantiationData[1]);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect || Mathf.Abs(transform.position.z) > Camera.main.orthographicSize)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //if (!photonView.IsMine)
        //    return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            PhotonNetwork.Destroy(collision.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }



}
