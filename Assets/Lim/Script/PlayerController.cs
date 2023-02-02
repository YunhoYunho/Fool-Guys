using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //========= Player Move ===========
    [Header("Player Move")]
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float moveDash;
    [SerializeField] private float jumpPower;
    [SerializeField] private float normalSpeed;
    [SerializeField] private Vector3 velocity;
    [SerializeField] Vector3 MoveVec;

    private float maxVelocityX = 2, maxVelocityZ = 2;
    //=================================


    //=========== Player Controller ===========
    private Rigidbody rigid;
    private Animator anim;
    //=========================================

    //======= Velocity And GroundCheck ========
    [SerializeField] private Transform groundChecker;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    //=========================================

    //============ Bool Checker ===============
    [SerializeField] private bool isMoving;
    //=========================================

    private void Awake()
    {
        normalSpeed = 300;
        moveSpeed = normalSpeed;
        
        moveDash = 1.5f;
        jumpPower = 10f;
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Jump();
        Dash();
        IsGrounded();
    }

    private void FixedUpdate()
    {
        FixedMove();
    }

    private void Move()
    {

        Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
        Vector3 moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;

        MoveVec = moveVec;

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }

        // GetAxisRaw 의 입력값이 있는지의 여부를 bool로 판단하여 저장 
        bool vermove = Input.GetAxisRaw("Vertical") != 0 ? true : false;
        bool hormove = Input.GetAxisRaw("Horizontal") != 0 ? true : false;

        isMoving = vermove || hormove ? true : false;
        // ver, hor 둘 중 하나라도 true일 경우 true 저장

        //InputMoveKey();
        // ㄴ 기존 방식의 함수화

        anim.SetBool("isMoving", isMoving);

    }

    private void FixedMove()
    {
        rigid.AddForce(MoveVec * moveSpeed);
        if (MoveVec.sqrMagnitude != 0)
        {
            transform.forward = Vector3.Lerp(transform.forward, MoveVec, Time.fixedDeltaTime * 10);
        }

    }

    private void Dash()
    {
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? normalSpeed * moveDash : normalSpeed;
    }

    private void InputMoveKey()
    {
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ?
                   true : false;

        // 키 WASD 4개중 하나라도 입력이 있다면 true 아니라면 false
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }
}





