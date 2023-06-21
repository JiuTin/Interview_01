using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipGrid : BagGrid
{
    protected Article article;
    public Image articleSprite;
    protected ArticleType currentType;
    protected override void Awake()
    {
        base.Awake();
        articleSprite = transform.Find("Image").GetComponent<Image>();
        articleSprite.gameObject.SetActive(false);
    }

    public virtual void Equip(Article article)
    {
        if(this.article!=null)
        {
            UnEquip();
        }
        this.article = article;
        articleSprite.sprite = Resources.Load(article.spritePath, typeof(Sprite)) as Sprite;
        articleSprite.gameObject.SetActive(true);
        //修改属性
    }
    public void UnEquip()
    {
        if (this.article != null)
        {
            BagPanel.Instance.AddArticleData(article);
            articleSprite.gameObject.SetActive(false);
            this.article = null;
            //修改属性
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (BagPanel.Instance.currentArticle != null)
        {
            if (BagPanel.Instance.currentArticle.article.articleType == currentType)
            {
                GetComponent<Image>().color = Color.green;

            }
            else
            {
                if (BagPanel.Instance.currentArticle == null)
                { return; }
                GetComponent<Image>().color = Color.red;
            }
        }
        
    }
    public override void DragToGrid(ArticleItem article)
    {
        //base.DragToGrid(article);
        if (article.article.articleType == currentType)
        {
            Equip(article.article);
            article.article.UseDataArticle();
        }
        article.MoveToOrigin(null);
    }
}
