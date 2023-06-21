using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
public enum PlayerPosture
{ 
    Crouch,
    Stand,
    Jumping,
    Falling,
    Landing,
    Attack,
    Hit,
    Roll,
    Climbing,
    Push
}
public enum LocomotionState
{ 
    Idle,
    Walk,
    Run
}
public enum ArmState
{ 
    Normal,
    Aim
}
public class PlayerController : MonoBehaviour
{
    #region 字段属性
    public PlayerPosture playerPosture = PlayerPosture.Stand;
    public LocomotionState locomotionState = LocomotionState.Idle;
    public ArmState armState = ArmState.Normal;
    float crouchThreshold = 0f;
    float standThreshold = 1f;
    float midairThreshold = 2.1f;
    float feetTWeen;
    float stateTimer;
    [Header("下蹲移动速度")]
    public float crouchSpeed=1f;
    [Header("行走移动速度")]
    public float walkSpeed=1.5f;
    [Header("跑步速度")]
    public float runSpeed=5.5f;

    bool isRunning=false;
    bool isCrouch=false;
    bool isAiming=false;
    bool isJumping=false;
    bool isAttack = false;
    bool isHit = false;
    bool isInvincible = false;
    bool isRoll = false;
    public bool isIdle = false; //对话，摄像机移动时，让角色处于等待状态;
    //状态机变量的哈希值
    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;
    int verticalSpeedHash;
    int landrFoorHash;
    int lattackHash;
    int stateTimerHash;
    int hitHash;
    int deathHash;
    int rollHash;
    //重力
    public float gravity = -9.8f;
    //垂直方向的速度
    float verticalVelocity;
    //跳跃速度
    float jumpSpeed = 5f;
    //弹跳高度
    public float maxHeight = 1.5f;


    Transform playerTransform;
    Animator animator;
    CharacterController characterController;
    Vector2 moveInput;
    //玩家实际移动的方向
    Vector3 playerMovement = Vector3.zero;
    //摄像机的坐标
    public Transform cameraTransform;
    //玩家实际移动的速度
    float realySpeed;
    

    //取跳跃的最后三帧
    static readonly int CACHE_SIZE = 3;
    Vector3[] velCache = new Vector3[CACHE_SIZE];
    int currentCaCheIndex = 0; //缓存池中最老的索引
    Vector3 averageSpeed = Vector3.zero;

    //地面检测
    bool isGround;
    //是否可以跌落
    bool coulFall;
    //跌落的高度
    float fallHeight = 0.2f;
    float groundCheckOffset = 0.5f;
    //是否处于跳跃CD状态
    bool isLanding;
    //跳跃CD设置
    float jumpCD = 0.15f;
    //无敌cd
    public float invincibleTimer = 0f;
    public float invincibleCD=0.5f;
    #endregion

    #region  攀爬相关变量
    public PlayerSensor playerSensor;
    Vector3 playerMovementWorldSpace = Vector3.zero;     //玩家在世界坐标下的输入方向
    bool isClimbReady;                                        //当前是否做好攀爬准备

    int defaultClimbParameter = 0;                                         //攀爬的动画参数
    int vaultParameter = 1;                                                       //攀爬
    int lowClimbParameter = 2;                                                //低位攀爬
    int highClimbParameter = 3;                                               //高位攀爬
    int currentClimbParameter;                                             //当前正在使用的攀爬变量

    Vector3 leftHandPosition;                                                //攀爬时左手的位置
    Vector3 rightHandPosition;                                             //攀爬时右手的位置
    Vector3 rightFootPosition;                                              //攀爬时右脚的位置


    #endregion
    #region  推拉相关变量
    bool isPush;
    bool pushStateChanged;
    Transform interactPoint;
    Transform rightTarget;
    Transform leftTarget;
    MoveableObject moveableObject;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;
    public RigBuilder rigBuilder;
    #endregion

    #region  内部函数
    private void Start()
    {
        playerTransform = transform;
        animator = GetComponent<Animator>();
        postureHash = Animator.StringToHash("玩家姿态");
        moveSpeedHash = Animator.StringToHash("移动速度");
        turnSpeedHash = Animator.StringToHash("转向速度");
        verticalSpeedHash = Animator.StringToHash("垂直速度");
        landrFoorHash = Animator.StringToHash("左右脚参数");
        lattackHash = Animator.StringToHash("LAttack");
        stateTimerHash = Animator.StringToHash("StateTimer");
        deathHash = Animator.StringToHash("Death");
        hitHash = Animator.StringToHash("Hit");
        rollHash = Animator.StringToHash("Roll");
        characterController = GetComponent<CharacterController>();
        playerSensor = GetComponent<PlayerSensor>();
    }
    private void Update()
    {
        //角色处于isIdle时，禁止其它操作
        if (isIdle)
        {
            
            return;
        }
        invincibleTimer += Time.deltaTime;
        if (invincibleTimer > invincibleCD)
        {
            isInvincible = false;
        }
        ReSetState();                //重置状态
        CheckGround();
        AnimationController();       //is判断
        InputController();            //输入x,y控制
        SwitchPlayerStates();         //角色状态的转换
        CacaulateGravity();           //计算重力
        Jump();
        Push();
        
        CaculateInputDirection();     //计算方向
        PlayerMoveAniamtor();         //控制动画
        SetAnimationRiggingWeight();  //设置animator里的左右手权重
    }
    #endregion

    #region  输入指令
    void InputController()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
    }

    void Jump()
    {
        if (playerPosture ==PlayerPosture.Stand && isJumping)
        {
            
            float velOffset;
            switch (locomotionState)
            {
                case LocomotionState.Run:
                    velOffset = 2f;
                    break;
                case LocomotionState.Walk:
                    velOffset = 0.8f;
                    break;
                case LocomotionState.Idle:
                    velOffset = 0f;
                    break;
                default:
                    velOffset = 0f;
                    break;
            }
            
            switch (playerSensor.ClimbDetect(playerTransform, playerMovementWorldSpace,velOffset))
            {
                
                case NextPlayerMovement.jump:
                    verticalVelocity = Mathf.Sqrt(-2 * gravity * maxHeight);
                    feetTWeen = Random.Range(-1f, 1f);
                    //获得0的动画层里的播放状态。
                    feetTWeen = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
                    feetTWeen = feetTWeen < 0.5f ? 1 : -1;
                    if (locomotionState == LocomotionState.Run)
                    {
                        feetTWeen *= 3;
                    }
                    else if (locomotionState == LocomotionState.Walk)
                    {
                        feetTWeen *= 2;
                    }
                    else
                    {
                        feetTWeen = Random.Range(0.5f, 1f) * feetTWeen;
                    }
                    break;
                case NextPlayerMovement.climbLow:
                    leftHandPosition = playerSensor.ledge + Vector3.Cross(-playerSensor.climbHitNormal, Vector3.up) * 0.3f;
                    currentClimbParameter = lowClimbParameter;
                    isClimbReady = true;
                    break;
                case NextPlayerMovement.climbHigh:
                    rightHandPosition = playerSensor.ledge + Vector3.Cross(playerSensor.climbHitNormal, Vector3.up) * 0.3f;
                    rightFootPosition = playerSensor.ledge + Vector3.down*1.2f;
                    currentClimbParameter = highClimbParameter;
                    isClimbReady = true;
                    break;
                case NextPlayerMovement.vault:
                    rightHandPosition = playerSensor.ledge;
                    currentClimbParameter = vaultParameter;
                    isClimbReady = true;                    
                    break;
            }
            
        }
    }

    /// <summary>
    /// 推拉方法，用来处理推拉箱子的状态切换
    /// </summary>
    void Push()
    {
        if (isPush && playerPosture == PlayerPosture.Stand)
        {
            moveableObject = playerSensor.MoveableObjectCheck(playerTransform, playerMovementWorldSpace);
            if (moveableObject)
            {
                InteractPoint interact = moveableObject.GetInteractPoint(playerTransform);
                interactPoint = interact.interactPoint;
                rightTarget = interact.rightHandTarget;
                leftTarget = interact.leftHandTarget;
                rightHandConstraint.data.target = rightTarget;
                leftHandConstraint.data.target = leftTarget;
                rigBuilder.Build();
                animator.Rebind();

                pushStateChanged = true;
            }
        }
        else if (!isPush && playerPosture == PlayerPosture.Push)
        {
            pushStateChanged = true;
        }
    }
    #endregion
    /// <summary>
    /// 角色状态得转换
    /// </summary>
     void SwitchPlayerStates()
    {
        switch (playerPosture)
        {
            case PlayerPosture.Stand:
                if (verticalVelocity > 0)
                {
                    playerPosture = PlayerPosture.Jumping;
                }
                else if (!isGround && coulFall)
                {
                    playerPosture = PlayerPosture.Falling;
                }
                else if (isCrouch)
                {
                    playerPosture = PlayerPosture.Crouch;
                }
                else if (isClimbReady)
                {
                    playerPosture = PlayerPosture.Climbing;
                }
                else if (isAttack)
                {
                    playerPosture = PlayerPosture.Attack;
                }
                else if (isRoll)
                {
                    playerPosture = PlayerPosture.Roll;
                }
                else if (isHit)
                {
                    playerPosture = PlayerPosture.Hit;
                }
                else if (pushStateChanged)
                {
                    playerPosture = PlayerPosture.Push;
                }
                break;
            case PlayerPosture.Crouch:
                if (!isGround && coulFall)
                {
                    playerPosture = PlayerPosture.Falling;
                }
                else if (!isCrouch)
                {
                    playerPosture = PlayerPosture.Stand;
                }
                break;
            case PlayerPosture.Jumping:
                if (isGround)
                {
                    StartCoroutine(CoolDownJump());
                }
                if (isLanding)
                {
                    playerPosture = PlayerPosture.Landing;
                }
                break;
            case PlayerPosture.Falling:
                if (isGround)
                {
                    StartCoroutine(CoolDownJump());
                }
                if (isLanding)
                {
                    playerPosture = PlayerPosture.Landing;
                }
                break;
            case PlayerPosture.Landing:
                if (!isLanding)
                {
                    playerPosture = PlayerPosture.Stand;
                }
                break;
            case PlayerPosture.Attack:
                if (!isAttack)
                {
                    playerPosture = PlayerPosture.Stand;
                }
                break;
            case PlayerPosture.Hit:
                if (!isHit)
                {
                    playerPosture = PlayerPosture.Stand; 
                }
                break;
            case PlayerPosture.Roll:
                if (!isRoll)
                {
                    playerPosture = PlayerPosture.Stand;
                }
                break;
            case PlayerPosture.Climbing:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("攀爬")&& !animator.IsInTransition(0) )
                {
                    playerPosture = PlayerPosture.Stand;
                }
                break;
            case PlayerPosture.Push:
                if (pushStateChanged)
                {
                    playerPosture = PlayerPosture.Stand;
                }
                break;
        }
        isClimbReady = false;
        pushStateChanged = false;
        //角色移动的状态设置
        if (moveInput.magnitude == 0)
        {
            locomotionState = LocomotionState.Idle;
        }
        else if (!isRunning)
        {
            locomotionState = LocomotionState.Walk;
        }
        else
        {
            locomotionState = LocomotionState.Run;
        }
        //if (isAiming)
        //{
        //    armState = ArmState.Aim;
        //}
        //else
        //{
        //    armState = ArmState.Normal;
        //}
    }

    void CheckGround()
    {
        //由offset向下发射一个球形射线，距离为 groundCheckOffset - characterController.radius + 2 * characterController.skinWidth ,脚底+两个skinWidth
        if (Physics.SphereCast(playerTransform.position + (Vector3.up * groundCheckOffset), characterController.radius, Vector3.down, out RaycastHit hit, groundCheckOffset - characterController.radius + 2 * characterController.skinWidth))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
            coulFall = !Physics.Raycast(playerTransform.position, Vector3.down, fallHeight);

        }
    }
    void CaculateInputDirection()
    {
        Vector3 camForwardProject = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        playerMovementWorldSpace = camForwardProject * moveInput.y +cameraTransform.right * moveInput.x;
        playerMovement = playerTransform.InverseTransformVector(playerMovementWorldSpace);
    }
    void CacaulateGravity()
    {
        if (playerPosture!=PlayerPosture.Jumping && playerPosture!=PlayerPosture.Falling)
        {
            if (isGround)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
            else
            {
                verticalVelocity = gravity * Time.deltaTime;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }
    /// <summary>
    /// 状态机设置
    /// </summary>
    void PlayerMoveAniamtor()
    {

        //翻滚
        if (playerPosture == PlayerPosture.Roll)
        {
            animator.SetTrigger(rollHash);
        }
        //攻击
        else if (playerPosture == PlayerPosture.Attack)
        {

            animator.SetTrigger(lattackHash);

        }
        else if (playerPosture == PlayerPosture.Hit && !isInvincible)
        {
            invincibleTimer = 0;
            animator.SetTrigger(hitHash);
            isInvincible = true;

        }
        else if (playerPosture == PlayerPosture.Stand)
        {
            animator.SetBool("推拉", false);
            animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:

                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:

                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:

                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
                default:
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Crouch)
        {
            animator.SetFloat(postureHash, 0, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                default:
                    animator.SetFloat(moveSpeedHash, moveInput.magnitude * crouchSpeed, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Jumping)
        {

            animator.SetFloat(postureHash, midairThreshold, 0.1f, Time.deltaTime);
            animator.SetFloat(verticalSpeedHash, verticalVelocity, 0.1f, Time.deltaTime);
            animator.SetFloat(landrFoorHash, feetTWeen);
        }
        else if (playerPosture == PlayerPosture.Landing)
        {
            animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
            switch (locomotionState)
            {
                case LocomotionState.Idle:

                    animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Walk:

                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                    break;
                case LocomotionState.Run:

                    animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                    break;
                default:
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Falling)
        {
            animator.SetFloat(postureHash, midairThreshold, 0.1f, Time.deltaTime);
            animator.SetFloat(verticalSpeedHash, verticalVelocity, 0.1f, Time.deltaTime);
        }
        else if (playerPosture == PlayerPosture.Climbing)
        {
            animator.SetInteger("攀爬方式", currentClimbParameter);
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.LookRotation(-playerSensor.climbHitNormal), 0.5f);
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("爬"))
            {
                currentClimbParameter = defaultClimbParameter;
                //可以根据自己得需求来调整设置
                animator.MatchTarget(leftHandPosition, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.1f);
                animator.MatchTarget(leftHandPosition + Vector3.up * 0.18f, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.up, 0f), 0.1f, 0.3f);
            }
            else if (info.IsName("爬高"))
            {
                currentClimbParameter = defaultClimbParameter;
                animator.MatchTarget(rightFootPosition, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.13f);
                animator.MatchTarget(rightHandPosition, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 0f), 0.2f, 0.32f);

            }
            else if (info.IsName("翻越"))
            {
                currentClimbParameter = defaultClimbParameter;
                animator.MatchTarget(rightHandPosition, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.2f);
                animator.MatchTarget(rightHandPosition + Vector3.up * 0.1f, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 0f), 0.35f, 0.45f);
            }
        }
        else if (playerPosture == PlayerPosture.Push)
        {
            animator.SetBool("推拉",true);
            animator.SetBool("推", moveInput.y > 0);
            animator.SetBool("拉", moveInput.y < 0);
        }
        //设置转向
        if (armState == ArmState.Normal && playerPosture!=PlayerPosture.Jumping&&playerPosture!=PlayerPosture.Push)
        {
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            animator.SetFloat(turnSpeedHash, rad, 0.1f, Time.deltaTime*4);
            playerTransform.Rotate(0, rad * 200 * Time.deltaTime, 0f);
        }
    }
    void SetAnimationRiggingWeight()
    {
        rightHandConstraint.weight = animator.GetFloat("RightHandWeight");
        leftHandConstraint.weight = animator.GetFloat("LeftHandWeight");
    }
    void AnimationController()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouch = !isCrouch;
        }
        if (Input.GetMouseButton(1))
        {
            isRoll = true;
        }
        else
        {
            isRoll = false;
        }
        if (playerPosture == PlayerPosture.Stand && Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            isPush = true;
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            isPush = false;
        }
    }
    /// <summary>
    /// 计算缓存池中的平均速度,用于“平滑”或者“去噪”场景
    /// </summary>
    /// <param name="newVal">这一帧的速度</param>
    /// <returns></returns>
    Vector3 AverageVel(Vector3 newVal)
    {
        velCache[currentCaCheIndex] = newVal;
        currentCaCheIndex++;
        currentCaCheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3 vel in velCache)
        {
            average += vel;
        }
        return average / CACHE_SIZE;
    }

    private void OnAnimatorMove()
    {
        if (isIdle)
        { return; }
        if (playerPosture == PlayerPosture.Climbing)
        {
            characterController.enabled = false;
            animator.ApplyBuiltinRootMotion();           //激活动画Root motion
        }
        else if (playerPosture == PlayerPosture.Push)
        {
            characterController.enabled = true;
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("开始推拉") || info.IsName("空手运动"))
            {
                animator.ApplyBuiltinRootMotion();
                animator.MatchTarget(interactPoint.position, interactPoint.rotation, AvatarTarget.Root, new MatchTargetWeightMask(Vector3.one, 1f), 0.2f, 0.5f);

            }
            else
            {
                Vector3 playerDeltaMovement = animator.deltaPosition;
                playerDeltaMovement.y = verticalVelocity * Time.deltaTime;
                characterController.Move(playerDeltaMovement);
                averageSpeed = AverageVel(animator.velocity);
                if (moveableObject)
                {
                    playerDeltaMovement = playerTransform.InverseTransformDirection(playerDeltaMovement);  //转到世界坐标
                    playerDeltaMovement.x = 0; 
                    playerDeltaMovement = playerTransform.TransformDirection(playerDeltaMovement);         //转到本地坐标

                    playerDeltaMovement.y = 0;
                    moveableObject.transform.Translate(playerDeltaMovement);
                }
            }

        }
        else if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            characterController.enabled = true;
            Vector3 playerDeltaMovement = animator.deltaPosition;
            playerDeltaMovement.y = verticalVelocity * Time.deltaTime;
            characterController.Move(playerDeltaMovement);
            averageSpeed = AverageVel(animator.velocity);
        }
        else
        {
            characterController.enabled = true;
            averageSpeed.y = verticalVelocity;
            Vector3 playerDeltaMovement = averageSpeed * Time.deltaTime; //防止帧率不同导致的异常
            characterController.Move(playerDeltaMovement);

        }
    }
    public void OnHurt()
    {
        
        isHit = true;
    }
    public void OnDeath()
    {
        animator.SetTrigger(deathHash);
    }
    public void OnDeathEvent()
    { 
        //暂停游戏时间

        //显示菜单界面
    }
    public void EndHurt()
    {
        invincibleTimer = 0;
        isHit = false;
    }
    IEnumerator CoolDownJump()
    {
        isLanding = true;
        playerPosture = PlayerPosture.Landing;
        yield return new WaitForSeconds(jumpCD);
        isLanding = false;
    }

    void ReSetState()
    {
        animator.ResetTrigger(lattackHash);
        //animator.ResetTrigger("Magic1");
        animator.ResetTrigger(rollHash);
        stateTimer = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
        animator.SetFloat(stateTimerHash, stateTimer);
    }
}
