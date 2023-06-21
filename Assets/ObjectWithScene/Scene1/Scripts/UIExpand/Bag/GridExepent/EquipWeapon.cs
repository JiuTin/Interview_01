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

    //装备和脱下装备，在方法里修改角色属性和模型
    public override void Equip(Article article)
    {
        base.Equip(article);
        //添加属性
        weapon.text = (100+ ((WeaponArticle)article).attack).ToString();
    }
}
