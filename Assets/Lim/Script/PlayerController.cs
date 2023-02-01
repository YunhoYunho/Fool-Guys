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

    private void Awake()
    {
        normalSpeed = 20;
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
        Checkvelocity();
    }

    private void Checkvelocity()
    {
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity.Set(maxSpeed, rigid.velocity.y, rigid.velocity.z);

        if (rigid.velocity.z > maxSpeed)
            rigid.velocity.Set(rigid.velocity.x, rigid.velocity.y, maxSpeed);

        velocity = rigid.velocity;

    }


    void limitMoveSpeed()
    {
        if (rigid.velocity.x > maxVelocityX)
        {
            rigid.velocity = new Vector3(maxVelocityX, rigid.velocity.y, rigid.velocity.z);
        }
        if (rigid.velocity.x < (maxVelocityX * -1))
        {
            rigid.velocity = new Vector3((maxVelocityX * -1), rigid.velocity.y, rigid.velocity.z);
        }

        if (rigid.velocity.z > maxVelocityZ)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, maxVelocityZ);
        }
        if (rigid.velocity.z < (maxVelocityZ * -1))
        {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, (maxVelocityZ * -1));
        }
    }

    private void Move()
    {
        //float x = Input.GetAxisRaw("Horizontal");
        //float z = Input.GetAxisRaw("Vertical");
        //Vector3 moveVec = new Vector3(x, 0, z) * moveSpeed * Time.deltaTime;
        //rigid.AddForce(moveVec, ForceMode.Impulse);

        Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
        Vector3 moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;
        rigid.AddForce(moveVec * moveSpeed);
        limitMoveSpeed();
        if (moveVec.sqrMagnitude != 0)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.5f);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoving", true);
        }

        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isMoving", false);
        }

    }

    private void Dash()
    {
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? normalSpeed * moveDash : normalSpeed;
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
