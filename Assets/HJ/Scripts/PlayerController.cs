using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using Unity.VisualScripting;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {

        //========= Player Move ===========
        [Header("Player Move")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveDash;
        [SerializeField] private float jumpPower;
        [SerializeField] private float normalSpeed;
        [SerializeField] private Vector3 velocity;
        //=================================


        //=========== Player Controller ===========
        private Rigidbody rig;
        //=========================================

        //======= Velocity And GroundCheck ========
        [SerializeField] private Transform groundChecker;
        [SerializeField] private bool isGrounded;
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundMask;
        //=========================================


        [SerializeField] private float maxVelocity;
        [SerializeField] private float magnitude;

        private void Awake()
        {
            normalSpeed = 10;
            moveSpeed = normalSpeed;
            moveDash = 1.5f;
            jumpPower = 10f;
        }

        private void Start()
        {
            rig = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            Move();
            Jump();
            Dash();
            IsGrounded();
            Checkvelocity();
        }

        private void Checkvelocity()
        {
            velocity = rig.velocity;
        }

        private void Move()
        {
            Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

            Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
            if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

            Vector3 moveVec = forwardVec * moveInput.z + rightVec * moveInput.x;

            rig.AddForce(moveVec * moveSpeed, ForceMode.Force);

            // 최대 속도 보정
            Vector3 horizonVelocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
            if (horizonVelocity.magnitude > maxVelocity)
            {
                horizonVelocity = horizonVelocity.normalized * maxVelocity;
                rig.velocity = new Vector3(horizonVelocity.x, rig.velocity.y, horizonVelocity.z);
            }
            magnitude = horizonVelocity.magnitude;

        }

        private void Dash()
        {
            moveSpeed = Input.GetKey(KeyCode.LeftShift) ? normalSpeed * moveDash : normalSpeed;
        }

        private void Jump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }

        private void IsGrounded()
        {
            isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        }
    }

}

