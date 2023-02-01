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
    private CharacterController controller;
    //=========================================

    //======= Velocity And GroundCheck ========
    [SerializeField] private Transform groundChecker;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    //=========================================

    private void Awake()
    {
        normalSpeed = 100;
        moveSpeed = 100;
        moveDash = 1f;
        jumpPower = 7f;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Move();
        Jump();
        Dash();
        IsGrounded();
    }
    private void Move()
    {
        Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        Vector3 moveVec = forwardVec * moveInput.z + rightVec * moveInput.x;
        controller.Move(moveVec * moveSpeed * Time.deltaTime);

        if (moveVec.sqrMagnitude != 0)
            transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.8f);
    }

    private void Dash()
    {
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? normalSpeed * moveDash : normalSpeed;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpPower;
        }
        controller.Move(Vector3.up * velocity.y * Time.deltaTime);
    }

    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        velocity.y += Physics.gravity.y * Time.deltaTime;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
}
