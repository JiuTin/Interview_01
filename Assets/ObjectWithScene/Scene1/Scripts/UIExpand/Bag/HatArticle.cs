using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HatArticle : Article
{
    public float hp;
    public HatArticle()
    {

    }
    public HatArticle(string name, int count, string spritePath, float hp) : base(name, count, spritePath)
    {
        this.hp = hp;
    }
    public override string GetArticleItemInfo()
    {

        string info = base.GetArticleItemInfo();
        StringBuilder sb = new StringBuilder();
        sb.Append(info);
        sb.Append("<color=red>");
        sb.Append("ÉúÃü:").Append(hp);
        sb.Append("</color>");
        return sb.ToString();
    }
    public override void UseDataArticle()
    {
        base.UseDataArticle();
    }
}
