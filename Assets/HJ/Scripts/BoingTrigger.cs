using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoingTrigger : MonoBehaviour
{
    public enum Type { Random, Trigger }
    public Type type;

    //public UnityEvent OnCollision;

    private Animator anim;

    private Coroutine randomCoroutine;
    [SerializeField] private float checkTime = 2f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        if (type != Type.Random)
            return;

        randomCoroutine = StartCoroutine(RandomActivate());
    }

    private IEnumerator RandomActivate()
    {
        while(true)
        {
            checkTime = Random.Range(0.5f, 3f);
            yield return new WaitForSeconds(checkTime);

            int rand = Random.Range(0, 2);
            //Debug.Log("·£´ý°ª: " + rand);
            if (rand == 0)
            {
                anim.SetTrigger("Activate");
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (type != Type.Trigger)
            return;

        if(collision.transform.CompareTag("Player"))
        {
            anim.SetTrigger("Active");
        }    
    }
}
