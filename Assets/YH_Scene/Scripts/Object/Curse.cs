using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : MonoBehaviour, IItem
{
    [SerializeField] private float speedDown = 10.0f;

    public void Use(GameObject item)
    {
        PlayerController controller = GetComponent<PlayerController>();

        controller.moveSpeed -= speedDown;

        //StartCoroutine(SpeedDown);

        Destroy(item);
    }

    private IEnumerator SpeedDown()
    {
        yield return new WaitForSeconds(speedDown);
    }
}
