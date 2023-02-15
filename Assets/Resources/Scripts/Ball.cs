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
        //if (PhotonNetwork.IsMasterClient)
        //{
        //int styleCount = transform.childCount;
        //int rand = Random.Range(0, styleCount);

        //pv.RPC("InitStyle", RpcTarget.All, styleCount, rand);



        //{
        //    pv.RPC("SetColor", RpcTarget.All);
        //}
        //}
        if (gameObject.GetComponent<ColorSelector>() != null)
            gameObject.GetComponent<ColorSelector>().SetRandomStyle();

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.localScale = Vector3.zero;
    }

    //[PunRPC]
    //public void InitStyle(int styleCount, int rand)
    //{
    //    transform.GetChild(rand).gameObject.SetActive(true);

    //    for (int i = 0; i < styleCount; i++)
    //    {
    //        transform.GetChild(i).gameObject.SetActive(false);
    //    }
    //}


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
