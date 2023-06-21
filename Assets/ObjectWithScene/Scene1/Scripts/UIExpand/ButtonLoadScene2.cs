using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadScene2 : MonoBehaviour
{
    Button bt;
    void Start()
    {
        bt = transform.GetComponent<Button>();
        bt.onClick.AddListener(() =>
        {
            GameObject.Find("GamePool").GetComponent<AsyncLoadScene>().OnLoadNextScene();
        });
    }

    
}
