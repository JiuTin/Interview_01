using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DamageNumber : MonoBehaviour
{

    Vector3 defaultPos;
    private void Awake()
    {
        defaultPos = transform.localPosition;
    }
    private void OnEnable()
    {
        NumberMove();
    }

    //…Ë÷√∂Øª≠
    void NumberMove()
    {
        GamePool.Instance.RecoverObjDelay(this.gameObject, 1.5f);
        transform.DOLocalMoveY(transform.localPosition.y + 1f, 1.5f);
        transform.DOScale(transform.localScale * 1.2f, 1.5f);
        Invoke("ResetMove", 1.5f);
    }
    void ResetMove()
    {
        transform.localPosition = defaultPos;
    }
}
