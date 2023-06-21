using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1;
        UIManager.Instance.PushPanel("MenuPanel");
        //XmlManager.Instance.CreateXml();
    }

}
