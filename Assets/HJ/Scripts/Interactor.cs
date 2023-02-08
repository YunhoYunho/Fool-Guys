using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    [RequireComponent(typeof(PlayerController))]
    public class Interactor : MonoBehaviour
    {

        [Header("Setting")]
        [SerializeField]
        private Transform viewPoint;
        [SerializeField]
        private float checkDistance = 4f;
        [SerializeField]
        private LayerMask interactableLayer;

        //======================================

        private PlayerController player;

        //======================================

        private void Awake()
        {
            player = GetComponent<PlayerController>();
        }

        private void Update()
        {
            CheckRangeByRay();
        }
        private void CheckRangeByRay()
        {
            RaycastHit hit;
            if (Physics.Raycast(viewPoint.position, viewPoint.forward, out hit, checkDistance, interactableLayer))
            {
                Interact(hit.transform);

                // TODO:
                // 인터렉션 가능함을 UI 등을 통해 알리기
            }

            Debug.DrawRay(viewPoint.position, viewPoint.forward * checkDistance, Color.red);
        }

        private void Interact(Transform obj)
        {
            // TODO:
            // 인풋 매니저를 통해 GetButtonDown("Interaction")으로 바꾸기
            if (!Input.GetKeyDown(KeyCode.E))
                return;

            IInteractable interactable = obj.GetComponentInParent<IInteractable>();
            interactable?.Interaction(player);
        }
    }

}
