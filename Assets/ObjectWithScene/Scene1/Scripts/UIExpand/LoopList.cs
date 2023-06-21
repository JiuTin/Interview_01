using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopList : MonoBehaviour
{
    public GameObject itemPrefab;
    private GridLayoutGroup contentLayout;
    private ContentSizeFitter contentSize;
    private RectTransform contentRect;
    DataApater<Friend> apater;
    List<Friend> testList;
    private void Awake()
    {
        Init();
        apater = new DataApater<Friend>();
        apater.InitData(XmlManager.Instance.ReadChild());
    }
    private void Start()
    {
        contentSize.enabled = true;
        contentLayout.enabled = true;
        OnAddHead();
        Invoke("EnableWithContent", 0.1f);
    }
    private void Init()
    {
        contentRect = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        if (contentRect == null)
        {
            throw new System.Exception("滚动页面为空");
        }
        contentLayout = transform.Find("Viewport/Content").GetComponent<GridLayoutGroup>();
        contentSize = transform.Find("Viewport/Content").GetComponent<ContentSizeFitter>();
    }
    private void EnableWithContent()
    {
        contentSize.enabled = false;
        contentLayout.enabled = false;
    }
    private GameObject CreateLoopItem()
    {
        //查找对象池里有没有回收的节点
        GameObject item = GamePool.Instance.FindUseableObj("FriendItem");
        if (item != null)
        {
            item.SetActive(true);
            return item;
        }
        //没有回收的节点，创建对象
        item = GamePool.Instance.CreateObj("FriendItem", itemPrefab, contentRect);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        item.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        item.GetComponent<RectTransform>().sizeDelta = contentLayout.cellSize;
        //为实例化的对象添加相应的事件
        item.GetComponent<FriendItem>().OnAddHead += this.OnAddHead;
        item.GetComponent<FriendItem>().OnAddLast += this.OnAddLast;
        item.GetComponent<FriendItem>().OnRemoveHead += this.OnRemoveHead;
        item.GetComponent<FriendItem>().OnRemoveLast += this.OnRemoveLast;
        return item;
    }
    public void OnAddHead()
    {
        Friend friend = apater.GetHeaderData();
        if (friend != null)
        { 
            Transform first = FindFirst();
            GameObject item = CreateLoopItem();
            item.transform.SetAsFirstSibling();
            if (first != null)
            {
                item.transform.localPosition = first.localPosition + new Vector3(0, contentLayout.cellSize.y+contentLayout.spacing.y,0);
            }
            SetData(friend, item);
        }
    }
    public void OnAddLast()
    {
        Friend friend = apater.GetLastData();
        if (friend != null)
        { 
            Transform last = FindLast();
            GameObject item = CreateLoopItem();
            item.transform.SetAsLastSibling();
            if (last != null)
            {
                item.transform.localPosition = last.localPosition - new Vector3(0,contentLayout.cellSize.y + contentLayout.spacing.y,0);
            }
            if (IsNeedAddHeight(item.transform))
            {
                contentRect.sizeDelta += new Vector2(0, contentLayout.cellSize.y + contentLayout.spacing.y);
            }
            SetData(friend, item);
        }
    }
    public void OnRemoveHead()
    {
        if (apater.RemoveHeader())
        { 
            Transform first = FindFirst();
            if (first != null)
            {
                GamePool.Instance.RecoverObj(first.gameObject);
            }  
        }
            
    }
    public void OnRemoveLast()
    {
        if (apater.RemoveLastData())
        { 
            Transform last = FindLast();
            if (last != null)
            {
                GamePool.Instance.RecoverObj(last.gameObject);
            }
        }
    }

    private Transform FindFirst()
    {
        for (int i = 0; i < contentRect.childCount; i++)
        {
            if (contentRect.GetChild(i).gameObject.activeSelf)
            {
                return contentRect.GetChild(i);
            }
        }
        return null;
    }
    private Transform FindLast()
    {
        for (int i = contentRect.childCount - 1; i >= 0; i--)
        {
            if (contentRect.GetChild(i).gameObject.activeSelf)
            {
                return contentRect.GetChild(i);
            }
        }
        return null;
    }
    private bool IsNeedAddHeight(Transform item)
    {
        Vector3[] contentCorners = new Vector3[4];
        Vector3[] itemCorners = new Vector3[4];
        item.GetComponent<RectTransform>().GetWorldCorners(itemCorners);
        contentRect.GetWorldCorners(contentCorners);
        if (itemCorners[0].y < contentCorners[0].y)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 设置好友预制体的头像，名称和ID
    /// </summary>
    /// <param name="friend"></param>
    /// <param name="game"></param>
    private void SetData(Friend friend, GameObject game)
    {
        Sprite sprite = Resources.Load(friend.spirte,typeof(Sprite)) as Sprite;
        
        game.transform.Find("Head_Image").GetComponent<Image>().sprite = sprite;
        game.transform.Find("Name_Text").GetComponent<Text>().text = friend.name;
        game.transform.Find("Id_Text").GetComponent<Text>().text = friend.id;
    }
}

