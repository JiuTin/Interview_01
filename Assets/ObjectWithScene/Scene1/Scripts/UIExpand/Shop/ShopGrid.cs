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
    //���info
    private string GetInfo()
    {
        if (article != null)
        {
            return GetTypeInfo(this.article);
        }
        return null;
    }
    //����article���������info
    private string GetTypeInfo(Article article)
    {
        StringBuilder sb = new StringBuilder();
        if (article.articleType == ArticleType.Weapon)
        {
            WeaponArticle weapon = (WeaponArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("����:").Append("<color=red>");
            sb.Append(weapon.attack).Append("</color>");
        }
        else if (article.articleType == ArticleType.Clothing)
        {
            ClothingArticle weapon = (ClothingArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("����:").Append("<color=greed>");
            sb.Append(weapon.def).Append("</color>");
        }
        else if (article.articleType == ArticleType.Hat)
        {
            HatArticle weapon = (HatArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("����:").Append("<color=red>");
            sb.Append(weapon.hp).Append("</color>");
        }
        else if (article.articleType == ArticleType.Shoes)
        {
            ShoesArticle weapon = (ShoesArticle)article;
            sb.Append(weapon.name).Append("\n");
            sb.Append("�ٶ�:").Append("<color=red>");
            sb.Append(weapon.speed).Append("</color>");
        }
        else
        {
            sb.Append(article.name).Append("\n");
        }
        sb.Append("\n").Append("�۸�:");
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
        //ѡ����Ч
        GetComponent<Image>().color = new Color(1,1,1,1);
        isChose = true;
    }
}
