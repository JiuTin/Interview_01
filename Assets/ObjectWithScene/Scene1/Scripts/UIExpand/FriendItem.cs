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

    //事件
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
    /// 判断该节点是否需要删除或添加
    /// </summary>
    public void ListenerCorners()
    {
        rect.GetWorldCorners(selfCorners);
        scrolRect.GetWorldCorners(worldCorners);
        if (IsFirst())
        {
            if (selfCorners[1].y < worldCorners[1].y)
            {
                //添加节点
                if (OnAddHead != null)
                {
                    OnAddHead();
                }
            }
            if (selfCorners[0].y > worldCorners[1].y)
            {
                //删除节点
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
                //删除节点
                if (OnRemoveLast!=null)
                {
                    OnRemoveLast();
                }
            }
            if (selfCorners[0].y > worldCorners[0].y)
            {
                //添加节点
                if (OnAddLast != null)
                {
                    OnAddLast();
                }
            }
        }
        

    }
    //判断是不是第一个
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
    //判断是不是最后一个
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
