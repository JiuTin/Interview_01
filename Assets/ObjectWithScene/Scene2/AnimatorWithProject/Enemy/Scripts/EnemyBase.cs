using System;

using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{

    public float checkDistance;    //检车距离
    public float maxHeightDiff;    //检测高度
    [Range(0, 180)]
    public float lookAngle;           //视野角度
    protected RaycastHit[] hits = new RaycastHit[10];    //检测目标
    public LayerMask mask;          //检测的层级
    public GameObject target;       //目标
    protected NavMeshAgent navMesh;
    public float followDistance;  //追踪的距离
    public float attackDistance;  //攻击的距离
    Vector3 defaultPos;           //初始位置

    //速度
    protected float runSpeed = 3.5f;
    protected float walkSpeed = 1f;
    protected bool isAttack = false;
    public  float attackCD;
    private float attackTimer;
    protected Animator animator;
    protected Rigidbody rig;
    bool isDeath=false;

    //哈希值
    protected int speedHash;
    protected int attackHash;
    protected int deathHash;
    //
    protected virtual void Start()
    {
        defaultPos = transform.position;
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");
        attackHash = Animator.StringToHash("Attack");
        rig = GetComponent<Rigidbody>();
        deathHash = Animator.StringToHash("Death");
    }
    protected virtual void Update()
    {
        if (isDeath)
        { return; }
        CheckAttackCD();
        CheckTarget();
        FollowTarget();
    }
    private void OnAnimatorMove()
    {
        if (!isAttack && !isDeath)
        { 
            rig.MovePosition(transform.position + animator.deltaPosition);
        }
    }
    /// <summary>
    /// 搜索目标
    /// </summary>
    protected virtual void CheckTarget()
    {
        int count = Physics.SphereCastNonAlloc(transform.position, checkDistance, Vector3.forward, hits, 0.1f, mask);
        for (int i = 0; i < count; i++)
        {
            //判断是不是可以攻击的游戏物体
            if (!hits[i].transform.GetComponent<Damageable>())
            { continue; }
            //是不是在视野范围内
            if (Vector3.Angle(transform.forward, hits[i].transform.position - transform.position) > lookAngle)
            {
                continue;
            }
            //找到目标(离自己距离最进的目标)
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, hits[i].transform.position);
                float current = Vector3.Distance(transform.position, target.transform.position);
                if (distance < current)
                {
                    target = hits[i].transform.gameObject;
                }
            }
            else
            {
                target = hits[i].transform.gameObject;
            }
        }
    }
    //移动到目标
    protected virtual void MoveToTarget()
    {
        if (target != null)
        {
            navMesh.SetDestination(target.transform.position);
        }
    }
    //跟随目标
    protected virtual void FollowTarget()
    {
        //追踪
        MoveToTarget();
        SetMoveAnimator();
        if (target != null)
        {
            try
            {
                //判断路径是否可达
                if (navMesh.pathStatus == NavMeshPathStatus.PathPartial || navMesh.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    LoseTarget();
                    return;
                }
                //判断高度
                //判断高度差
                if (Mathf.Abs(target.transform.position.y - transform.position.y) > maxHeightDiff)
                {
                    LoseTarget();
                    return;
                }
                //判断追踪距离
                if (Vector3.Distance(target.transform.position, transform.position) > followDistance)
                {
                    //目标丢失
                    LoseTarget();
                    return;
                }              
                //判断攻击距离
                if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
                {
                    if (isAttack)
                    { 
                        //攻击
                        Attack();             
                    }
                }
            }
            catch (Exception e)
            {
                //目标丢失
                LoseTarget();
            }
        }
    }
    //改变动画状态
    protected virtual void SetMoveAnimator()
    {
        if (!isAttack)
        {
            navMesh.speed = runSpeed;
        }
        //设置速度
        animator.SetFloat(speedHash, navMesh.velocity.magnitude);


    }

    protected virtual void LoseTarget()
    {
        target = null;
        //回到默认位置
        navMesh.SetDestination(defaultPos);
        //if (navMesh.destination != Vector3.zero)
        //{
        //    navMesh.speed = walkSpeed;
        //}
        //else
        //{
        //    navMesh.speed = 0;
        //}
        animator.SetFloat(speedHash, navMesh.velocity.magnitude);
    }

    private void CheckAttackCD()
    {
        if (!isAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackCD)
            {
                isAttack = true;
                attackTimer = 0;
            }
        }
    }
    protected virtual void Attack()
    {
        
    }
    public virtual void OnDeath()
    {
        animator.SetTrigger(deathHash);
        isDeath = true;
    }
    //protected virtual void OnDrawGizmosSelected()
    //{
    //    //画出检测范围
    //    Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
    //    Gizmos.DrawSphere(transform.position, checkDistance);
    //    //画出追踪范围
    //    Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.4f);
    //    Gizmos.DrawSphere(transform.position, followDistance);
    //    //画出攻击范围
    //    Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.r, 0.4f);
    //    Gizmos.DrawSphere(transform.position, attackDistance);
    //    Gizmos.DrawLine(transform.position, transform.position + Vector3.up * checkDistance);
    //    UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, lookAngle, 3);
    //    UnityEditor.Handles.DrawSolidArc(transform.position, -Vector3.up, transform.forward, lookAngle, 3);
    //}

}
