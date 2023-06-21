using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelInput : MonoBehaviour
{
    bool keyCode_Z;
    bool keyCode_Esc;
    GameObject currentPanel;
    private void Update()
    {
        PanelComtroller();
        PanelController();
    }

    void PanelComtroller()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!keyCode_Z)
            {
                keyCode_Z = true;
                TaskManager.Instance.ShowPanel();
            }
            else
            {
                keyCode_Z = false;
                TaskManager.Instance.HidePanel();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            keyCode_Esc = !keyCode_Esc;
            currentPanel = transform.Find("PausePanel").gameObject;
        }
    }

    void PanelController()
    {
        if (keyCode_Esc)
        {
            currentPanel?.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            currentPanel?.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
