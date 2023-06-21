using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    private List<Article> dataList = new List<Article>();
    public GameObject articlePrefab;
    private BagGrid[] bagGrids;
    private List<GameObject> games = new List<GameObject>();
    private bool isHiden;
    public static BagPanel Instance;

    //装备格子
    public EquipWeapon weaponGrid;
    public EquipHat hatGrid;
    public EquipShoes shoesGrid;
    public EquipClothing clothingGrid;
    //道具移动相关字段
    public ArticleItem currentArticle;
    public BagGrid currentGrid;

    //提示信息面板
    public ArticleItemInfo info;

    //人物属性信息

    public override void Awake()
    {
        base.Awake();
        Instance = this;
        isHiden = true;
        InitArticle();
        bagGrids = GetComponentsInChildren<BagGrid>();
    }

    //加载全部类型的道具
    public void LoadData()
    {
        HideAllArticleItems();
        for (int i = 0; i < dataList.Count; i++)
        {
            GetBagGrid().SetArticleItem(GetArticleItem(dataList[i]));
        }
    }

    //加载对应类型的道具
    public void LoadData(ArticleType type)
    {
        if (isHiden)
        { 
            HideAllArticleItems();
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].articleType == type)
            { 
                GetBagGrid().SetArticleItem(GetArticleItem(dataList[i]));
            }
        }
    }
    //获得空闲的格子
    public BagGrid GetBagGrid()
    {
        for (int i = 0; i < bagGrids.Length; i++)
        {
            if (bagGrids[i].ArticleItem == null || !bagGrids[i].ArticleItem.gameObject.activeSelf)
            {
                return bagGrids[i];
            }
        }
        return null;
    }
    public void HideAllArticleItems()
    {
        for (int i = 0; i < bagGrids.Length; i++)
        {
            if (bagGrids[i].ArticleItem != null)
            { 
                bagGrids[i].ClearGrid();
            }
        }
    }
    public ArticleItem GetArticleItem(Article article)
    {
        GameObject obj = GetArticlePrefab();
        ArticleItem articleItem = obj.GetComponent<ArticleItem>();
        articleItem.SetArticle(article);
        return articleItem;
    }
    public GameObject GetArticlePrefab()
    {
        for (int i = 0; i < games.Count; i++)
        {
            if (games[i].activeSelf == false)
            {
                games[i].SetActive(true);
                return games[i];
            }
        }
        return GameObject.Instantiate(articlePrefab);
    }
    public void InitArticle()
    {
        dataList = XmlManager.Instance.ReadArticle();
        
    }
    //删除数据
    public void RemoveArticle(Article article)
    {
        this.dataList.Remove(article);
    }
    //背包添加逻辑
    public void AddArticleData(Article article)
    {
        article.count++;
        if (dataList.Contains(article))
        {
            for (int i = 0; i < bagGrids.Length; i++)
            {
                if (bagGrids[i].ArticleItem != null)
                { 
                
                    //更新显示
                    if (bagGrids[i].ArticleItem.article == article)
                    {
                        bagGrids[i].ArticleItem.SetArticle(article);
                        break;
                    }
                }
            }
        }
        else
        {
            dataList.Add(article);
            //有做修改
            ArticleItem articleItem = GetArticlePrefab().GetComponent<ArticleItem>();
            articleItem.SetArticle(article);
            GetBagGrid().SetArticleItem(articleItem);
        }
    }

    public void AddFromShop()
    {
        if (Save.Instance.GetShopData() != null)
        { 
            List<Article> itemList = Save.Instance.GetShopData();
        
            bool isAdd = false;
            for(int i=0;i<itemList.Count;i++)
            {
                for (int j = 0; j < dataList.Count; j++)
                {
                    if (itemList[i].name == dataList[j].name)
                    {
                        dataList[j].count++;
                        isAdd = true;
                        break;
                    }
                }
                if (isAdd == false)
                {
                    dataList.Add(itemList[i]);
                }
                isAdd = false;
            }
            Save.Instance.ClearShopData();
        }
    }


    #region   对应得事件
    public void OnAllToggleValueChange(bool v)
    {
        if(v){ LoadData(); }
    }
    public void OnWeaponToggelValueChange(bool v)
    {
        if(v)
        {
            LoadData(ArticleType.Weapon);
            isHiden = false;
            LoadData(ArticleType.Hat);
            LoadData(ArticleType.Shoes);
            LoadData(ArticleType.Clothing);
            isHiden = true;
        }
    }
    public void OnDrugValueChange(bool v)
    {
        if (v)
        {
            LoadData(ArticleType.Drug);
        }
    }
    public void OnJewelleryValueChange(bool v)
    {
        if (v)
        {
            LoadData(ArticleType.Jewellery);
        }
    }
    #endregion
    public override void OnEnter()
    {
        base.OnEnter();
        AddFromShop();
        LoadData();
    }
}
