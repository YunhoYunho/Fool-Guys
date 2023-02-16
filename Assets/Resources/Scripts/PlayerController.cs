using Cinemachine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public enum PlayerState { Idle, GetDown, GetUp, GettingUp, Jump }

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviourPun, IPunObservable
{
    //============= Player Move ===============
    [Header("Player Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 moveVec;
    [SerializeField] private float maxVelocity = 5;
    //=========================================
    [Space]

    //============ Player Model ===============
    [SerializeField] public GameObject normalModel;
    [SerializeField] private Transform hipBones;
    [SerializeField] private BoneTransform[] ragdollBoneTransform;
    [SerializeField] private BoneTransform[] animBoneTransform;
    [SerializeField] private BoneTransform[] currentBoneTransform;
    [SerializeField] private Transform[] bones;
    [SerializeField] private Collider[] playerColliders;
    [SerializeField] private CapsuleCollider playercol;
    private CharacterJoint[] joint;
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
    [SerializeField] private bool isAttacking;
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


    //=============== Photon ==================
    [SerializeField]
    private TMP_Text nickName;

    public string Team;

    private PhotonView pv;

    private bool movable;

    private SkinnedMeshRenderer[] Skincolor;

    private GameManager gameManager;

    [SerializeField]
    private TMP_Text TeamText;

    public Vector3 myColor;


    private CinemachineFreeLook playerCam;
    private CinemachineVirtualCamera freeCam;


    //=============== Photon ==================
    [SerializeField] private ParticleSystem collisionEffect;

    private AudioSource audioSource;
    [SerializeField] private AudioClip bodyHit;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip falling;
    [SerializeField] private AudioClip respawn;


    //private CinemachineFreeLook freeCam;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //=== PlayerState SetUp ===
        //moveSpeed = 50f;
        //jumpPower = 13f;
        state = PlayerState.Idle;
        groundRadious = 0.4f;
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
        joint = GetComponentsInChildren<CharacterJoint>();
        playerCam = GameObject.Find("PlayerCamera").GetComponent<CinemachineFreeLook>();
        //freeCam = GameObject.Find("FreeCamera").GetComponent<CinemachineFreeLook>();
        freeCam = GameObject.Find("FreeCamera").GetComponent<CinemachineVirtualCamera>();
        audioSource = GetComponent<AudioSource>();
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
        SetRigidBodyCollisionDetection();
        SetJointProjection();
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
        if (!gameManager.onGameStart) return;

        switch (state)
        {
            case PlayerState.Idle:
                //MoveDetect();
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

        FixedMove();
        FixedJump();
        FixedAttack();
        //pv.RPC("FixedMove", RpcTarget.All);
        //pv.RPC("FixedJump", RpcTarget.All);
        //pv.RPC("FixedAttack", RpcTarget.All);
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

    public Transform[] GetBonesTransform()
    {
        return bones;
    }

    public bool GetStateRagDoll()
    {
        return ragdoll;
    }

    private void GettingUpAnimationCheck()
    {
        if (pv.IsMine == false) return;

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
        string upperAttackAnim;

        if (isJumping)
            updateAnim = jumpAnim;
        else if (isLanding)
            updateAnim = landingAnim;
        else if (isFalling)
            updateAnim = fallingAnim;
        else if (inMidAir)
            updateAnim = inMidAirAnim;
        else if (isMoving)
            updateAnim = moveAnim;
        else if (isAttacking)
            updateAnim = attackAnim;
        else
            updateAnim = null;

        if (attackOrder)
            upperAttackAnim = attackAnim;
        else
            upperAttackAnim = null;

        for (int i = 0; i < animlist.Count; i++)
        {
            bool playAnim = animlist[i] == updateAnim ? true : false;
            anim.SetBool(animlist[i], playAnim);
        }

        bool attackOnOff = upperAttackAnim == null ? false : true;
        anim.SetBool(attackAnim, attackOnOff);

        upperAttackAnim = null;
    }
    private void SetJoint()
    {
        for (int i = 0; i < playerColliders.Length; i++)
        {
            playerColliders[i].enabled = !getUp;
        }
        playercol.isTrigger/* enabled */= !getUp;
    }

    #region 캐릭터 움직임 과 MaxSpeed로의 속도제한

    //private void MoveDetect()
    //{
    //    if (pv.IsMine == false) return;

    //    pv.RPC("Move", RpcTarget.All);
    //}

    private void Move()
    {
        if (pv.IsMine == false) return;

        Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

        Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
        moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;

        if (moveVec.sqrMagnitude != 0) transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.4f);
        //transform.forward = Vector3.Lerp(transform.forward, moveVec, Time.fixedDeltaTime * 10);


        // GetAxisRaw 의 입력값이 있는지의 여부를 bool로 판단하여 저장 
        bool vermove = Input.GetAxisRaw("Vertical") != 0 ? true : false;
        bool hormove = Input.GetAxisRaw("Horizontal") != 0 ? true : false;
        velocity = rigid.velocity;

        // ver, hor 둘 중 하나라도 true일 경우 true 저장
        bool Val = vermove || hormove ? true : false;

        pv.RPC("moveAnimDetect", RpcTarget.All, Val);


        if (getUp)
            MaxSpeed();
    }

    [PunRPC]
    private void moveAnimDetect(bool val)
    {
        isMoving = val;
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
        if (pv.IsMine == false) return;

        if (Input.GetButtonDown("Jump") && isGrounded)
            jumpOrder = true;
    }

    private void jumpAnimDetect(bool val)
    {

    }

    private void AttackOrder()
    {
        if (pv.IsMine == false) return;

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
            pv.RPC("JumpDetect", RpcTarget.All, true);
            //isJumping = true;
            audioSource.clip = jump;
            audioSource.Play();
            isLanding = false;
            jumpOrder = false;
        }

    }

    [PunRPC]
    private void JumpDetect(bool val)
    {
        isJumping = val;
    }

    private void FixedAttack()
    {
        if (!pv.IsMine) return;


        if (attackOrder && attackcoroutine == null)
        {
            //isAttacking = true;
            pv.RPC("AttackAnimDetect", RpcTarget.All, true);
            attackCollider.GetComponent<BoxCollider>().enabled = true;
            attackcoroutine = StartCoroutine(Attack());
        }
    }

    private void FixedMove()
    {

        //isMoving = moveVec.sqrMagnitude != 0 ? true : false;
        rigid.AddForce(moveVec * moveSpeed);
    }

    #endregion

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        attackCollider.GetComponent<BoxCollider>().enabled = false;
        //audioSource.clip = punch;
        //audioSource.Play();
        yield return new WaitForSeconds(0.2f);
        //attackOrder = false;
        pv.RPC("AttackAnimDetect", RpcTarget.All, false);
        //isAttacking = false;
        attackcoroutine = null;
    }

    [PunRPC]
    private void AttackAnimDetect(bool val)
    {
        attackOrder = val;
        isAttacking = val;
    }


    private void HitTest()
    {
        if (pv.IsMine == false) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            pv.RPC("OnHit", RpcTarget.All);
            //OnHit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TestRespawn();
        }

    }

    private void Respawn()
    {
        moveVec = Vector3.zero;
        rigid.velocity = Vector3.zero;
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
        gameObject.transform.position = gameManager.NowRespawnArea.transform.position;
        gameObject.transform.rotation = rotation;
        audioSource.clip = respawn;
        audioSource.Play();
    }

    private void TestRespawn()
    {
        moveVec = Vector3.zero;
        rigid.velocity = Vector3.zero;
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
        gameObject.transform.position = gameManager.TestRespawnArea.transform.position;
        gameObject.transform.rotation = rotation;
    }


    private void GettingUpBehaviour()
    {
        if (!getUp)
        {
            gettingUp = 0;
            return;
        }

        gettingUp += Time.deltaTime;
        if (gettingUp > 1.2f)
        {
            gettingUp = 0;
            state = PlayerState.Idle;
        }
    }

    [PunRPC]
    public void OnHit()
    {
        isFalling = false;
        getUp = false;
        ragdoll = true;
        getupTimer = 0;
        audioSource.clip = bodyHit;
        audioSource.Play();
        //gameObject.GetComponent<PhotonAnimatorView>().enabled = getUp;
        gameObject.GetComponent<Animator>().enabled = getUp;
        OnRagDoll();
        rigid.velocity = Vector3.zero;
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

    private void SetRigidBodyCollisionDetection()
    {
        foreach (Rigidbody rb in rig)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void OnRagDoll()
    {
        SetJoint();
        SetRigidBodyGravity();
        gameObject.GetComponent<Animator>().enabled = getUp;
        gameObject.GetComponent<Rigidbody>().useGravity = getUp;
        rigid.velocity = Vector3.zero;
        moveVec = Vector3.zero;
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

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            if (hit.transform.gameObject.layer == groundMask)
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

    private void SetJointProjection()
    {
        foreach (CharacterJoint pj in joint)
        {
            pj.enableProjection = true;
        }
    }

    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
    }

    private void CheckFalling()
    {
        if (!pv.IsMine) return;

        if (isJumping)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer > 1.5f)
            {
                isFalling = true;
                pv.RPC("JumpDetect", RpcTarget.All, false);
                fallTimer = 0;
            }
        }
        else if (!isJumping && !isGrounded && !isFalling)
        {
            inMidAir = true;
            fallTimer += Time.deltaTime;
            if (fallTimer > 0.75f)
            {
                inMidAir = false;
                isFalling = true;
                fallTimer = 0;
                audioSource.clip = falling;
                audioSource.Play();
            }
        }
        //else if (isFalling)
        //{
        //    fallTimer += Time.deltaTime;
        //    if (fallTimer > 1.5f)
        //    {
        //        //OnHit();
        //    }
        //}
        else
            fallTimer = 0;
    }

    private void CheckLanding()
    {
        if (!pv.IsMine) return;

        if (!isFalling)
        {
            if (isGrounded && (isJumping || inMidAir) && rigid.velocity.y < 0)
            {
                isLanding = true;
                pv.RPC("JumpDetect", RpcTarget.All, false);
                //isJumping = false;
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

    [PunRPC]
    public void SetColor()
    {
        Player player = gameObject.GetComponent<PhotonView>().Owner;

        object R, G, B;

        if (player.CustomProperties.TryGetValue("R", out R) &&
            player.CustomProperties.TryGetValue("G", out G) &&
            player.CustomProperties.TryGetValue("B", out B))
        {
            Skincolor = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            Color playercolor = new Color((float)R, (float)G, (float)B);
            myColor = new Vector3((float)R, (float)G, (float)B);
            for (int i = 0; i < Skincolor.Length; i++)
            {
                Skincolor[i].material.color = playercolor;
            }
        }
    }

    [PunRPC]
    public void SetNickname()
    {
        Player player = gameObject.GetComponent<PhotonView>().Owner;
        int rnd = Random.Range(1000, 9999);

        nickName.text = player.NickName != "" ? player.NickName + "\n<size=0.1>▼" : "Tester" + rnd + "\n<size=0.1>▼";
    }


    [PunRPC]
    public void SetTeam(string team)
    {
        Team = team;

        switch (team)
        {
            //case "Red": TeamText.text = "<color=red>[RED]</color>"; gameManager.GoalPointChange("Red", 1); break;
            case "Red": TeamText.text = "<color=red>[RED]</color>"; if (pv.IsMine) gameManager.GetComponent<PhotonView>().RPC("GoalPointChange", RpcTarget.All, "Red", 1); break;
            case "Blue": TeamText.text = "<color=blue>[BLUE]</color>"; if (pv.IsMine) gameManager.GetComponent<PhotonView>().RPC("GoalPointChange", RpcTarget.All, "Blue", 1); break;
            //case "Blue": TeamText.text = "<color=blue>[BLUE]</color>"; gameManager.GoalPointChange("Blue", 1); break;
            default: break;
        }

        if (pv.IsMine)
        {
            Player player = gameObject.GetComponent<PhotonView>().Owner;
            gameManager.GetComponent<PhotonView>().RPC("AddTeamList", RpcTarget.All, Team, player.NickName);

        }
    }

    //public void SendMyInfo()
    //{
    //    if (gameManager.WinTeam == Team)
    //    {
    //        gameManager.GetComponent<PhotonView>().RPC("AddWinnerNicknameList", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
    //        //gameManager.Winner.Add(PhotonNetwork.LocalPlayer.NickName);
    //    }

    //}

    class BoneTransform
    {
        public Vector3 position { get; set; }

        public Quaternion rotation { get; set; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (!pv.IsMine) return;

        if (isFalling)
        {
            pv.RPC("OnHit", RpcTarget.All);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            collisionEffect.Play();
            Debug.Log("Effect");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!pv.IsMine) return;

        if (other.gameObject.CompareTag("RespawnPlane"))
        {
            Respawn();
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            if (pv.IsMine)
            {
                gameManager.GetComponent<PhotonView>().RPC("GoalPointChange", RpcTarget.All, Team, -1);
                gameManager.GetComponent<PhotonView>().RPC("CheckGoalIn", RpcTarget.All, 1);
                playerCam.gameObject.SetActive(false);
                freeCam.gameObject.SetActive(true);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


}