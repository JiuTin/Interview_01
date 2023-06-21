using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    private RectTransform rect;
    private RectTransform scrolRect;
    private Vector3[] selfCorners;
    private Vector3[] worldCorners;

    //�¼�
    public Action OnAddHead;
    public Action OnRemoveHead;
    public Action OnAddLast;
    public Action OnRemoveLast;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        scrolRect = transform.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();
        selfCorners = new Vector3[4];
        worldCorners = new Vector3[4];

    }
    private void Update()
    {
        ListenerCorners();
    }

    /// <summary>
    /// �жϸýڵ��Ƿ���Ҫɾ�������
    /// </summary>
    public void ListenerCorners()
    {
        rect.GetWorldCorners(selfCorners);
        scrolRect.GetWorldCorners(worldCorners);
        if (IsFirst())
        {
            if (selfCorners[1].y < worldCorners[1].y)
            {
                //��ӽڵ�
                if (OnAddHead != null)
                {
                    OnAddHead();
                }
            }
            if (selfCorners[0].y > worldCorners[1].y)
            {
                //ɾ���ڵ�
                if (OnRemoveHead != null)
                {
                    OnRemoveHead();
                }
            }
        }
        if (IsLast())
        {
            if (selfCorners[1].y < worldCorners[0].y)
            {
                //ɾ���ڵ�
                if (OnRemoveLast!=null)
                {
                    OnRemoveLast();
                }
            }
            if (selfCorners[0].y > worldCorners[0].y)
            {
                //��ӽڵ�
                if (OnAddLast != null)
                {
                    OnAddLast();
                }
            }
        }
        

    }
    //�ж��ǲ��ǵ�һ��
    public bool IsFirst()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }
    //�ж��ǲ������һ��
    public bool IsLast()
    {
        for (int i = transform.parent.childCount-1; i >=0; i--)
        {
            if (transform.parent.GetChild(i).gameObject.activeSelf)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }
}
