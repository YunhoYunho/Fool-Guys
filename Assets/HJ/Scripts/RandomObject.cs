using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomObject : MonoBehaviour
{
    public List<Transform> objectList;
    
    public enum Type { Step, Door }

    public Type type;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            objectList.Add(transform.GetChild(i));
        }

        switch (type)
        {
            case Type.Step:
                SteppingObj();
                break;
            case Type.Door:
                DoorObj();
                break;
            default:
                break;
        }
    }

    private void SteppingObj()
    {
        for (int i = 0; i < objectList.Count * 0.5f; i++)
        {
            int rand = Random.Range(0, 2);
            objectList[i * 2 + rand].GetComponent<Collider>().isTrigger = true;
            Debug.Log("trigger : " + objectList[i * 2 + rand]);
        }
    }

    private void DoorObj()
    {
        for (int i = 0; i < objectList.Count * 0.5f; i++)
        {
            int rand = Random.Range(0, 2);
            Transform targetDoorSet = objectList[i * 2 + rand];

            targetDoorSet.GetComponent<Rigidbody>().isKinematic = true;
/*            Transform[] childDoor = targetDoorway.GetComponentsInChildren<Transform>();


            for (int j = 0; j < childDoor.Length; j++)
            {
                childDoor[j].gameObject.isStatic = true;
                Debug.Log("static Door : " + childDoor[j]);
            }
*/
            Debug.Log("static : " + objectList[i * 2 + rand]);
        }
    }
}
