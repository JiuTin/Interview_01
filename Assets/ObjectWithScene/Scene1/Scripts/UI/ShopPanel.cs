using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    private List<Article> dataList;
    public ShopGrid[] grids;
    public GameObject currentChose;
    public static ShopPanel Instance;
    private void Start()
    {
        base.Awake();
        dataList = new List<Article>();
        dataList = XmlManager.Instance.ReadArticle();
        grids = transform.Find("ArticleChosePanel").GetComponentsInChildren<ShopGrid>();
        SetArticles();
        Instance = this;
    }
    public void SetArticles()
    {
        for (int i = 0; i < grids.Length; i++)
        {
            int range = Random.Range(0, dataList.Count);
            grids[i].SetArticle(dataList[range]);
            grids[i].SetGrid();
        }
    }
    public override void OnClosePanel()
    {
        base.OnClosePanel();
        if (currentChose != null)
        {
            ShopPanel.Instance.currentChose.GetComponent<Image>().color = ShopPanel.Instance.currentChose.GetComponent<ShopGrid>().defaultColor;
            currentChose = null;
        }
    }
    public void OnReset()
    {
        //���Sign
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].sign.SetActive(false);
        }
        SetArticles();
    }
    public void OnBuy()
    {
        if (currentChose != null)
        {
            //�жϼ۸��Ƿ��㹻
            //����ʱ
            if (currentChose.GetComponent<ShopGrid>().article.price > PlayerInfo.Instance.money)
            {
                //��ʾ��ʾ��Ϣ
            }
            else
            {
                //�㹻ʱ
                Save.Instance.SetShopData(currentChose.GetComponent<ShopGrid>().article);
                //�ı乺�����Ʒ��ʹ�䲻�ɹ���
                currentChose.GetComponent<ShopGrid>().sign.SetActive(true);
                currentChose.GetComponent<Image>().color = currentChose.GetComponent<ShopGrid>().defaultColor;
                //ˢ����ҳ��
                PlayerInfo.Instance.money -= currentChose.GetComponent<ShopGrid>().article.price;
                GameObject.Find("MenuPanel(Clone)").GetComponent<MenuPanel>().ReadCharacterInfo();
            }
            
        }
        currentChose = null;
    }
}
