using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomObject : MonoBehaviourPun ,IPunObservable
{
    public List<Transform> objectList;

    public enum Type { Step, Door }

    public Type type;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            objectList.Add(transform.GetChild(i));
        }
        StartCoroutine(DelayInit());
    }

    IEnumerator DelayInit()
    {
        yield return new WaitForSeconds(3f);
        switch (type)
        {
            case Type.Step:
                SteppingObj();
                break;
            case Type.Door:
                SetKinematic();
                DoorObj();
                break;
            default:
                break;
        }
    }


    private void SetKinematic()
    {
        for (int index = 0; index < objectList.Count; index++)
        {
            Transform[] trans = objectList[index].GetComponentsInChildren<Transform>();
            foreach (Transform tf in trans)
            {
                tf.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    private void SteppingObj()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < objectList.Count * 0.5f; i++)
            {
                int rand = Random.Range(0, 2);
                pv.RPC("TriggerOnFunc", RpcTarget.All, rand, i);
            }
        }
        
    }

    [PunRPC]
    private void TriggerOnFunc(int rand, int i)
    {
        objectList[i * 2 + rand].GetComponent<Collider>().isTrigger = true;
        objectList[i * 2 + rand].gameObject.layer = 0;
        //Debug.Log("trigger : " + objectList[i * 2 + rand]);
    }


    private void DoorObj()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            int rand = Random.Range(0, 4);
            pv.RPC("KinematicOffFunc", RpcTarget.All, rand);

        }

        
    }

    [PunRPC]
    private void KinematicOffFunc(int rand)
    {
        Transform targetDoorSet = objectList[rand];
        targetDoorSet.GetComponentsInChildren<Transform>();
        targetDoorSet.GetComponent<Rigidbody>().isKinematic = false;

        foreach (Transform tf in targetDoorSet)
        {
            tf.GetComponent<Rigidbody>().isKinematic = false;
        }

        Obstacle[] obs = objectList[rand].gameObject.GetComponentsInChildren<Obstacle>();
        
        foreach (Obstacle ob in obs)
        {
            Destroy(ob);
        }


    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}