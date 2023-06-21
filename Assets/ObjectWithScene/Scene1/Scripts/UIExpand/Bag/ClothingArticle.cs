using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClothingArticle : Article
{
    public float def;
    public ClothingArticle()
    {

    }
    public ClothingArticle(string name, int count, string spritePath, float def) : base(name, count, spritePath)
    {
        this.def = def;
    }
    public override string GetArticleItemInfo()
    {

        string info = base.GetArticleItemInfo();
        StringBuilder sb = new StringBuilder();
        sb.Append(info);
        sb.Append("<color=red>");
        sb.Append("·ÀÓù:").Append(def);
        sb.Append("</color>");
        return sb.ToString();
    }
    public override void UseDataArticle()
    {
        base.UseDataArticle();
    }
}
