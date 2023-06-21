using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    //���쵥��ģʽ
    private static UIManager _instance;
    //ʹ���ֵ䱣��json�ļ��������
    public Dictionary<string, string> panelPathDic;
    //ʹ���ֵ䱣��ʵ������panle
    public Dictionary<string, BasePanel> panelDic;

    //����һ��ջ����֤�������Ĳ���
    private Stack<BasePanel> panelStack;
    //�������ĸ�����
    private Transform canvasTrans;
    private Transform CanvasTrans
    {
        get {
            if (canvasTrans == null)
            {
                canvasTrans = GameObject.Find("Canvas").transform;
            }
            return canvasTrans;
        }
    }
    public static UIManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }
    private UIManager()
    {
        GetJsonInfo();
        
    }
    public BasePanel GetPanel(string type)
    {
        //ʵ�����ֵ�Ϊ��ʱ����ʼ���ֵ�
        if (panelDic == null)
        {
            panelDic = new Dictionary<string, BasePanel>();
        }
        BasePanel panel = panelDic.GetTry<string, BasePanel>(type);
        //panelΪ��ʱ����Ҫʵ�������,����ӵ�ʵ�����ֵ�����
        if (panel == null)
        {
            string path = panelPathDic.GetTry<string, string>(type);
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(path));
            //���ø����壬�����־ֲ�����
            obj.transform.SetParent(CanvasTrans,false);
            panel = obj.GetComponent<BasePanel>();
            panelDic.Add(type, panel);
        }
        return panel;
    }
    //����Json�ļ�
    private void GetJsonInfo()
    {
        string jsonString = Resources.Load<TextAsset>("Json/PanelPath").ToString();
        PanelArray panelArray = JsonUtility.FromJson<PanelArray>(jsonString);
        //��json�ļ������Ϣ���浽�ֵ���
        if (panelPathDic == null)
        {
            panelPathDic = new Dictionary<string, string>();
        }
        foreach (Panel panel in panelArray.panelArray)
        {
            panelPathDic.Add(panel.type, panel.path);
        }
    }
    //�����ջ
    public void PushPanel(string type)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }
        BasePanel panel = GetPanel(type);
        panelStack.Push(panel);
        panel.OnEnter();
    }
    //����ջ
    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel lastPanel = panelStack.Peek();
        lastPanel.OnRecover();
    }
    public void ClearData()
    {
        if (panelStack!=null && panelStack.Count > 0)
        { 
            panelStack.Clear();
        }
        if (panelDic != null && panelDic.Count>0)
        {
            panelDic.Clear();
            
        }
    }
}
