using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageMessage
{
    public float damage; //伤害
    //public Vector3 damagePos; //攻击位置
}
[Serializable]
public class DamageEvent : UnityEvent<Damageable, DamageMessage> { }
public class Damageable : MonoBehaviour
{
    public float hp; //当前hp
    public float maxHp; //最大hp
    public float sp;   //Sp
    public float maxSP;  //最大SP
    public float atk; //攻击力
    [Header("伤害数字")]
    public GameObject damageNumber;
    public float invincibleTime = 0;  //无敌时间
    private bool isInvincible = false; //无敌状态
    private float timer; //计时器
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
                //设置血条
                hpBar.SetHpValue(hp);
            }
            onDeath?.Invoke(this, data);
        }
        else
        {
            if (hpBar != null)
            { 
                //设置血条
                hpBar.SetHpValue(hp);
            }
            //受伤
            onHurt?.Invoke(this, data); 
        }
        //设置伤害文本，并实例化伤害数字
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
