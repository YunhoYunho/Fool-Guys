using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class PlayerControllercc : MonoBehaviour
{
    //============= Player Move ===============
    [Header("Player Move")]
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float jumpPower;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 moveVec;
    [SerializeField] private float maxVelocity = 2;
    //=========================================
    [Space]

    //============ Player Model ===============
    [SerializeField] private GameObject normalModel;
    //=========================================

    //================ Attack =================
    [SerializeField] private GameObject attackCollider;
    private Coroutine attackcoroutine;
    private Coroutine getUpCoroutine;
    //=========================================

    //=========== Player Controller ===========
    private Rigidbody rigid;
    private Rigidbody[] ragdollRigid;
    private Collider col;
    private Collider[] cols;
    [SerializeField] private Animator anim;
    //=========================================

    [Header("Velocity And GroundCheck")]
    //======= Velocity And GroundCheck ========
    [SerializeField] private Transform groundChecker;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    //=========================================
    [Space]

    //============ Bool Checker ===============
    [SerializeField] private bool isMoving;
    [SerializeField] private bool jumpOrder;
    [SerializeField] private bool attackOrder;
    [SerializeField] private bool getUp;
    //=========================================

    //=========== Animator String =============
    private List<string> animlist;
    private string moveanim = "isMoving";
    //=========================================

    private Transform hipBone;

    private BoneTransform[] standUpBoneTransforms;
    private BoneTransform[] ragdollBoneTransforms;
    private Transform[] bones;

    private enum State
    {
        Normal,
        Ragdoll
    }

    private State state = State.Normal;

    private class BoneTransform
    {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }
    }

    private void Awake()
    {
        moveSpeed = 300;
        jumpPower = 10f;
        attackcoroutine = null;
        getUpCoroutine = null;
        animlist = new List<string>();
        getUp = true;
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        ragdollRigid = GetComponentsInChildren<Rigidbody>();
        col = GetComponent<Collider>();
        cols = GetComponentsInChildren<Collider>();
        hipBone = anim.GetBoneTransform(HumanBodyBones.Hips);

        bones = hipBone.GetComponentsInChildren<Transform>();

        SetAnimList();

        Cursor.lockState = CursorLockMode.Locked;

       for (int i = 0; i < cols.Length; i++)
       {
           cols[i].enabled = false;
       }
        col.enabled = true;
    }

    private void SetAnimList()
    {
        animlist.Add(moveanim);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal: break;
            case State.Ragdoll: break;
        }

        Move();
        Jump();
        Attack();
        IsGrounded();
        HitTest();
        AnimationUpdate();
    }

    private void FixedUpdate()
    {
        FixedMove();
        FixedJump();
        FixedAttack();
    }

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = hipBone.position;
        transform.position = hipBone.position;

        hipBone.position = originalHipsPosition;
    }

    private void AnimationUpdate()
    {
        //string updateAnim;
        //if (isMoving)
        //    updateAnim = moveanim;



        //for (int i=0; i< animlist.Count; i++)
        //{
        //    bool playAnim = animlist[i] == updateAnim ? true : false;
        //    anim.SetBool(updateAnim, playAnim);
        //}

        // ㄴ 애니메이션이 더 늘어날 경우 사용할 내용
        //    현재는 bool 값이 하나라서 밑의 내용으로 충분하기 때문에
        //    사용하지 않고 있음

        anim.SetBool(moveanim, isMoving);
        
    }

    private void Move()
    {
        if (!getUp) return;

        if (!CurAnim("Idle") && !CurAnim("Run")) return;

        Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
        moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;

        if (moveVec.sqrMagnitude != 0) transform.forward = Vector3.Lerp(transform.forward, moveVec, Time.fixedDeltaTime * 10);

        // GetAxisRaw 의 입력값이 있는지의 여부를 bool로 판단하여 저장 
        bool vermove = Input.GetAxisRaw("Vertical") != 0 ? true : false;
        bool hormove = Input.GetAxisRaw("Horizontal") != 0 ? true : false;

        isMoving = vermove || hormove ? true : false;
        // ver, hor 둘 중 하나라도 true일 경우 true 저장
        MaxSpeed();
    }

    private void MaxSpeed()
    {
        if (rigid.velocity.x > maxVelocity)
        {
            rigid.velocity = new Vector3(maxVelocity, rigid.velocity.y, rigid.velocity.z);
        }

        if (rigid.velocity.x < (maxVelocity * -1))
        {
            rigid.velocity = new Vector3((maxVelocity * -1), rigid.velocity.y, rigid.velocity.z);
        }

        if (rigid.velocity.z > maxVelocity)
        {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, maxVelocity);
        }

        if (rigid.velocity.z < (maxVelocity * -1))
        {
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, (maxVelocity * -1));
        }
    }


    private void FixedMove()
    {
        rigid.AddForce(moveVec * moveSpeed);
    }

    private void FixedJump()
    {
        if (jumpOrder)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpOrder = false;
        }

    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") && isGrounded)
            attackOrder = true;
    }

    private void FixedAttack()
    {
        if (attackOrder && attackcoroutine == null)
        {
            attackCollider.GetComponent<BoxCollider>().enabled = true;
            attackcoroutine = StartCoroutine(OffAttackCollier());
        }
    }

    private IEnumerator OffAttackCollier()
    {
        yield return new WaitForSeconds(0.5f);
        attackCollider.GetComponent<BoxCollider>().enabled = false;
        attackOrder = false;
        attackcoroutine = null;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
            jumpOrder = true;
    }

    private void HitTest()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            EnterRagdoll();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ExitRagdoll();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            rigid.constraints = RigidbodyConstraints.None;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }


    private void ExitRagdoll()
    {
        anim.enabled = true;
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
        }
        col.enabled = true;
    }

    public void EnterRagdoll()
    {
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
        }

        col.enabled = true;
        anim.enabled = false;
    }

    private void Hit()
    {
        EnterRagdoll();
    }

    public void HITHIT()
    {
        anim.SetTrigger("Hurt");
        rigid.constraints = RigidbodyConstraints.None;
        StartCoroutine(StandUp());
    }


    private IEnumerator StandUp()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Recover");
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }

    private bool CurAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}