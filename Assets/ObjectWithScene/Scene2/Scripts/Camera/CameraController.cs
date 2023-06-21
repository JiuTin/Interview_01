using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [Header("�����Ŀ��")]
    public Transform followTarget;
    Vector3 targetPos;
    Transform cameraTrans;
    float hAngle, vAngle; //ˮƽ�봹ֱ�н�
    [Header("���ƴ�ֱ�Ƕ�")]
    public Vector2 vAngleLimit;
    [Header("���������")]
    public Vector2 mouseSens;
    [Header("ƫ����")]
    public Vector3 offest;
    [Header("����Ŀ��ľ���")]
    public float distance;
    [Header("���������")]
    public Vector2 distanceLimit;
    [Header("��������������ٶ�")]
    public float zoomMaxSpeed;
    public float zoomDecaySpeed;
    float zoomSpeed;
    [Header("��������")]
    public float compensateDistance;
    private float changeDistance;
    Vector3 prepateCameraPos;   //Ԥ���������λ��
    Vector3 colliderPos;        //�ڵ���λ��
    //�����ײ
    bool isWall;
# region �ڲ�����
    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        LockOrOpenMouse();
        SetAngle();
        CameraMoveDistance();
        CheckShelter();
        CameraZoomPos();
    }
    private void LateUpdate()
    {
        CaculateCameraPos(changeDistance);
        
    }
    #endregion
    /// <summary>
    /// ��������������Ƕ�ֵ
    /// </summary>
    float CaculateCameraAngle(float rad)
    {
        return rad * Mathf.PI/180;
    }
    /// <summary>
    /// �������
    /// </summary>
    void LockOrOpenMouse()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    /// <summary>
    /// ����������ĽǶȲ�������һ���ķ�Χ��
    /// </summary>
    void SetAngle()
    {
        hAngle += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens.x;
        vAngle += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens.y;
        vAngle = Mathf.Clamp(vAngle, vAngleLimit.x, vAngleLimit.y);
    }
    /// <summary>
    /// �����������λ�úͳ���
    /// </summary>
    void CaculateCameraPos(float distance)
    {
        targetPos = followTarget.position;
        targetPos.y += distance * Mathf.Sin(CaculateCameraAngle(vAngle));
        float distanceZ = distance * Mathf.Cos(CaculateCameraAngle(vAngle));
        targetPos.x += distanceZ * Mathf.Sin(CaculateCameraAngle(hAngle));
        targetPos.z += distanceZ * Mathf.Cos(CaculateCameraAngle(hAngle));
        cameraTrans.position =targetPos;
        cameraTrans.LookAt(followTarget.position+ offest);
    }
    /// <summary>
    /// �������������Ŀ���λ��
    /// </summary>
    void CameraMoveDistance()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoomSpeed = Input.GetAxis("Mouse ScrollWheel") * zoomMaxSpeed;
        }
        distance += zoomSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, distanceLimit.x, distanceLimit.y);
        zoomSpeed = Mathf.Lerp(zoomSpeed, 0, Time.deltaTime * zoomDecaySpeed);
    }
    /// <summary>
    /// ��������ڵ�סʱ���ƶ�����
    /// </summary>
    void CameraZoomPos()
    {
        //�����ϰ���
        if (isWall)
        {
            changeDistance = Mathf.Lerp(changeDistance,((colliderPos - followTarget.position).magnitude - compensateDistance),0.2f);
            if (changeDistance < 0)
            {
                changeDistance = -0.1f;
            }
        }
        else
        {
            changeDistance = Mathf.Lerp(changeDistance,distance,0.05f);
            
        }
    }
    /// <summary>
    /// ����Ƿ����ڵ�
    /// </summary>
    void CheckShelter()
    {
        RaycastHit ray = new RaycastHit();
        LayerMask mask = (1 << LayerMask.NameToLayer("Ground"));
        if (Physics.Raycast(followTarget.position, cameraTrans.position - followTarget.position, out ray, distance, mask))
        {
            colliderPos = ray.point;
            isWall = true;
        }
        else
        {
            isWall = false;
        }
    }
    /// <summary>
    /// ��ʼ���Ƕ�
    /// </summary>
    void Init()
    {
        hAngle = 90;
        vAngle = 20;
        cameraTrans = transform;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
