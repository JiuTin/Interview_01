using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopGrid : MonoBehaviour,IPointerClickHandler
{
    public Article article;
    private Image articleSprite;
    private Text info;
    public Color defaultColor;
    public GameObject sign;
    public bool isChose;
    private void Awake()
    {
        articleSprite = transform.Find("Image").GetComponent<Image>();
        info = transform.Find("Text").GetComponent<Text>();
        defaultColor = this.GetComponent<Image>().color;
        sign = transform.Find("Sign").gameObject;
        sign.SetActive(false);
        isChose = false;
    }
    public void SetArticle(Article article)
    {
        if (article != null)
        { 
            this.article = article;
        }
    }
    public void SetGrid()
    {
        isChose = false;
        sign.SetActive(false);
        articleSprite.sprite = Resources.Load(article.spritePath, typeof(Sprite)) as Sprite;
        info.text = GetInfo();
    }
    //获得info
    private string GetInfo()
    {
        if (article != null)
        {
            return GetTypeInfo(this.article);
        }
        return null;
    }
    //根据article的类型设计info
    private string GetTypeInfo(Article article)
    {
        StringBuilder sb = new StringBuilder();
        if (article.articleType == ArticleType.Weapon)
        {
            WeaponArticle weapon = (WeaponArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("攻击:").Append("<color=red>");
            sb.Append(weapon.attack).Append("</color>");
        }
        else if (article.articleType == ArticleType.Clothing)
        {
            ClothingArticle weapon = (ClothingArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("防御:").Append("<color=greed>");
            sb.Append(weapon.def).Append("</color>");
        }
        else if (article.articleType == ArticleType.Hat)
        {
            HatArticle weapon = (HatArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("生命:").Append("<color=red>");
            sb.Append(weapon.hp).Append("</color>");
        }
        else if (article.articleType == ArticleType.Shoes)
        {
            ShoesArticle weapon = (ShoesArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("速度:").Append("<color=red>");
            sb.Append(weapon.speed).Append("</color>");
        }
        else
        {
            sb.Append(article.name).Append("\n");
        }
        sb.Append("\n").Append("价格:");
        sb.Append("<color=blue>").Append(article.price).Append("</color>");
        return sb.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isChose == true)
        {
            return;
        }
        if (ShopPanel.Instance.currentChose != null)
        {
            ShopPanel.Instance.currentChose.GetComponent<Image>().color = ShopPanel.Instance.currentChose.GetComponent<ShopGrid>().defaultColor;
        }
        ShopPanel.Instance.currentChose = this.gameObject;
        //选中特效
        GetComponent<Image>().color = new Color(1,1,1,1);
        isChose = true;
    }
}
