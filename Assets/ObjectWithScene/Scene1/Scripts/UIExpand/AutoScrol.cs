using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AutoScrol : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    //��ʼ���������
    private RectTransform content;
    protected ScrollRect rect;
    Toggle[] toggleGroup;
    //������صĲ���
    protected float[] horizontalInterval;
    protected int pageCount;
    protected int currentIndex;
    public bool isAutoScrol;
    bool isMoving;
    float startPos;
    //��ʱ��
    float autoTimer = 0f;
    float autoTime = 3f;
    float timer=0f;
    //�Զ����¼�
    private event Action<int> moveEvent;
    private void Awake()
    {
        rect = transform.GetComponent<ScrollRect>();
        content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        pageCount = content.childCount;
        currentIndex = 0;
        isAutoScrol = true;
        var item = transform.Find("ToggleGroup");
        if (item != null)
        {
            toggleGroup = item.GetComponentsInChildren<Toggle>();
        }
        else
        {
            toggleGroup = null;
        }
    }
    protected virtual void Start()
    {
        horizontalInterval = new float[pageCount];
        if (pageCount == 1)
        {
            throw new System.Exception("����ֻ��һ��");
        }
        for (int i = 0; i < pageCount; i++)
        {
            horizontalInterval[i] = i*( 1.0f/(float)( pageCount - 1));
        }
        if (toggleGroup != null)
        { 
            foreach (var item in toggleGroup)
            {
                moveEvent += (int index) => {
                    if (int.Parse(item.name) == index)
                    {
                        item.isOn = true;
                    }
                    else
                    {
                        item.isOn = false;
                    }
                };
            }
        }
    }
    protected virtual void Update()
    {
        ListenerAuto();
        ListenerMove();
    }

    protected void ListenerAuto()
    {
        if (!isAutoScrol)
        {
            return;
        }
        else
        {
            autoTimer += Time.deltaTime;
            if (autoTimer > autoTime)
            {
                autoTimer = 0;
                currentIndex = currentIndex == (pageCount-1) ? 0 : currentIndex + 1;
                ScrolToIndex(currentIndex);
            }
        }
    }
    protected void ListenerMove()
    {
        if (isMoving)
        {
            timer += Time.deltaTime*2;
            rect.horizontalNormalizedPosition = Mathf.Lerp(startPos, horizontalInterval[currentIndex], timer);
            if (timer > 1)
            {
                isMoving = false;
            }
            //�ƶ��¼�����ʱ��ִ���¼�
            moveEvent?.Invoke(currentIndex);
        }
    }
    protected void ScrolToIndex(int index)
    {
        isMoving = true;
        startPos = rect.horizontalNormalizedPosition;
        currentIndex = index;
        timer = 0f;
    }


    //��ק�¼�
    public void OnBeginDrag(PointerEventData eventData)
    {
        isAutoScrol = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int minIndex = 0;
        for (int i = 1; i < pageCount; i++)
        {
            //�ҵ��ƶ�������,�����ҳ������
            if (Mathf.Abs(horizontalInterval[i] - rect.horizontalNormalizedPosition) < Mathf.Abs(horizontalInterval[minIndex] - rect.horizontalNormalizedPosition))
            {
                minIndex = i;  
            }
        }
        ScrolToIndex(minIndex);
        isAutoScrol = true;
        autoTimer = 0;
    }
}
