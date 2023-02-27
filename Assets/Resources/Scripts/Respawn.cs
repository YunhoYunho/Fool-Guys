using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();

            if (pv != null) 
            {
                if (other.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("리스폰 지점 닿음");
                    gameManager.SetCheckPoint(gameObject);
                }
            }
            
        }
        
    }

}
