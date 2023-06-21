using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel :MonoBehaviour
{
    CanvasGroup canvasGroup;
    public virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void OnEnter()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public virtual void OnPause()
    {

        canvasGroup.blocksRaycasts = false;
    }
    public virtual void OnRecover()
    {
        canvasGroup.blocksRaycasts = true;
    }
    public virtual void OnExit()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
    public virtual void OnClosePanel()
    {
        UIManager.Instance.PopPanel();
    }
    public bool IsHide()
    {
        if (canvasGroup.alpha == 0)
        {
            return true;
        }
        return false;
    }
}
