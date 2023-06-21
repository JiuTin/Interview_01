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

    //װ��������װ�����ڷ������޸Ľ�ɫ���Ժ�ģ��
    public override void Equip(Article article)
    {
        base.Equip(article);
        speed.text = (12 + ((ShoesArticle)article).speed).ToString();
    }
}
