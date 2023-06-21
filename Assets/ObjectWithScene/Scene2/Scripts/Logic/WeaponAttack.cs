using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CheckPoint
{
    public Transform point;
    public float radius;
    public Color color;
}
public class WeaponAttack : MonoBehaviour
{
    public CheckPoint[] checkPoint;
    DamageMessage data = new DamageMessage();
    public int damage = 1;
    public LayerMask mask;
    private RaycastHit[] hits = new RaycastHit[5];
    //public GameObject myself;
    public bool isAttack=false;
    //缓存列表，攻击过的敌人
    List<GameObject> attackList = new List<GameObject>();
    private void Update()
    {
        CheckEnemy();
    }
    //攻击设置
    public void BeginAttack()
    {
        isAttack = true;
    }
    public void EndAttack()
    {
        isAttack = false;
        attackList.Clear();
    }

    //检测敌人
    void CheckEnemy()
    {
        if (!isAttack) { return; }
        for (int i = 0; i < checkPoint.Length; i++)
        {
            int count = Physics.SphereCastNonAlloc(checkPoint[i].point.position, checkPoint[i].radius, transform.forward, hits, 0.1f, mask);
            for (int j = 0; j < count; j++)
            {
                CheckDamage(hits[i].transform.gameObject);
            }
        }
    }
    //造成伤害
    void CheckDamage(GameObject gameObject)
    {
        if (attackList.Contains(gameObject))
        { return; }
        //判断物体是否可以被攻击
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if (damageable == null)
        { return; }
        //检测到自己
        //if (gameObject == myself) { return; }
        
        //进攻
        data.damage = damage;
       // data.damagePos = transform.position;
        damageable.OnDamage(data);
        attackList.Add(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < checkPoint.Length; i++)
        {
            Gizmos.DrawSphere(checkPoint[i].point.position, checkPoint[i].radius);
        }
    }
}
