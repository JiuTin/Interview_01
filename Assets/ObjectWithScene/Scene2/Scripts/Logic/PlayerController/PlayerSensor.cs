using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NextPlayerMovement
{ 
    jump,
    climbLow,
    climbHigh,
    vault
}
public class PlayerSensor : MonoBehaviour
{
    public NextPlayerMovement nextMovement = NextPlayerMovement.jump;
    public float lowClimpHeight = 0.5f;    //低墙的高度
    public Vector3 climbHitNormal;      //墙体的法线
    public float checkDistance = 1.5f;        //检测的距离
    float climbDistance;                        //角色距离墙壁的距离
    float climbAngle = 45f;                   //检测的角度
    public float bodyHeight = 1f;        //射线向上检测的距离
    public float hightClimbHeight=1.6f;     //高位攀爬的最低高度
    public Vector3 ledge;
    public LayerMask climbLayer;             //可以攀爬的标签
    #region 推动相关的参数
    public LayerMask moveableLayer;          //可以移动的标签
    float moveableObjectHeight=0.8f;              //可以推动箱子的高度
    #endregion
    private void Start()
    {
        climbDistance = Mathf.Cos(climbAngle) * checkDistance;
    }
    /// <summary>
    /// 环境检测后，角色将执行的下一个执行动画事件
    /// </summary>
    /// <param name="playerTransform"></param>
    /// <param name="inputDirection"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public NextPlayerMovement ClimbDetect(Transform playerTransform, Vector3 inputDirection, float offset)
    {
        //从角色脚步起0.5f的地方发出射线检测是否有墙体
        if (Physics.Raycast(playerTransform.position + Vector3.up * lowClimpHeight, playerTransform.forward, out RaycastHit hit, climbDistance, climbLayer))
        {
            climbHitNormal = hit.normal;
            //角度不符合
            if (Vector3.Angle(-climbHitNormal, inputDirection) > climbAngle || Vector3.Angle(-climbHitNormal, playerTransform.forward)>climbAngle)
            {
                return NextPlayerMovement.jump;
            }         
            //判断climbDistance的距离内是否可以攀爬（0.5f的高度）
            if (Physics.Raycast(playerTransform.position+Vector3.up * lowClimpHeight, -climbHitNormal, out RaycastHit firstHit, climbDistance + offset, climbLayer))
            {                
                //1.5f的高度
                if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimpHeight + bodyHeight), -climbHitNormal, out RaycastHit secondHit, climbDistance + offset,climbLayer))
                {
                    //2.5f的高度
                    if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimpHeight + 2 * bodyHeight), -climbHitNormal, out RaycastHit thirdHit, climbDistance,climbLayer))
                    {
                        //3.5f的高度
                        if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimpHeight + 3 * bodyHeight), -climbHitNormal, climbDistance,climbLayer))
                        {
                            return NextPlayerMovement.jump;
                        }
                        //2.5~3.5之间
                        else if (Physics.Raycast(thirdHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight,climbLayer))
                        {
                            ledge = ledgeHit.point;
                            return NextPlayerMovement.climbHigh;
                        }
                    }
                    //1.5~2.5之间，得判断是否高低位得攀爬
                    else if (Physics.Raycast(thirdHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight,climbLayer))
                    {                        
                        ledge = ledgeHit.point;
                        if (ledge.y - playerTransform.position.y > hightClimbHeight)
                        {
                            return NextPlayerMovement.climbHigh;
                        }
                        //在向下检测到的point往前移动0.2f的距离，如果还能检测到，则低位攀爬，否则翻越
                        else if (Physics.Raycast(secondHit.point + Vector3.up * bodyHeight - climbHitNormal * 0.2f, Vector3.down, bodyHeight,climbLayer))
                        {
                            return NextPlayerMovement.climbLow;
                        }
                        else
                        {
                            Debug.Log("f翻越");
                            return NextPlayerMovement.vault;
                        }
                    }
                }
                //0.5~1.5之间
                else if (Physics.Raycast(firstHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight, climbLayer))
                {
                    ledge = ledgeHit.point;
                    if (Physics.Raycast(firstHit.point +Vector3.up- climbHitNormal * 0.2f, Vector3.down, bodyHeight, climbLayer))
                    {
                        return NextPlayerMovement.climbLow;
                    }
                    else
                    {
                        return NextPlayerMovement.vault;
                    }
                }
            }
        }
        return NextPlayerMovement.jump;
    }
    /// <summary>
    /// 检测可以移动的物体
    /// </summary>
    /// <returns></returns>
    public MoveableObject MoveableObjectCheck(Transform playerTransform, Vector3 inputDirection)
    {
        if (Physics.Raycast(playerTransform.position + Vector3.up * moveableObjectHeight, playerTransform.forward, out RaycastHit hit, checkDistance,moveableLayer))
        {
            climbHitNormal = hit.normal;
            if (Vector3.Angle(playerTransform.forward,-climbHitNormal) > 45f || Vector3.Angle(inputDirection,-climbHitNormal) > 45f)
            {
                return null;
            }
            MoveableObject moveableObject;
            if (hit.collider.TryGetComponent<MoveableObject>(out moveableObject))
            {
                return moveableObject;
            }
        }
        return null;
    }
}
