using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ArticleType
{ 
    Weapon=0,
    Shoes,
    Clothing,
    Jewellery,
    Hat,
    Drug,
}
public class Article 
{
    public string name;
    public int count;
    public string spritePath;
    public int price;
    public ArticleType articleType;
    public Action<Article> onDataChange;
    public Article article
    {
        get {
            return this;
        }
    }
    public Article()
    { }
    public Article(string name,int count,string spritePath)
    {
        this.name = name;
        this.count = count;
        this.spritePath=spritePath;
    }
    public virtual string GetArticleItemInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("名称:" + this.name).Append("\n");
        sb.Append("类别:" + GetArticleType(this.articleType)).Append("\n");
        sb.Append("数量:" + this.count).Append("\n");
        
        return sb.ToString();
    }

    private string GetArticleType(ArticleType type)
    {
        switch (type)
        {
            case ArticleType.Weapon:
                return "武器";
            case ArticleType.Shoes:
                return "鞋子";
            case ArticleType.Clothing:
                return "衣服";
            case ArticleType.Jewellery:
                return "珠宝";
            case ArticleType.Hat:
                return "帽子";
            case ArticleType.Drug:
                return "药品";
        }
        return "";
    }
    public virtual void UseDataArticle()
    {
        this.count--;
        if (count == 0)
        {
            BagPanel.Instance.RemoveArticle(article);
        }
        if (onDataChange != null)
        {
            onDataChange(this);
        }
    }
}

