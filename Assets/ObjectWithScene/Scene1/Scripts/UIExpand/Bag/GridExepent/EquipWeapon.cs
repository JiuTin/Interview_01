using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipWeapon : EquipGrid
{
    public Text weapon;
    protected override void Awake()
    {
        base.Awake();
        this.currentType = ArticleType.Weapon;
        
    }

    //װ��������װ�����ڷ������޸Ľ�ɫ���Ժ�ģ��
    public override void Equip(Article article)
    {
        base.Equip(article);
        //�������
        weapon.text = (100+ ((WeaponArticle)article).attack).ToString();
    }
}
