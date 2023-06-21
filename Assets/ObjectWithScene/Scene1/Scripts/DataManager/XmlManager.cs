using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XmlManager 
{
    public string filePath;
    private static XmlManager _instance;
    public static XmlManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new XmlManager();
            }
            return _instance;
        }

    }
    public XmlManager()
    {

        filePath = Application.dataPath + "/Resources/Xml/xmlTest.xml";
    }
    public void CreateXml()
    {
        
    }
    //为某个节点添加信息
    public void AddChild(string parentNode)
    { }
    //删除某个节点信息
    public void RemoveChild(string parentNode, string name)
    { }
    //更新某个节点信息
    public void UpdateChild(string parentNode, string name)
    { }
    //读取某个节点信息
    public List<Friend> ReadChild() 
    {
        if (File.Exists(filePath))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            List<Friend> datas = new List<Friend>();
            XmlNodeList nodeList = xml.SelectSingleNode("Friends").ChildNodes;
            foreach (XmlElement item in nodeList)
            {
                Friend friend = new Friend();
                friend.spirte = item.GetAttribute("picture");
                friend.name = item.GetAttribute("name");
                friend.id = item.GetAttribute("id");
                datas.Add(friend);
            }
            nodeList = null;
            return datas;
        }
        
        return null;
    }
    //读取道具
    public List<Article> ReadArticle()
    {
        string path = Application.dataPath + "/Resources/Xml/ArticleItem.xml";
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            List<Article> datas = new List<Article>();
            XmlNodeList nodeList = xml.SelectSingleNode("Article").ChildNodes;
            foreach (XmlElement item in nodeList)
            {

                Article article = AnalysisType(item);
                article.name = item.GetAttribute("name");
                article.count = int.Parse(item.GetAttribute("number"));
                article.spritePath = item.GetAttribute("sprite");
                article.price = int.Parse(item.GetAttribute("price"));
                datas.Add(article);
            }
            return datas;
        }
        return null;
    }
    //解析数据的类型
    public Article AnalysisType(XmlElement item)
    {
        if (item.GetAttribute("type") == "0")
        {
            WeaponArticle weapon = new WeaponArticle();
            weapon.attack = float.Parse(item.GetAttribute("attack"));
            weapon.articleType = ArticleType.Weapon;
            return weapon;
        }
        if (item.GetAttribute("type") == "1")
        {
            ShoesArticle shoes = new ShoesArticle();
            shoes.speed = float.Parse(item.GetAttribute("speed"));
            shoes.articleType = ArticleType.Shoes;
            return shoes;
        }
        if (item.GetAttribute("type") == "2")
        {
            ClothingArticle clothing = new ClothingArticle();
            clothing.def = float.Parse(item.GetAttribute("def"));
            clothing.articleType = ArticleType.Clothing;
            return clothing;
        }
        if (item.GetAttribute("type") == "3")
        {
            Article article = new Article();
            article.articleType = ArticleType.Jewellery;
            return article;
        }
        if (item.GetAttribute("type") == "4")
        {
            HatArticle hat = new HatArticle();
            hat.hp = float.Parse(item.GetAttribute("hp"));
            hat.articleType = ArticleType.Hat;
            return hat;
        }
        if (item.GetAttribute("type") == "5")
        {
            Article article = new Article();
            article.articleType = ArticleType.Drug;
            return article;
        }
        return null;
    }
}
