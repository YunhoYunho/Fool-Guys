using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
public enum PlayerState { Idle, Attack, GetDown, GetUp, GettingUp, Jump }

public class PlayerController : MonoBehaviour
{
    //============= Player Move ===============
    [Header("Player Move")]
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float jumpPower;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 moveVec;
    [SerializeField] private float maxVelocity = 5;
    //=========================================
    [Space]

    //============ Player Model ===============
    [SerializeField] private GameObject normalModel;
    [SerializeField] private Transform hipBones;
    [SerializeField] private BoneTransform[] ragdollBoneTransform;
    [SerializeField] private BoneTransform[] animBoneTransform;
    [SerializeField] private BoneTransform[] currentBoneTransform;
    [SerializeField] private Transform[] bones;
    [SerializeField] private Collider[] playerColliders;
    [SerializeField] private CapsuleCollider playercol;
    //=========================================

    //================ Attack =================
    [SerializeField] private GameObject attackCollider;
    private Coroutine attackcoroutine;
    //=========================================

    //=========== Player Controller ===========
    private Rigidbody rigid;
    private Rigidbody[] rig;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerState state;
    //=========================================

    [Header("Velocity And GroundCheck")]
    //======= Velocity And GroundCheck ========
    [SerializeField] private Transform groundChecker;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundRadious;
    //=========================================
    [Space]

    //============ Bool Checker ===============
    [SerializeField] private bool isMoving;
    [SerializeField] private bool jumpOrder;
    [SerializeField] private bool attackOrder;
    [SerializeField] private bool getUp;
    [SerializeField] private bool playstandup;
    [SerializeField] private bool ragdoll;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isLanding;
    [SerializeField] private bool inMidAir;
    //=========================================

    //=============== Animator ================
    private List<string> animlist;
    private string moveAnim = "isMoving";
    private string attackAnim = "isAttack";
    private string getupAnim = "GetUp";
    private string getupClipName = "Getting Up";
    private string fallingAnim = "isFalling";
    private string jumpAnim = "isJumping";
    private string landingAnim = "isLanding";
    private string inMidAirAnim = "inMidAir";
    //=========================================

    //=============== Timer ===================
    private float getupTimer = 0;
    private float standupAnimTimer = 0;
    private float resettingBonesTimer = 0.3f;
    private float gettingUp = 0;
    private float fallTimer = 0;
    //=========================================

    private void Awake()
    {
        //=== PlayerState SetUp ===
        moveSpeed = 10f;
        jumpPower = 10f;
        state = PlayerState.Idle;
        groundRadious = 0.5f;
        //=========================

        //==== SetUp Coroutine ====
        attackcoroutine = null;
        //=========================

        //====== GetComponent =====
        hipBones = anim.GetBoneTransform(HumanBodyBones.Hips);
        rig = normalModel.GetComponentsInChildren<Rigidbody>();
        bones = hipBones.GetComponentsInChildren<Transform>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        playercol = GetComponent<CapsuleCollider>();
        playerColliders = normalModel.GetComponentsInChildren<Collider>();
        //=========================

        //=== Bone Transform Set ===
        animBoneTransform = new BoneTransform[bones.Length];
        ragdollBoneTransform = new BoneTransform[bones.Length];

        for (int bone = 0; bone < bones.Length; bone++)
        {
            animBoneTransform[bone] = new BoneTransform();
            ragdollBoneTransform[bone] = new BoneTransform();
        }

        StartAnimTransformCopy(getupClipName, animBoneTransform);
        //==========================

        //=== List And Bool Set ===
        animlist = new List<string>();
        getUp = true;
        ragdoll = false;
        isJumping = false;
        isFalling = false;
        //=========================
    }

    private void Start()
    {
        // === SetUp Before Play ===
        SetAnimList();
        SetJoint();
        SetRigidBodyGravity();
        Cursor.lockState = CursorLockMode.Locked;
        // =========================
    }

    private void SetAnimList()
    {
        animlist.Add(moveAnim);
        animlist.Add(attackAnim);
        animlist.Add(fallingAnim);
        animlist.Add(jumpAnim);
        animlist.Add(landingAnim);
        animlist.Add(inMidAirAnim);
    }

    private void Update()
    {
        switch (state)
        {
            case PlayerState.Idle:
            case PlayerState.Attack:
                Move();
                JumpOrder();
                AttackOrder();
                break;
            case PlayerState.GetDown:
                GetUpTimer();
                break;
            case PlayerState.GetUp:
                GettingUpAnimationCheck();
                ResettingBones();
                break;
            case PlayerState.GettingUp:
                GettingUpAnimationCheck();
                GettingUpBehaviour();
                break;
        }

        IsGrounded();
        CheckFalling();
        CheckLanding();
        HitTest();
        AnimationUpdate();
    }

    private void FixedUpdate()
    {
        FixedJump();
        FixedAttack();
    }

    private void LateUpdate()
    {
        if (ragdoll)
        {
            Vector3 pos = hipBones.position;
            transform.position = hipBones.position;
            hipBones.position = pos;
        }
    }

    private void GettingUpAnimationCheck()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || 
            Input.GetAxisRaw("Vertical") != 0)
        {
            isMoving = true;
        }
        else
            isMoving = false;
    }

    private void AnimationUpdate()
    {
        string updateAnim;

        if (attackOrder)
            updateAnim = attackAnim;
        else if (isJumping)
            updateAnim = jumpAnim;
        else if (isLanding)
            updateAnim = landingAnim;
        else if (isFalling)
            updateAnim = fallingAnim;
        else if (inMidAir)
            updateAnim = inMidAirAnim;
        else if (isMoving)
            updateAnim = moveAnim;
        else
            updateAnim = null;

        for (int i = 0; i < animlist.Count; i++)
        {
            bool playAnim = animlist[i] == updateAnim ? true : false;
            anim.SetBool(animlist[i], playAnim);
        }
    }

    private void SetJoint()
    {
        for (int i = 0; i < playerColliders.Length; i++)
        {
            playerColliders[i].enabled = !getUp;
        }
        playercol.enabled = getUp;
    }

    #region 캐릭터 움직임 과 MaxSpeed로의 속도제한
    private void Move()
    {
        Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
        moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;

        if (moveVec.sqrMagnitude != 0) transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.8f);

        // GetAxisRaw 의 입력값이 있는지의 여부를 bool로 판단하여 저장 
        bool vermove = Input.GetAxisRaw("Vertical") != 0 ? true : false;
        bool hormove = Input.GetAxisRaw("Horizontal") != 0 ? true : false;
        velocity = rigid.velocity;
        isMoving = vermove || hormove ? true : false;
        // ver, hor 둘 중 하나라도 true일 경우 true 저장

        rigid.AddForce(moveVec * moveSpeed);

        if (getUp)
            MaxSpeed();
    }

    private void MaxSpeed()
    {
        if (Mathf.Abs(rigid.velocity.x) > maxVelocity)
        {
            float posSpeed = rigid.velocity.x > 0 ? 1f : -1f;
            rigid.velocity = new Vector3(maxVelocity * posSpeed, rigid.velocity.y, rigid.velocity.z);
        }

        if (Mathf.Abs(rigid.velocity.z) > maxVelocity)
        {
            float posSpeed = rigid.velocity.z > 0 ? 1f : -1f;
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, maxVelocity * posSpeed);
        }
    }

    #endregion


    #region Idle 상태에서의 행동 명령 입력

    private void JumpOrder()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
            jumpOrder = true;
    }

    private void AttackOrder()
    {
        if (!attackOrder)
        {
            if (Input.GetButtonDown("Fire1") && isGrounded)
                attackOrder = true;
        }
    }

    #endregion


    #region 명령받은 행동을 FixedUpdate에서 실행하기 위한 함수
    private void FixedJump()
    {
        if (jumpOrder)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isLanding = false;
            jumpOrder = false;
            isJumping = true;
        }

    }
    private void FixedAttack()
    {
        if (attackOrder && attackcoroutine == null)
        {
            attackCollider.GetComponent<BoxCollider>().enabled = true;
            attackcoroutine = StartCoroutine(Attack());
        }
    }

    #endregion

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        attackCollider.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        attackOrder = false;
        attackcoroutine = null;
    }

    private void HitTest()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
             OnHit();
        }

    }

    private void GettingUpBehaviour()
    {
        if (!getUp)
        {
            gettingUp = 0;
            return;
        }

        gettingUp += Time.deltaTime;
        if(gettingUp > 1.2f)
        {
            gettingUp = 0;
            state = PlayerState.Idle;
        }
    }

    public void OnHit()
    {
        getUp = false;
        ragdoll = true;
        getupTimer = 0;
        OnRagDoll();
        state = PlayerState.GetDown;
    }

    private void GetUp()
    {
        getUp = true;
        playstandup = true;
        gettingUp = 0;
        rigid.velocity = Vector3.zero;
        ResettingPos();
        PopulateBonesTransform(ragdollBoneTransform);
        state = PlayerState.GetUp;
    }

    private void SetRigidBodyGravity()
    {
        foreach (Rigidbody rb in rig)
        {
            rb.useGravity = !getUp;
        }
    }

    private void OnRagDoll()
    {
        SetJoint();
        SetRigidBodyGravity();
        gameObject.GetComponent<Animator>().enabled = getUp;
        gameObject.GetComponent<Rigidbody>().useGravity = getUp;
    }

    private void OutRagDoll()
    {
        SetJoint();
        gameObject.GetComponent<Animator>().enabled = getUp;
        SetRigidBodyGravity();
        gameObject.GetComponent<Rigidbody>().useGravity = getUp;
        anim.Rebind();
    }

    private void PopulateBonesTransform(BoneTransform[] bonetransforms)
    {
        for (int bone = 0; bone < bones.Length; bone++)
        {
            bonetransforms[bone].position = bones[bone].localPosition;
            bonetransforms[bone].rotation = bones[bone].localRotation;
        }
    }

    private void StartAnimTransformCopy(string clipname, BoneTransform[] bone)
    {
        Vector3 vec = transform.position;
        Quaternion qua = transform.rotation;

        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clipname == clip.name)
            {
                clip.SampleAnimation(gameObject, 0);
                PopulateBonesTransform(animBoneTransform);
                break;
            }
        }

        transform.position = vec;
        transform.rotation = qua;
    }

    private void ResettingPos()
    {
        Vector3 originPos = hipBones.position;
        transform.position = hipBones.position;

        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            if(hit.transform.gameObject.layer == groundMask)
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }

        hipBones.position = originPos;
    }

    private void ResettingBones()
    {
        standupAnimTimer += Time.deltaTime;
        float standupPer = standupAnimTimer / resettingBonesTimer;
        SetJoint();
        gameObject.GetComponent<Rigidbody>().useGravity = true;

        if (Physics.Raycast(hipBones.position + (Vector3.up / 3), Vector3.down, out RaycastHit hit))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

        for (int bone = 0; bone < bones.Length; bone++)
        {
            bones[bone].localPosition = Vector3.Lerp(
                ragdollBoneTransform[bone].position,
                animBoneTransform[bone].position, standupPer);

            bones[bone].localRotation = Quaternion.Lerp(
                ragdollBoneTransform[bone].rotation,
                animBoneTransform[bone].rotation, standupPer);
        }

        if (standupPer >= 1.0f)
        {
            OutRagDoll();
            anim.Play(getupAnim);
            playstandup = false;
            ragdoll = false;
            standupAnimTimer = 0;
            state = PlayerState.GettingUp;
        }
    }

    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }

    private void CheckFalling()
    {
        if (isJumping)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer > 2f)
            {
                isFalling = true;
                isJumping = false;
                fallTimer = 0;
            }
        }
        else if (!isJumping && !isGrounded && !isFalling)
        {
            inMidAir = true;
            fallTimer += Time.deltaTime;
            if (fallTimer > 0.7f)
            {
                inMidAir = false;
                isFalling = true;
                fallTimer = 0;
            }
        }
        else if(isFalling)
        {
            fallTimer += Time.deltaTime;
            if (fallTimer > 1.5f)
                OnHit();
        }
        else
            fallTimer = 0;
    }

    private void CheckLanding()
    {
        if (!isFalling)
        {
            if (isGrounded && (isJumping || inMidAir) && rigid.velocity.y < 0)
            {
                isLanding = true;
                isJumping = false;
                inMidAir = false;
                StartCoroutine(LandingAnimation());
            }
        }
    }

    private IEnumerator LandingAnimation()
    {
        yield return new WaitForSeconds(0.05f);
        isLanding = false;
    }

    private void GetUpTimer()
    {
        if (Physics.CheckSphere(hipBones.position, groundRadious, groundMask))
        {
            getupTimer += Time.deltaTime;
            if (getupTimer > 1f)
            {
                GetUp();
            }
        }
        else
        { getupTimer = 0.0f; }
    }

    class BoneTransform
    {
        public Vector3 position { get; set; }

        public Quaternion rotation { get; set; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFalling)
        {
            OnHit();
            isFalling = false;
        }
    }

}