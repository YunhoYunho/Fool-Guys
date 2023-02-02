using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GamePlayerController : MonoBehaviourPun, IPunObservable
{
    int count;

    [SerializeField]
    private Bullet bullet;

    private void Start()
    {
        if (!photonView.IsMine) 
            Destroy(this);
    }

    private void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
            gameObject.transform.Translate(new Vector3(0f, 0f, 20f) * Time.deltaTime);
            //rigid.AddForce(new Vector3(0f, 0f, 20f) * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            gameObject.transform.Translate(new Vector3(0f, 0f, -20f) * Time.deltaTime);
            //rigid.AddForce(new Vector3(0f, 0f, -20f) * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            gameObject.transform.Rotate(new Vector3(0f, -100f, 0f) * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            gameObject.transform.Rotate(new Vector3(0f, 100f, 0f) * Time.deltaTime);

        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");


    }

    private void Shoot()
    {
        if (!Input.GetButtonDown("Jump"))
            return;

        photonView.RPC("CreateBullet", RpcTarget.All, transform.position, transform.rotation);
    }

    [PunRPC]
    public void CreateBullet(Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("Bullet", position, rotation);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 순서 조심 ( 받는 것도 보낸 순서대로 받아야함 )
        if (stream.IsWriting)
        {
            stream.SendNext(count);
        }

        if (stream.IsReading)
        {
            count = (int)stream.ReceiveNext();
        }
    }

    
}
