using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

/*ʹ��MVCʵ�ֵ�¼ע��ģ��ĵ�¼
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
            Debug.Log("��ʼ��");
            Instance = this;
            Init(AppConst.loginPath);
        }
        else
        {
            Debug.Log("�ѳ�ʼ��");
        }
    }
    //�ж��˺��Ƿ����
    public bool AccountExists(string account)
    {
        return accountLib.ContainsKey(account);
    }
    //�ж������Ƿ���ȷ
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
    //��ʼ���˺ſ�
    void Init(string _failName)
    {
        //�ж��ļ��Ƿ����
        if(!File.Exists(_failName))
        {
            Debug.Log("�ļ�����");
            return;
        }
        XmlDocument xmlD = new XmlDocument();
        //����xml�ĵ�����ò�����
        xmlD.Load(_failName);
        //��ȡroot��ǩ
        var root = xmlD.SelectSingleNode("root") as XmlElement;
        foreach (XmlElement item in root.ChildNodes)
        {
            //��ȡ�˺ŵ��˺ſ���
            accountLib.Add(item.GetAttribute("name"), item.GetAttribute("password"));
        }
    }
    //ע���˺�
    public void AccountSave(string account,string password)
    {
        accountLib.Add(account, password);
        XmlDocument xmlD = new XmlDocument();
        XmlElement root = null;
        //������ǩ
        XmlElement accountE = xmlD.CreateElement("account");
        accountE.SetAttribute("name", account);
        accountE.SetAttribute("password", password);
        //�ļ������ڣ���һ��ע��
        if (!File.Exists(AppConst.loginPath))
        {
            Debug.Log(1);
            root = xmlD.CreateElement("Root");
            xmlD.AppendChild(root);
            root.AppendChild(accountE);
            xmlD.Save(AppConst.loginPath);
            return;
        }
        //�ǵ�һ��ע��
        xmlD.Load(AppConst.loginPath);
        root = xmlD.SelectSingleNode("root") as XmlElement;
        root.AppendChild(accountE);
        xmlD.Save(AppConst.loginPath);
    }
}
