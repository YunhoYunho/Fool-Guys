using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] private GameObject ragdollModel;
    [SerializeField] private Transform hipBones;
    [SerializeField] private BoneTransform[] ragdollBoneTransform;
    [SerializeField] private BoneTransform[] animBoneTransform;
    [SerializeField] private Transform[] bones;
    [SerializeField] private Collider[] playerColliders;
    [SerializeField] private CapsuleCollider playercol;
    //=========================================

    //================ Attack =================
    [SerializeField] private GameObject attackCollider;
    private Coroutine attackcoroutine;
    private Coroutine getUpCoroutine;
    //=========================================

    //=========== Player Controller ===========
    private Rigidbody rigid;
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

    //=============== Animator =================
    private List<string> animlist;
    private string moveanim = "isMoving";
    [SerializeField] string getupClipName;
    //=========================================

    private void Awake()
    {
        moveSpeed = 20;
        jumpPower = 10f;

        attackcoroutine = null;
        getUpCoroutine = null;

        bones = hipBones.GetComponentsInChildren<Transform>();
        animBoneTransform = new BoneTransform[bones.Length];
        ragdollBoneTransform = new BoneTransform[bones.Length];

        StartAnimTransformCopy(getupClipName, animBoneTransform);

        for (int bone = 0; bone < bones.Length; bone++)
        {
            animBoneTransform[bone] = new BoneTransform();
            ragdollBoneTransform[bone] = new BoneTransform();
        }

        animlist = new List<string>();
        getUp = true;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        playercol = GetComponent<CapsuleCollider>();
        //playerColliders = gameObject.GetComponentsInChildren<Collider>();
        playerColliders = normalModel.GetComponentsInChildren<Collider>();
        SetAnimList();
        hipBones = anim.GetBoneTransform(HumanBodyBones.Hips);

        SetJoint();
        JointEnable();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetAnimList()
    {
        animlist.Add(moveanim);
    }

    private void Update()
    {
        Move();
        Jump();
        Attack();
        IsGrounded();
        HitTest();
        AnimationUpdate();

        if (!getUp)
        {
            FollowCamera();
            FollowRagDollPosision();
        }
    }

    private void FixedUpdate()
    {
        FixedMove();
        FixedJump();
        FixedAttack();
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

    private void SetJoint()
    {
        for (int i = 0; i < playerColliders.Length; i++)
        {
            playerColliders[i].enabled = !getUp;
        }
        playercol.enabled = getUp;
    }

    private void JointEnable()
    {
        CharacterJoint[] joints = normalModel.GetComponentsInChildren<CharacterJoint>();
        
        for(int i=0; i< joints.Length; i++)
        {
            joints[i].enableProjection = true;
        }
    }

    private void Move()
    {
        if (!getUp)
            return;

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
        if (!getUp)
            return;
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
        if(attackOrder && attackcoroutine == null)
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
        if(Input.GetButtonDown("Jump") && isGrounded)
            jumpOrder = true;
    }

    private void HitTest()
    {
        if (Input.GetKeyDown(KeyCode.P))
            OnHit();

    }
    public void OnHit()
    {
        getUp = false;
        ChangeRagDoll();
    }

    private IEnumerator GetUp()
    {
        yield return new WaitForSeconds(1.5f);
        getUp = true;
        ChangeRagDoll();
        PopulateBonesTransform(ragdollBoneTransform);
        getUpCoroutine = null;
    }

    private void ChangeRagDoll()
    {
        SetJoint();
        gameObject.GetComponent<Animator>().enabled = getUp;
        if (getUpCoroutine == null)
            getUpCoroutine = StartCoroutine(GetUp());
    }

    private void ChangeModelTransformCopy(Transform ChangeStart, Transform ChangeEnd)
    {
        for (int i = 0; i < ChangeStart.transform.childCount; i++)
        {
            if (ChangeStart.transform.childCount != 0)
            {
                ChangeModelTransformCopy(ChangeStart.transform.GetChild(i), ChangeEnd.transform.GetChild(i));
            }
            ChangeEnd.transform.GetChild(i).localPosition = ChangeStart.transform.GetChild(i).localPosition;
            ChangeEnd.transform.GetChild(i).localRotation = ChangeStart.transform.GetChild(i).localRotation;
        }
    }

    private void FollowRagDollPosision()
    {
        Vector3 originPos = hipBones.transform.position;

        transform.position = Vector3.Lerp(transform.position, hipBones.transform.position, Time.fixedDeltaTime * 10);

        Vector3 pos = animBoneTransform[0].posision;
        pos.y = 0;
        pos = transform.rotation * pos;
        transform.position -= pos;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);


        hipBones.transform.position = originPos;
    }

    private void PopulateBonesTransform(BoneTransform[] bonetransforms)
    {
         for(int bone =0; bone< bones.Length; bone++)
        {
            bonetransforms[bone].posision = bones[bone].localPosition;
            bonetransforms[bone].rotation = bones[bone].localRotation;
        }
    }

    private void StartAnimTransformCopy(string clipname, BoneTransform[] bone)
    {
        Vector3 vec = transform.position;
        Quaternion qua = transform.rotation;

        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if(clipname == clip.name)
            {
                clip.SampleAnimation(gameObject,0);
                PopulateBonesTransform(animBoneTransform);
                break;
            }
        }

        transform.position = vec;
        transform.rotation = qua; 
    }

    private void FollowCamera()
    {
        Camera cam = Camera.main;

        cam.transform.position = rigid.position;
    }


    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }

    class BoneTransform
    {
        public Vector3 posision { get; set; }

        public Quaternion rotation { get; set; }
    }

}