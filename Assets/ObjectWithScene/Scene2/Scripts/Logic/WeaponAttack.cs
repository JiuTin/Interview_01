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
    //�����б��������ĵ���
    List<GameObject> attackList = new List<GameObject>();
    private void Update()
    {
        CheckEnemy();
    }
    //��������
    public void BeginAttack()
    {
        isAttack = true;
    }
    public void EndAttack()
    {
        isAttack = false;
        attackList.Clear();
    }

    //������
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
    //����˺�
    void CheckDamage(GameObject gameObject)
    {
        if (attackList.Contains(gameObject))
        { return; }
        //�ж������Ƿ���Ա�����
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if (damageable == null)
        { return; }
        //��⵽�Լ�
        //if (gameObject == myself) { return; }
        
        //����
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
