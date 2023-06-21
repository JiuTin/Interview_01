using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save 
{
    private List<Article> shopList = new List<Article>();
    private static Save _instance;
    public static Save Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new Save();
            }
            return _instance;
        }
    }

    public void SetShopData(Article article)
    {
        if (shopList == null)
        {
            shopList = new List<Article>();
        }
        shopList.Add(article);   
    }
    public void ClearShopData()
    {
        shopList=null;

    }
    public List<Article> GetShopData()
    {
        if (shopList != null)
        { 
            return shopList;
        }
        return null;
    }
}
