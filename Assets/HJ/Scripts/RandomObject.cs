using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour,IPunObservable
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
        Debug.Log("딜레이 이닛 접근");
        switch (type)
        {
            case Type.Step:
                pv.RPC("SteppingObj", RpcTarget.All);
                break;
            case Type.Door:
                SetKinematic();
                pv.RPC("DoorObj", RpcTarget.All);
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

    [PunRPC]
    private void SteppingObj()
    {
        Debug.Log("SteppingObj 접근");
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < objectList.Count * 0.5f; i++)
            {
                int rand = Random.Range(0, 2);
                objectList[i * 2 + rand].GetComponent<Collider>().isTrigger = true;
                Debug.Log("trigger : " + objectList[i * 2 + rand]);
            }
        }

    }


    [PunRPC]
    private void DoorObj()
    {
        Debug.Log("DoorObj 접근");
        if (PhotonNetwork.IsMasterClient)
        {
            int rand = Random.Range(0, 4);
            Transform targetDoorSet = objectList[rand];
            targetDoorSet.GetComponentsInChildren<Transform>();
            targetDoorSet.GetComponent<Rigidbody>().isKinematic = false;
            foreach (Transform tf in targetDoorSet)
            {
                tf.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}