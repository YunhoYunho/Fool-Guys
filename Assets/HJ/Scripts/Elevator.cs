using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class Elevator : MonoBehaviour
    {
        public enum State { IdleUp, IdleDown , Up, Down }

        [SerializeField]
        private State state;

        [Space]
        [SerializeField]
        private Transform platform;
        [SerializeField]
        private Transform startPoint;
        [SerializeField]
        private Transform endPoint;

        [Space]
        [SerializeField]
        private float speed;

        private Vector3 direction;


        private void Start()
        {
            direction = (endPoint.position - startPoint.position).normalized;
        }

        private void FixedUpdate()
        {
            switch (state)
            {
                case State.IdleUp:
                    break;

                case State.IdleDown:
                    break;

                case State.Up:
                    platform.Translate(direction * speed * Time.fixedDeltaTime);

                    if (platform.position.y >= endPoint.position.y)
                        state = State.IdleUp;
                    break;

                case State.Down:
                    platform.Translate(-direction * speed * Time.fixedDeltaTime);

                    if (platform.position.y <= startPoint.position.y)
                        state = State.IdleDown;
                    break;

                default:
                    break;
            }
        }

        public void OperateUp()
        {
            state = State.Up;
        }

        public void OperateDown()
        {
            state = State.Down;
        }

        public void Toggle()
        {
            if (state == State.IdleUp || state == State.Up)
                OperateDown();
            else
                OperateUp();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
    }
}

