using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    private Text moneyText;
    private Text valueText;
    public override void Awake()
    {
        base.Awake();
        moneyText = transform.Find("DiamondsInfo/Number").GetComponent<Text>();
        valueText = transform.Find("MoneyInfo/Number").GetComponent<Text>();
        ReadCharacterInfo();
    }
    //点击按钮，实现页面的加载跳转
    public void OnOpenPanel(string type)
    {
        UIManager.Instance.PushPanel(type);
    }
    //加载人物信息
    public void ReadCharacterInfo()
    {
        moneyText.text = PlayerInfo.Instance.money.ToString();
        valueText.text = PlayerInfo.Instance.value.ToString();
            
    }
}
