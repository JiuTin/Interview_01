using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipClothing : EquipGrid
{
    public Text def;
    protected override void Awake()
    {
        base.Awake();
        this.currentType = ArticleType.Clothing;
    }

    //装备和脱下装备，在方法里修改角色属性和模型
    public override void Equip(Article article)
    {
        base.Equip(article);
        def.text = (15 + ((ClothingArticle)article).def).ToString();
    }
}
