using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFootIK : MonoBehaviour
{
    public PlayerController controller;
    Animator animator;               //������

    public bool enableFeetIk = true; //�Ƿ���ik
    public bool useIkFeature = false; //�Ƿ�ʹ��IK��ת
    public bool showSolverDebug = true;// Debug����

    [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1.2f; //�ӵ������ϵ�cast����
    [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1.5f; //����cast ����

    [SerializeField] private LayerMask environmentLayer; //���layer
    [SerializeField] private float pelvisOffset = 0f; //���offset
    [Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f; //��Ǹ�ֵ�ٶ�
    [Range(0, 1)] [SerializeField] private float feetToIkPositionSpeed = 0.5f; //��IK��ֵ�ٶ�

    public string leftFootAnimCurveName = "LeftFootWeight"; //Ȩ������
    public string rightFootAnimCurveName = "RightFootWeight"; //Ȩ������
    [Range(0, 100)] public float leftFootAngleOffset; //��תƫ��
    [Range(0, 100)] public float rightFootAngleOffset; //��תƫ��
    private Vector3 _rightFootPosition, _leftFootPosition; //�㲿����posiition
    private Vector3 _rightFootIkPosition, _leftFootIkPosition; //�㲿IK position
    private Quaternion _leftFootIkRotation, _rightFootIkRotation; //�㲿IK rotation
    private float _lastPelvisPositionY, _lastRightFootPositionY, _lastLeftFootPositionY; //��֡��Ϣ������lerp����

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

        //IK���߽���
        FootPositionSolver(_rightFootPosition, ref _rightFootIkPosition, ref _rightFootIkRotation, rightFootAngleOffset);
        FootPositionSolver(_leftFootPosition, ref _leftFootIkPosition, ref _leftFootIkRotation, leftFootAngleOffset);
    }
    /// <summary>
    /// ��ȡ�㲿������λ����Ϣ
    /// </summary>
    /// <param name="feetPosition">�����㲿����λ�õı���</param>
    /// <param name="foot">���Բ��ֵ�λ��</param>
    private void AdjustFeetTarget(ref Vector3 feetPosition,HumanBodyBones foot)
    {
        feetPosition = animator.GetBoneTransform(foot).position;            //��ȡ�����㲿��transform position
        feetPosition.y = transform.position.y + heightFromGroundRaycast;    //y��ֵ���ϡ����ϼ��ľ��롿����Ҫ�Ƿ�ֹ��ģ��
    }

    /// <summary>
    /// ���߼�⣬����hitʱ����¼hiPoint��ֵ������Vector3.up��hitPoint���ߵļн�
    /// </summary>
    /// <param name="fromskyPosition">���㲿��Ϣ</param>
    /// <param name="feetIkPosition">�ŵ�λ�õ�IK��Ϣ</param>
    /// <param name="feetIkRotation">�ŵ���ת��IK��Ϣ</param>
    /// <param name="angleoffset">��תƫ����</param>
    private void FootPositionSolver(Vector3 fromskyPosition,ref Vector3 feetIkPosition,ref Quaternion feetIkRotation,float angleoffset)
    {
        if (showSolverDebug)
            Debug.DrawLine(fromskyPosition, fromskyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.green);
        if (Physics.Raycast(fromskyPosition, Vector3.down, out var feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            feetIkPosition = fromskyPosition;               //����x,z��ֵ
            feetIkPosition.y = feetOutHit.point.y + pelvisOffset;    //hit Pos��Y��ֵ�����pelvisOffset��ֹ��ģ

            //FromToRotation(Vectpr3 fromDirection,Vector3 toDirection)->����һ���� fromDirection ��ת�� toDirection ����ת����*�������rotation���÷���
            feetIkRotation = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal)*transform.rotation;    //���㷨��ƫ��
            feetIkRotation = Quaternion.AngleAxis(angleoffset, Vector3.up) * feetIkRotation;   //�������ƫ�ƣ���Ԫ�����,p3=p2*p1-> ��p2��ת�����Ծֲ��������p1����ת
            return;
        }
        feetIkPosition = Vector3.zero;   //û��hit,���㡣
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
    /// ���ƫ�ƣ���֤IK�ܴﵽ��ǰ�ᣬ��ֹ���ֽŲ�λ�ôﲻ��IK�������
    /// ����pelvis����֤IK�ܵ���
    /// </summary>
    private void MovePelvisHeight()
    {
        if (_rightFootIkPosition == Vector3.zero || _leftFootIkPosition == Vector3.zero || _lastPelvisPositionY==0f)
        {
            _lastPelvisPositionY = animator.bodyPosition.y;
            return;
        }
        float loffsetPosition = _leftFootIkPosition.y - transform.position.y;  //���IK pos �뵱ǰtranform�ĸ߶Ȳ�
        float roffsetPosition = _rightFootIkPosition.y - transform.position.y; //�ҽ�IK pos �뵱ǰtranform�ĸ߶Ȳ�
        //ѡ���Сֵ
        //�������ֵ��������ƫ�ƾ����С��
        //����Ǹ�ֵ��������ƫ�ƾ���ϴ��
        float totaloffest = (loffsetPosition < roffsetPosition) ? loffsetPosition : roffsetPosition;
        Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totaloffest;
        //newPelvisPosition.y = Mathf.Lerp(_lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed); //���ڲ�ֵ������ԭ�򣬴Ӹߴ���Ծ���ʹ�ʱ�����лص���Ч����
        
        animator.bodyPosition = newPelvisPosition;                //��Ӱ����Ծ���ƶ�״̬��
        _lastPelvisPositionY = animator.bodyPosition.y;     //��¼��Ϣ
    }
    /// <summary>
    /// ��λ�ú���ת��Ϣ���ж��δ����ֵ��IK Goal.
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
