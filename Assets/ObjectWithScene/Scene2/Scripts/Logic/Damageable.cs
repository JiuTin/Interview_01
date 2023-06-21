using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageMessage
{
    public float damage; //�˺�
    //public Vector3 damagePos; //����λ��
}
[Serializable]
public class DamageEvent : UnityEvent<Damageable, DamageMessage> { }
public class Damageable : MonoBehaviour
{
    public float hp; //��ǰhp
    public float maxHp; //���hp
    public float sp;   //Sp
    public float maxSP;  //���SP
    public float atk; //������
    [Header("�˺�����")]
    public GameObject damageNumber;
    public float invincibleTime = 0;  //�޵�ʱ��
    private bool isInvincible = false; //�޵�״̬
    private float timer; //��ʱ��
    public HpBar hpBar;
    public DamageEvent onHurt;
    public DamageEvent onDeath;
    public DamageEvent onReset;
    private void Start()
    {
        if (hpBar != null)
        { 
            hpBar.SetMaxValue(maxHp);
        }
    }
    private void Update()
    {
        if (isInvincible)
        {
            timer += Time.deltaTime;
            if (timer >= invincibleTime)
            {
                isInvincible = false;
                timer = 0;
            }
        }
    }
    public void OnDamage(DamageMessage data)
    {
        if (hp < 0) { return; }
        if (isInvincible)
        {
            return;
        }
        
        hp -= data.damage;
        isInvincible = true;
        if (hp <= 0)
        {
            if (hpBar != null)
            {
                //����Ѫ��
                hpBar.SetHpValue(hp);
            }
            onDeath?.Invoke(this, data);
        }
        else
        {
            if (hpBar != null)
            { 
                //����Ѫ��
                hpBar.SetHpValue(hp);
            }
            //����
            onHurt?.Invoke(this, data); 
        }
        //�����˺��ı�����ʵ�����˺�����
        if (this.tag == "Enemy")
        { 
            GamePool.Instance.CreateObj("DamageNumber", damageNumber, transform.Find("HpBar/Canvas"));
        }
    }
    public void ResetDamage()
    {
        hp = maxHp;
        isInvincible = false;
        timer = 0;
        onReset?.Invoke(this, null);
    }
}
