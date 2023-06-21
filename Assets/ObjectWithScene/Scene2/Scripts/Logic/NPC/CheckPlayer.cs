using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    [Header("交互范围")]
    public float checkRange;
    public LayerMask mask;
    public GameObject dialogueWithKeyF;
    RaycastHit[] hit =new RaycastHit[1];
    public Dialogue dialouge;

    [Header("对话内容"),TextArea(1, 3)]
    public string[] dialogueContent;
    //对话内容
    private void Update()
    {
        CheckTheObject();
    }

    /// <summary>
    /// 检查进入范围的对象是否为玩家
    /// </summary>
    private void CheckTheObject()
    {
        //检测
        if (Physics.SphereCastNonAlloc(transform.position,checkRange,transform.forward,hit,0.1f,mask)>0)
        {
            //显示交互框
            if (dialogueWithKeyF != null)
            { 
                dialogueWithKeyF.SetActive(true);
            }
            //按下F键，激活对话,并设置相机的位置
            if (Input.GetKeyDown(KeyCode.F) &&dialogueWithKeyF!=null)
            {
                dialogueWithKeyF.SetActive(false);
                dialogueWithKeyF = null;
                //激活对话面板
                dialouge.ShowPanel();
                //并设置对话
                dialouge.ShowDialogue(dialogueContent);
                //停止角色的控制
                hit[0].transform.GetComponent<PlayerController>().isIdle=true;
            }
        }
        else
        {
            if (dialogueWithKeyF != null)
            { 
                dialogueWithKeyF.SetActive(false);
            }
        }
    }

    //在编辑器下画出检测范围

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, checkRange);
    //}
}
