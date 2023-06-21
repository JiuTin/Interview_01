using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    //构造单例模式
    private static UIManager _instance;
    //使用字典保存json文件里的数据
    public Dictionary<string, string> panelPathDic;
    //使用字典保存实例化的panle
    public Dictionary<string, BasePanel> panelDic;

    //创建一个栈来保证单个面板的操纵
    private Stack<BasePanel> panelStack;
    //保存面板的父物体
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
        //实例化字典为空时，初始化字典
        if (panelDic == null)
        {
            panelDic = new Dictionary<string, BasePanel>();
        }
        BasePanel panel = panelDic.GetTry<string, BasePanel>(type);
        //panel为空时，需要实例化面板,并添加到实例化字典里面
        if (panel == null)
        {
            string path = panelPathDic.GetTry<string, string>(type);
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(path));
            //设置父物体，并保持局部坐标
            obj.transform.SetParent(CanvasTrans,false);
            panel = obj.GetComponent<BasePanel>();
            panelDic.Add(type, panel);
        }
        return panel;
    }
    //解析Json文件
    private void GetJsonInfo()
    {
        string jsonString = Resources.Load<TextAsset>("Json/PanelPath").ToString();
        PanelArray panelArray = JsonUtility.FromJson<PanelArray>(jsonString);
        //将json文件里的信息保存到字典里
        if (panelPathDic == null)
        {
            panelPathDic = new Dictionary<string, string>();
        }
        foreach (Panel panel in panelArray.panelArray)
        {
            panelPathDic.Add(panel.type, panel.path);
        }
    }
    //面板入栈
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
    //面板出栈
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
