using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

/*使用MVC实现登录注册模块的登录
 * 
 */
public class LoginModel
{
    public static LoginModel Instance=new LoginModel();

    Dictionary<string, string> accountLib = new Dictionary<string, string>();
    public LoginModel()
    {
        if (Instance == null)
        {
            Debug.Log("初始化");
            Instance = this;
            Init(AppConst.loginPath);
        }
        else
        {
            Debug.Log("已初始化");
        }
    }
    //判断账号是否存在
    public bool AccountExists(string account)
    {
        return accountLib.ContainsKey(account);
    }
    //判断密码是否正确
    public bool Matching(string account,string password)
    {
        if(AccountExists(account))
        {
            if (accountLib[account].Equals(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    //初始化账号库
    void Init(string _failName)
    {
        //判断文件是否存在
        if(!File.Exists(_failName))
        {
            Debug.Log("文件有误");
            return;
        }
        XmlDocument xmlD = new XmlDocument();
        //加载xml文档，获得操作流
        xmlD.Load(_failName);
        //读取root标签
        var root = xmlD.SelectSingleNode("root") as XmlElement;
        foreach (XmlElement item in root.ChildNodes)
        {
            //读取账号到账号库中
            accountLib.Add(item.GetAttribute("name"), item.GetAttribute("password"));
        }
    }
    //注册账号
    public void AccountSave(string account,string password)
    {
        accountLib.Add(account, password);
        XmlDocument xmlD = new XmlDocument();
        XmlElement root = null;
        //创建标签
        XmlElement accountE = xmlD.CreateElement("account");
        accountE.SetAttribute("name", account);
        accountE.SetAttribute("password", password);
        //文件不存在，第一次注册
        if (!File.Exists(AppConst.loginPath))
        {
            Debug.Log(1);
            root = xmlD.CreateElement("Root");
            xmlD.AppendChild(root);
            root.AppendChild(accountE);
            xmlD.Save(AppConst.loginPath);
            return;
        }
        //非第一次注册
        xmlD.Load(AppConst.loginPath);
        root = xmlD.SelectSingleNode("root") as XmlElement;
        root.AppendChild(accountE);
        xmlD.Save(AppConst.loginPath);
    }
}
