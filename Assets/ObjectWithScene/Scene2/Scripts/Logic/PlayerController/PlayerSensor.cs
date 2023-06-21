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
    public float lowClimpHeight = 0.5f;    //��ǽ�ĸ߶�
    public Vector3 climbHitNormal;      //ǽ��ķ���
    public float checkDistance = 1.5f;        //���ľ���
    float climbDistance;                        //��ɫ����ǽ�ڵľ���
    float climbAngle = 45f;                   //���ĽǶ�
    public float bodyHeight = 1f;        //�������ϼ��ľ���
    public float hightClimbHeight=1.6f;     //��λ��������͸߶�
    public Vector3 ledge;
    public LayerMask climbLayer;             //���������ı�ǩ
    #region �ƶ���صĲ���
    public LayerMask moveableLayer;          //�����ƶ��ı�ǩ
    float moveableObjectHeight=0.8f;              //�����ƶ����ӵĸ߶�
    #endregion
    private void Start()
    {
        climbDistance = Mathf.Cos(climbAngle) * checkDistance;
    }
    /// <summary>
    /// �������󣬽�ɫ��ִ�е���һ��ִ�ж����¼�
    /// </summary>
    /// <param name="playerTransform"></param>
    /// <param name="inputDirection"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public NextPlayerMovement ClimbDetect(Transform playerTransform, Vector3 inputDirection, float offset)
    {
        //�ӽ�ɫ�Ų���0.5f�ĵط��������߼���Ƿ���ǽ��
        if (Physics.Raycast(playerTransform.position + Vector3.up * lowClimpHeight, playerTransform.forward, out RaycastHit hit, climbDistance, climbLayer))
        {
            climbHitNormal = hit.normal;
            //�ǶȲ�����
            if (Vector3.Angle(-climbHitNormal, inputDirection) > climbAngle || Vector3.Angle(-climbHitNormal, playerTransform.forward)>climbAngle)
            {
                return NextPlayerMovement.jump;
            }         
            //�ж�climbDistance�ľ������Ƿ����������0.5f�ĸ߶ȣ�
            if (Physics.Raycast(playerTransform.position+Vector3.up * lowClimpHeight, -climbHitNormal, out RaycastHit firstHit, climbDistance + offset, climbLayer))
            {                
                //1.5f�ĸ߶�
                if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimpHeight + bodyHeight), -climbHitNormal, out RaycastHit secondHit, climbDistance + offset,climbLayer))
                {
                    //2.5f�ĸ߶�
                    if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimpHeight + 2 * bodyHeight), -climbHitNormal, out RaycastHit thirdHit, climbDistance,climbLayer))
                    {
                        //3.5f�ĸ߶�
                        if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimpHeight + 3 * bodyHeight), -climbHitNormal, climbDistance,climbLayer))
                        {
                            return NextPlayerMovement.jump;
                        }
                        //2.5~3.5֮��
                        else if (Physics.Raycast(thirdHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight,climbLayer))
                        {
                            ledge = ledgeHit.point;
                            return NextPlayerMovement.climbHigh;
                        }
                    }
                    //1.5~2.5֮�䣬���ж��Ƿ�ߵ�λ������
                    else if (Physics.Raycast(thirdHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight,climbLayer))
                    {                        
                        ledge = ledgeHit.point;
                        if (ledge.y - playerTransform.position.y > hightClimbHeight)
                        {
                            return NextPlayerMovement.climbHigh;
                        }
                        //�����¼�⵽��point��ǰ�ƶ�0.2f�ľ��룬������ܼ�⵽�����λ����������Խ
                        else if (Physics.Raycast(secondHit.point + Vector3.up * bodyHeight - climbHitNormal * 0.2f, Vector3.down, bodyHeight,climbLayer))
                        {
                            return NextPlayerMovement.climbLow;
                        }
                        else
                        {
                            Debug.Log("f��Խ");
                            return NextPlayerMovement.vault;
                        }
                    }
                }
                //0.5~1.5֮��
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
    /// �������ƶ�������
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
