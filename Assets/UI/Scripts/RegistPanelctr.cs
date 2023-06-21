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
        //ע����֤
        regist.onClick.AddListener(RegistCheck);
        //������֤
        account.onValueChanged.AddListener(
            (string value)=> {
                account.transform.Find("helpT").GetComponent<Text>().text = "";
            }
            );
        account.onEndEdit.AddListener(
            (string value) => {
                if (LoginModel.Instance.AccountExists(account.text))
                {
                    account.transform.Find("helpT").GetComponent<Text>().text = "�˺��Ѵ���";
                }
            }
            );
    }
    void RegistCheck()
    {
        if (account.text.Length == 0 || password.text.Length == 0 || passAgain.text.Length == 0)
        {
            helpText.text = "�˺����벻��Ϊ�գ�";
            helpText.color = Color.red;
            return;
        }
        if (LoginModel.Instance.AccountExists(account.text))
        {
            helpText.text = "�˺��Ѵ���";
            
            return;
        }
        if (password.text != passAgain.text)
        {
            helpText.text = "���벻һ��";
            
            return;
        }
        helpText.text = "ע��ɹ�";
        helpText.color = Color.green;
        LoginModel.Instance.AccountSave(account.text, password.text);
        regist.interactable = false;
    }
}
