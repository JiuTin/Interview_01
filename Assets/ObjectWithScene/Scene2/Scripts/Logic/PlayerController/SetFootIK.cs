using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFootIK : MonoBehaviour
{
    public PlayerController controller;
    Animator animator;               //动画机

    public bool enableFeetIk = true; //是否开启ik
    public bool useIkFeature = false; //是否使用IK旋转
    public bool showSolverDebug = true;// Debug绘制

    [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1.2f; //从地面向上的cast距离
    [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1.5f; //向下cast 距离

    [SerializeField] private LayerMask environmentLayer; //检测layer
    [SerializeField] private float pelvisOffset = 0f; //盆骨offset
    [Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f; //盆骨赋值速度
    [Range(0, 1)] [SerializeField] private float feetToIkPositionSpeed = 0.5f; //足IK赋值速度

    public string leftFootAnimCurveName = "LeftFootWeight"; //权重曲线
    public string rightFootAnimCurveName = "RightFootWeight"; //权重曲线
    [Range(0, 100)] public float leftFootAngleOffset; //旋转偏移
    [Range(0, 100)] public float rightFootAngleOffset; //旋转偏移
    private Vector3 _rightFootPosition, _leftFootPosition; //足部骨骼posiition
    private Vector3 _rightFootIkPosition, _leftFootIkPosition; //足部IK position
    private Quaternion _leftFootIkRotation, _rightFootIkRotation; //足部IK rotation
    private float _lastPelvisPositionY, _lastRightFootPositionY, _lastLeftFootPositionY; //上帧信息，用于lerp动画

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (!enableFeetIk) return;
        if (!animator) return;
        AdjustFeetTarget(ref _rightFootPosition,HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref _leftFootPosition, HumanBodyBones.LeftFoot);

        //IK射线解算
        FootPositionSolver(_rightFootPosition, ref _rightFootIkPosition, ref _rightFootIkRotation, rightFootAngleOffset);
        FootPositionSolver(_leftFootPosition, ref _leftFootIkPosition, ref _leftFootIkRotation, leftFootAngleOffset);
    }
    /// <summary>
    /// 获取足部骨骼的位置信息
    /// </summary>
    /// <param name="feetPosition">保存足部骨骼位置的变量</param>
    /// <param name="foot">人性部分的位置</param>
    private void AdjustFeetTarget(ref Vector3 feetPosition,HumanBodyBones foot)
    {
        feetPosition = animator.GetBoneTransform(foot).position;            //获取人形足部的transform position
        feetPosition.y = transform.position.y + heightFromGroundRaycast;    //y的值加上【向上检测的距离】，主要是防止卡模。
    }

    /// <summary>
    /// 射线检测，存在hit时，记录hiPoint的值。计算Vector3.up与hitPoint法线的夹角
    /// </summary>
    /// <param name="fromskyPosition">根足部信息</param>
    /// <param name="feetIkPosition">脚底位置的IK信息</param>
    /// <param name="feetIkRotation">脚底旋转的IK信息</param>
    /// <param name="angleoffset">旋转偏移量</param>
    private void FootPositionSolver(Vector3 fromskyPosition,ref Vector3 feetIkPosition,ref Quaternion feetIkRotation,float angleoffset)
    {
        if (showSolverDebug)
            Debug.DrawLine(fromskyPosition, fromskyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.green);
        if (Physics.Raycast(fromskyPosition, Vector3.down, out var feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            feetIkPosition = fromskyPosition;               //保存x,z的值
            feetIkPosition.y = feetOutHit.point.y + pelvisOffset;    //hit Pos的Y赋值。添加pelvisOffset防止穿模

            //FromToRotation(Vectpr3 fromDirection,Vector3 toDirection)->创建一个从 fromDirection 旋转到 toDirection 的旋转。再*上自身的rotation设置方向。
            feetIkRotation = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal)*transform.rotation;    //计算法向偏移
            feetIkRotation = Quaternion.AngleAxis(angleoffset, Vector3.up) * feetIkRotation;   //计算额外偏移，四元数相乘,p3=p2*p1-> 在p2旋转后再以局部坐标进行p1的旋转
            return;
        }
        feetIkPosition = Vector3.zero;   //没有hit,归零。
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (!enableFeetIk) return;
        if (!animator) return;
        if (controller.playerPosture == PlayerPosture.Stand || controller.playerPosture == PlayerPosture.Crouch)
        {
            MovePelvisHeight();
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat(rightFootAnimCurveName));
            if (useIkFeature)
            {
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(rightFootAnimCurveName));
            }
            MoveFeetToIkPoint(AvatarIKGoal.RightFoot, _rightFootIkPosition, _rightFootIkRotation, ref _lastRightFootPositionY);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(leftFootAnimCurveName));
            if (useIkFeature)
            {
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(leftFootAnimCurveName));
            }
            MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, _leftFootIkPosition, _leftFootIkRotation, ref _lastLeftFootPositionY);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
        }
    }
    /// <summary>
    /// 盆骨偏移，保证IK能达到的前提，防止出现脚步位置达不到IK的情况。
    /// 调整pelvis，保证IK能到达
    /// </summary>
    private void MovePelvisHeight()
    {
        if (_rightFootIkPosition == Vector3.zero || _leftFootIkPosition == Vector3.zero || _lastPelvisPositionY==0f)
        {
            _lastPelvisPositionY = animator.bodyPosition.y;
            return;
        }
        float loffsetPosition = _leftFootIkPosition.y - transform.position.y;  //左脚IK pos 与当前tranform的高度差
        float roffsetPosition = _rightFootIkPosition.y - transform.position.y; //右脚IK pos 与当前tranform的高度差
        //选择较小值
        //如果是正值，则向上偏移距离较小的
        //如果是负值，则向下偏移距离较大的
        float totaloffest = (loffsetPosition < roffsetPosition) ? loffsetPosition : roffsetPosition;
        Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totaloffest;
        //newPelvisPosition.y = Mathf.Lerp(_lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed); //由于插值动画的原因，从高处跳跃到低处时，会有回弹的效果。
        
        animator.bodyPosition = newPelvisPosition;                //会影响跳跃等移动状态，
        _lastPelvisPositionY = animator.bodyPosition.y;     //记录信息
    }
    /// <summary>
    /// 对位置和旋转信息进行二次处理后赋值给IK Goal.
    /// </summary>
    /// <param name="foot"></param>
    /// <param name="positionIkHolder"></param>
    /// <param name="rotationIkHolder"></param>
    /// <param name="lastFootPositionY"></param>
    void MoveFeetToIkPoint(AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder, ref float lastFootPositionY)
    {
        Vector3 targetIkPosition = animator.GetIKPosition(foot);
        if (positionIkHolder != Vector3.zero)
        {
            targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
            positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

            float yVar = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPositionSpeed);
            targetIkPosition.y += yVar;
            lastFootPositionY = yVar;
            targetIkPosition = transform.TransformPoint(targetIkPosition);
            animator.SetIKRotation(foot, rotationIkHolder);
        }
        animator.SetIKPosition(foot, targetIkPosition);
    }
}
