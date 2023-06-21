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
    //�����ť��ʵ��ҳ��ļ�����ת
    public void OnOpenPanel(string type)
    {
        UIManager.Instance.PushPanel(type);
    }
    //����������Ϣ
    public void ReadCharacterInfo()
    {
        moneyText.text = PlayerInfo.Instance.money.ToString();
        valueText.text = PlayerInfo.Instance.value.ToString();
            
    }
}
