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
    //װ��������װ�����ڷ������޸Ľ�ɫ���Ժ�ģ��
    public override void Equip(Article article)
    {
        base.Equip(article);
        hp.text = (120 + ((HatArticle)article).hp).ToString();
    }
}
