using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleItemInfo : MonoBehaviour
{
    private RectTransform rect;
    private Text infoText;
    //Vector3[] infoCorners;
    //Vector3[] currentCorners;
    private void Awake()
    {
        
        infoText = transform.Find("Text").GetComponent<Text>();
        rect = transform.Find("Text").GetComponent<RectTransform>();
    }
    private void LateUpdate()
    {
        rect.transform.position = Input.mousePosition+Vector3.one;
        //ListenerCorners();
    }
    //public void ListenerCorners()
    //{
    //    transform.GetComponent<RectTransform>().GetWorldCorners(currentCorners);
    //    rect.GetWorldCorners(infoCorners);
    //    Vector2 pivot = rect.pivot;
    //    if (infoCorners[0].x < currentCorners[0].x)
    //    {
    //        pivot.x = 0.15f;
    //    }
    //    if (infoCorners[3].x > currentCorners[3].x)
    //    {
    //        pivot.x = 1.15f;
    //    }
    //    if (infoCorners[1].y > currentCorners[1].y)
    //    {
    //        pivot.y = 1.15f;
    //    }
    //    if (infoCorners[0].y < currentCorners[0].y)
    //    {
    //        pivot.y = 0.15f;
    //    }
    //}
    public void Show()
    {
        this.gameObject.SetActive(true);
        rect.transform.position = Input.mousePosition;
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void SetInfo(string info)
    { 
        if(infoText!=null)
        {
            this.infoText.text = info;
        }
    }
}
