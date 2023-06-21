using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanelctr : MonoBehaviour
{
    public InputField account;
    public InputField password;
    public InputField passAgain;

    public Button regist;
    public Text helpText;

    private void Start()
    {
        //注册验证
        regist.onClick.AddListener(RegistCheck);
        //输入验证
        account.onValueChanged.AddListener(
            (string value)=> {
                account.transform.Find("helpT").GetComponent<Text>().text = "";
            }
            );
        account.onEndEdit.AddListener(
            (string value) => {
                if (LoginModel.Instance.AccountExists(account.text))
                {
                    account.transform.Find("helpT").GetComponent<Text>().text = "账号已存在";
                }
            }
            );
    }
    void RegistCheck()
    {
        if (account.text.Length == 0 || password.text.Length == 0 || passAgain.text.Length == 0)
        {
            helpText.text = "账号密码不能为空！";
            helpText.color = Color.red;
            return;
        }
        if (LoginModel.Instance.AccountExists(account.text))
        {
            helpText.text = "账号已存在";
            
            return;
        }
        if (password.text != passAgain.text)
        {
            helpText.text = "密码不一致";
            
            return;
        }
        helpText.text = "注册成功";
        helpText.color = Color.green;
        LoginModel.Instance.AccountSave(account.text, password.text);
        regist.interactable = false;
    }
}
