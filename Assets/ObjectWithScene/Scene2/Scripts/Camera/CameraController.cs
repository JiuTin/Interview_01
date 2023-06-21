using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [Header("跟随的目标")]
    public Transform followTarget;
    Vector3 targetPos;
    Transform cameraTrans;
    float hAngle, vAngle; //水平与垂直夹角
    [Header("限制垂直角度")]
    public Vector2 vAngleLimit;
    [Header("鼠标灵敏度")]
    public Vector2 mouseSens;
    [Header("偏移量")]
    public Vector3 offest;
    [Header("距离目标的距离")]
    public float distance;
    [Header("距离的限制")]
    public Vector2 distanceLimit;
    [Header("滚轮拉近距离的速度")]
    public float zoomMaxSpeed;
    public float zoomDecaySpeed;
    float zoomSpeed;
    [Header("补偿距离")]
    public float compensateDistance;
    private float changeDistance;
    Vector3 prepateCameraPos;   //预测摄像机的位置
    Vector3 colliderPos;        //遮挡的位置
    //检测碰撞
    bool isWall;
# region 内部函数
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
    /// 辅助函数，计算角度值
    /// </summary>
    float CaculateCameraAngle(float rad)
    {
        return rad * Mathf.PI/180;
    }
    /// <summary>
    /// 锁定鼠标
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
    /// 设置摄像机的角度并限制在一定的范围内
    /// </summary>
    void SetAngle()
    {
        hAngle += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSens.x;
        vAngle += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSens.y;
        vAngle = Mathf.Clamp(vAngle, vAngleLimit.x, vAngleLimit.y);
    }
    /// <summary>
    /// 计算摄像机的位置和朝向
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
    /// 计算摄像机距离目标的位置
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
    /// 摄像机被遮挡住时的移动补偿
    /// </summary>
    void CameraZoomPos()
    {
        //发现障碍物
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
    /// 检测是否有遮挡
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
    /// 初始化角度
    /// </summary>
    void Init()
    {
        hAngle = 90;
        vAngle = 20;
        cameraTrans = transform;
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
