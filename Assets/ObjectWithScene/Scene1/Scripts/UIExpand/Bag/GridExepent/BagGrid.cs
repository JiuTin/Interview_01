using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagGrid : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    protected ArticleItem item;
    public ArticleItem ArticleItem
    {
        get {
            return item;
        }
    }
    //颜色调整
    protected Image gridImage;
    protected Color defaultColor;
    protected virtual void Awake()
    {
        gridImage = transform.GetComponent<Image>();
        defaultColor = gridImage.color;
    }
    public void ClearGrid()
    {
        //item.transform.SetParent(null);
        item.gameObject.SetActive(false);
        item = null;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (BagPanel.Instance.currentArticle != null)
        {
            BagPanel.Instance.currentGrid = this;
            gridImage.color = Color.green;
        }
        if (item != null)
        {
            BagPanel.Instance.info.Show();
            //设置info信息
            BagPanel.Instance.info.SetInfo(item.GetArticleInfo());
        }
    }

    public  void OnPointerExit(PointerEventData eventData)
    {
        BagPanel.Instance.currentGrid = null;
        gridImage.color = defaultColor;
        BagPanel.Instance.info.Hide();
    }
    /// <summary>
    /// 交换格子道具
    /// </summary>
    /// <param name="article">格子里的道具</param>
    public virtual void DragToGrid(ArticleItem article)
    {
        BagGrid lastGrid = article.transform.parent.GetComponent<BagGrid>();
        if (this.item == null)
        {
            lastGrid.ClearGrid();
            SetArticleItem(article);
        }
        else
        {
            lastGrid.SetArticleItem(this.item);
            SetArticleItem(article);
        }
    }
    public void SetArticleItem(ArticleItem item)
    {
        this.item = item;
        this.item.gameObject.SetActive(true);
        this.item.transform.SetParent(transform);
        //可以在预制体里调整
        this.item.transform.localPosition = Vector3.zero;
        this.item.transform.localScale = Vector3.one;
        //设置articleitem与grid之间的间隔
        this.item.MoveToOrigin(() =>
        {
            item.GetComponent<RectTransform>().offsetMax = new Vector2(-2, -2);
            item.GetComponent<RectTransform>().offsetMin = new Vector2(2, 2);
        });
    }
}
