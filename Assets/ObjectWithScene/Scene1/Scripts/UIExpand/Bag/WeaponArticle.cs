using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WeaponArticle : Article
{
    public float attack;
    public WeaponArticle()
    { 
        
    }
    public WeaponArticle(string name, int count, string spritePath,float attack) :base(name,count,spritePath)
    {
        this.attack = attack;
    }
    public override string GetArticleItemInfo()
    {

        string info = base.GetArticleItemInfo();
        StringBuilder sb = new StringBuilder();
        sb.Append(info);
        sb.Append("<color=red>");
        sb.Append("¹¥»÷:").Append(attack);
        sb.Append("</color>");
        return sb.ToString();
    }
    public override void UseDataArticle()
    {
        base.UseDataArticle();
    }
}
