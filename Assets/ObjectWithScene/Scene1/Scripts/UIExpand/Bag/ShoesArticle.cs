using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShoesArticle : Article
{
    public float speed;
    public ShoesArticle()
    {

    }
    public ShoesArticle(string name, int count, string spritePath, float speed) : base(name, count, spritePath)
    {
        this.speed = speed;
    }
    public override string GetArticleItemInfo()
    {

        string info = base.GetArticleItemInfo();
        StringBuilder sb = new StringBuilder();
        sb.Append(info);
        sb.Append("<color=red>");
        sb.Append("ËÙ¶È:").Append(speed);
        sb.Append("</color>");
        return sb.ToString();
    }
    public override void UseDataArticle()
    {
        base.UseDataArticle();
    }
}
