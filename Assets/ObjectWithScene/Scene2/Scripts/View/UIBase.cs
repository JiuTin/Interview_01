using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    CanvasGroup group;
    protected  virtual void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public virtual void HidePanel()
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
    }
    public virtual void ShowPanel()
    {
        group.alpha = 1;
        group.blocksRaycasts = true;
    }
}
