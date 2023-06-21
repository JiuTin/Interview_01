using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPanelctrl : MonoBehaviour
{
    public InputField accountFile;
    public InputField passwordFile;

    public Button login;
    public Button regist;

    public Text helpText;
    public RegistPanelctr registPanel;
    private void Start()
    {
        login.onClick.AddListener(loginCheck);
        regist.onClick.AddListener(
            () => {
                gameObject.SetActive(false);
                registPanel.gameObject.SetActive(true);
            }
            );
    }
    void loginCheck()
    {
        if (accountFile.text.Length == 0 || passwordFile.text.Length == 0)
        {
            helpText.text = "密码或账号不能为空";
            return;
        }
        //判断账号是否存在
        if (!LoginModel.Instance.AccountExists(accountFile.text))
        {
            helpText.text = "账号不存在";
            return;
        }
        //判断登录密码
        if (LoginModel.Instance.Matching(accountFile.text, passwordFile.text))
        {
            //场景跳转
            SceneManager.LoadScene(1);
            return;
        }
    }

}
