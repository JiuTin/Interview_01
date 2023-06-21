using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipHat : EquipGrid
{
    public Text hp;
    protected override void Awake()
    {
        base.Awake();
        this.currentType = ArticleType.Hat;
    }
    //装备和脱下装备，在方法里修改角色属性和模型
    public override void Equip(Article article)
    {
        base.Equip(article);
        hp.text = (120 + ((HatArticle)article).hp).ToString();
    }
}
