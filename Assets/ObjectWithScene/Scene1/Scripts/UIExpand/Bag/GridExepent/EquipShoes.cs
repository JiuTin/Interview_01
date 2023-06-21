using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipShoes : EquipGrid
{
    public Text speed;
    protected override void Awake()
    {
        base.Awake();
        this.currentType = ArticleType.Shoes;
    }

    //装备和脱下装备，在方法里修改角色属性和模型
    public override void Equip(Article article)
    {
        base.Equip(article);
        speed.text = (12 + ((ShoesArticle)article).speed).ToString();
    }
}
