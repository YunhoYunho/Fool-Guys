using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class Move : MonoBehaviour, HJ_NoneUse.IInteractable
    {
        public void Interaction(PlayerController player)
        {
            Debug.Log(string.Format("{0} 와 상호작용 합니다.", player.name));
        }

        public void MoveLeft()
        {
            Debug.Log("Move Left 발동");
            float posX = Mathf.Lerp(transform.position.x, transform.position.x + 10, 1f);
            transform.Translate(new Vector3(posX, transform.position.y, transform.position.z));
        }

    }
}

