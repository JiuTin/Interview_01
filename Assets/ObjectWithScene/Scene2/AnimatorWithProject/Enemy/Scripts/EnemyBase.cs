using System;

using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{

    public float checkDistance;    //�쳵����
    public float maxHeightDiff;    //���߶�
    [Range(0, 180)]
    public float lookAngle;           //��Ұ�Ƕ�
    protected RaycastHit[] hits = new RaycastHit[10];    //���Ŀ��
    public LayerMask mask;          //���Ĳ㼶
    public GameObject target;       //Ŀ��
    protected NavMeshAgent navMesh;
    public float followDistance;  //׷�ٵľ���
    public float attackDistance;  //�����ľ���
    Vector3 defaultPos;           //��ʼλ��

    //�ٶ�
    protected float runSpeed = 3.5f;
    protected float walkSpeed = 1f;
    protected bool isAttack = false;
    public  float attackCD;
    private float attackTimer;
    protected Animator animator;
    protected Rigidbody rig;
    bool isDeath=false;

    //��ϣֵ
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
    /// ����Ŀ��
    /// </summary>
    protected virtual void CheckTarget()
    {
        int count = Physics.SphereCastNonAlloc(transform.position, checkDistance, Vector3.forward, hits, 0.1f, mask);
        for (int i = 0; i < count; i++)
        {
            //�ж��ǲ��ǿ��Թ�������Ϸ����
            if (!hits[i].transform.GetComponent<Damageable>())
            { continue; }
            //�ǲ�������Ұ��Χ��
            if (Vector3.Angle(transform.forward, hits[i].transform.position - transform.position) > lookAngle)
            {
                continue;
            }
            //�ҵ�Ŀ��(���Լ����������Ŀ��)
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
    //�ƶ���Ŀ��
    protected virtual void MoveToTarget()
    {
        if (target != null)
        {
            navMesh.SetDestination(target.transform.position);
        }
    }
    //����Ŀ��
    protected virtual void FollowTarget()
    {
        //׷��
        MoveToTarget();
        SetMoveAnimator();
        if (target != null)
        {
            try
            {
                //�ж�·���Ƿ�ɴ�
                if (navMesh.pathStatus == NavMeshPathStatus.PathPartial || navMesh.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    LoseTarget();
                    return;
                }
                //�жϸ߶�
                //�жϸ߶Ȳ�
                if (Mathf.Abs(target.transform.position.y - transform.position.y) > maxHeightDiff)
                {
                    LoseTarget();
                    return;
                }
                //�ж�׷�پ���
                if (Vector3.Distance(target.transform.position, transform.position) > followDistance)
                {
                    //Ŀ�궪ʧ
                    LoseTarget();
                    return;
                }              
                //�жϹ�������
                if (Vector3.Distance(target.transform.position, transform.position) <= attackDistance)
                {
                    if (isAttack)
                    { 
                        //����
                        Attack();             
                    }
                }
            }
            catch (Exception e)
            {
                //Ŀ�궪ʧ
                LoseTarget();
            }
        }
    }
    //�ı䶯��״̬
    protected virtual void SetMoveAnimator()
    {
        if (!isAttack)
        {
            navMesh.speed = runSpeed;
        }
        //�����ٶ�
        animator.SetFloat(speedHash, navMesh.velocity.magnitude);


    }

    protected virtual void LoseTarget()
    {
        target = null;
        //�ص�Ĭ��λ��
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
    //    //������ⷶΧ
    //    Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
    //    Gizmos.DrawSphere(transform.position, checkDistance);
    //    //����׷�ٷ�Χ
    //    Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.4f);
    //    Gizmos.DrawSphere(transform.position, followDistance);
    //    //����������Χ
    //    Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.r, 0.4f);
    //    Gizmos.DrawSphere(transform.position, attackDistance);
    //    Gizmos.DrawLine(transform.position, transform.position + Vector3.up * checkDistance);
    //    UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, lookAngle, 3);
    //    UnityEditor.Handles.DrawSolidArc(transform.position, -Vector3.up, transform.forward, lookAngle, 3);
    //}

}
