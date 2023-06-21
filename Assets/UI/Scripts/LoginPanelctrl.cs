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
            helpText.text = "������˺Ų���Ϊ��";
            return;
        }
        //�ж��˺��Ƿ����
        if (!LoginModel.Instance.AccountExists(accountFile.text))
        {
            helpText.text = "�˺Ų�����";
            return;
        }
        //�жϵ�¼����
        if (LoginModel.Instance.Matching(accountFile.text, passwordFile.text))
        {
            //������ת
            SceneManager.LoadScene(1);
            return;
        }
    }

}
