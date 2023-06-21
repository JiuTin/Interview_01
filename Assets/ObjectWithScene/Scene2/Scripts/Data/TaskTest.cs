using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTest : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<string, TaskData> testDic=new Dictionary<string, TaskData>();
    public bool isPause=false;
    private void Awake()
    {
        Time.timeScale = 1;
    }
    void Start()
    {
        testDic = TaskCfg.Instance.ReadTaskCfg();
    }

}
