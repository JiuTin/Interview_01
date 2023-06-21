using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,ICanvasRaycastFilter
{

    public Action onBegin;
    public Action onDrag;
    public Action onEnd;
    private RectTransform rect;
    private Vector3 mousePosition;

    private bool isDraging = false;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (rect == null)
        {
            throw new SystemException("没有RectTransform组件");
        }
    }
    private void Update()
    {
        if (isDraging)
        {
            rect.transform.position += (Input.mousePosition - mousePosition);
            mousePosition = Input.mousePosition;
            if (onDrag != null)
            {
                onDrag();
            }
        }
        if (Input.GetMouseButtonUp(0) && isDraging)
        {
            if (onEnd != null)
            {
                onEnd();
            }
            isDraging = false;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePosition = Input.mousePosition;
        isDraging = true;
        if (onBegin != null)
        {
            onBegin();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return !isDraging;
    }
}
