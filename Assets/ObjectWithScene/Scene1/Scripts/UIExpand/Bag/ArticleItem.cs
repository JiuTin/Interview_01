using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleItem : MonoBehaviour
{
    private Image articleSprite;
    private Text number;
    public Article article;
    //拖动UI相关字段
    private Canvas canvas;
    private UIDrag uiDrag;
    private int defaultSort;
    private Vector3 currentPos;
    private float moveTimer;
    private float moveTime = 0.2f;
    private bool isBlack=false;
    private Action OnMoveEnd;
    private void Awake()
    {
        articleSprite = transform.GetComponent<Image>();
        number = transform.Find("Text").GetComponent<Text>();
        canvas = GetComponent<Canvas>();
        uiDrag = GetComponent<UIDrag>();
        defaultSort = canvas.sortingOrder;
        uiDrag.onBegin += OnBegin;
        uiDrag.onDrag += OnDrag;
        uiDrag.onEnd += OnEnd;
    }
    public void SetArticle(Article article)
    {
        this.article = article;
        this.article.onDataChange = null;
        this.article.onDataChange += OnDataChange;
        //优化成Sprite
        articleSprite.sprite = Resources.Load(article.spritePath, typeof(Sprite)) as Sprite;
        number.text = article.count.ToString();
    }
    private void Update()
    {
        MovingToOrigin();
    }
    public void MovingToOrigin()
    {
        if (isBlack)
        {
            moveTimer += Time.deltaTime * (1 / moveTime);
            transform.localPosition = Vector3.Lerp(currentPos, Vector3.zero, moveTimer);
            if (moveTimer > 1)
            {
                isBlack = false;
                moveTimer = 0;
                if (OnMoveEnd != null)
                {
                    OnMoveEnd();
                }
            }
        }
    }
    public void MoveToOrigin(Action OnMoveEnd)
    {
        isBlack = true;
        moveTimer = 0;
        currentPos = transform.localPosition;
        this.OnMoveEnd += OnMoveEnd;
    }
    public void OnDrag()
    {
        
    }
    public void OnBegin()
    {
        canvas.sortingOrder = defaultSort + 1;
        BagPanel.Instance.currentArticle = this;
    }
    public void OnEnd()
    {
        //移动到的格子不为空，交换articleItem
        if (BagPanel.Instance.currentGrid != null)
        {
            BagPanel.Instance.currentGrid.DragToGrid(BagPanel.Instance.currentArticle);
            canvas.sortingOrder = defaultSort;
        }
        else
        { 
            MoveToOrigin(() => { canvas.sortingOrder = defaultSort; });
        }
        BagPanel.Instance.currentGrid = null;
        BagPanel.Instance.currentArticle = null;
    }
    //获得article信息
    public string GetArticleInfo()
    {
        return article.GetArticleItemInfo();
    }
    //使用道具
    public void OnDataChange(Article article)
    {
        if (article.count == 0)
        {
            transform.parent.GetComponent<BagGrid>().ClearGrid();
        }
        else
        {
            //更新数据
            SetArticle(this.article);
        }
    }
}
