using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviourPun, IPunObservable
{
    private PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    [PunRPC]
    public void Initiate()
    {
        if (gameObject.GetComponent<ColorSelector>() != null)
            gameObject.GetComponent<ColorSelector>().SetRandomStyle();

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.localScale = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("RespawnPlane"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
