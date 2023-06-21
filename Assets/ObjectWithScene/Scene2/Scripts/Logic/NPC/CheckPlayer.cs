using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    [Header("������Χ")]
    public float checkRange;
    public LayerMask mask;
    public GameObject dialogueWithKeyF;
    RaycastHit[] hit =new RaycastHit[1];
    public Dialogue dialouge;

    [Header("�Ի�����"),TextArea(1, 3)]
    public string[] dialogueContent;
    //�Ի�����
    private void Update()
    {
        CheckTheObject();
    }

    /// <summary>
    /// �����뷶Χ�Ķ����Ƿ�Ϊ���
    /// </summary>
    private void CheckTheObject()
    {
        //���
        if (Physics.SphereCastNonAlloc(transform.position,checkRange,transform.forward,hit,0.1f,mask)>0)
        {
            //��ʾ������
            if (dialogueWithKeyF != null)
            { 
                dialogueWithKeyF.SetActive(true);
            }
            //����F��������Ի�,�����������λ��
            if (Input.GetKeyDown(KeyCode.F) &&dialogueWithKeyF!=null)
            {
                dialogueWithKeyF.SetActive(false);
                dialogueWithKeyF = null;
                //����Ի����
                dialouge.ShowPanel();
                //�����öԻ�
                dialouge.ShowDialogue(dialogueContent);
                //ֹͣ��ɫ�Ŀ���
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

    //�ڱ༭���»�����ⷶΧ

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, checkRange);
    //}
}
