using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrueFalsePlatform : MonoBehaviour
{
    public List<Transform> platList;

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            platList.Add(transform.GetChild(i));
        }

        for(int i = 0; i < platList.Count * 0.5f; i++)
        {
            int rand = Random.Range(0, 2);
            platList[i * 2 + rand].GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < platList.Count * 0.5f; i++)
        {
            Gizmos.DrawLine(platList[i * 2].position, platList[i * 2 + 1].position);
        }
        
    }
}
