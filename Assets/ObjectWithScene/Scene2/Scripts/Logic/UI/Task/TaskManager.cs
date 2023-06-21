using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TaskManager : UIBase
{
    Dictionary<string, TaskData> currentTaskList = new Dictionary<string, TaskData>();  //当前的任务列表
    Dictionary<string, TaskItem> currentItemList = new Dictionary<string, TaskItem>();  //当前的实例对象列表,更新描述
    public static TaskManager Instance;
    //预制体
    public GameObject TaskItem;
    Transform parentTaskItem;   //预制体的父物体
    //任务描述

    //保存的数据
    int normalMoney = 0;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    private void Start()
    {
        parentTaskItem = transform.Find("TaskList/Viewport/Content").GetComponent<Transform>();
        HidePanel();
    }
    /// <summary>
    /// 添加任务到当前任务列表
    /// </summary>
    /// <param name="id"></param>
    public void AddTask(string id)
    {
        //任务数据是否有对应的数据，有就添加
        if (TaskCfg.Instance.taskList.ContainsKey(id))
        {
            //判断当前任务列表里是否有对应的任务
            if (currentTaskList.ContainsKey(id))
            {
                return;
            }
            else
            {
                currentTaskList.Add(id, TaskCfg.Instance.taskList[id]);
                //同时实例化一个任务
                GameObject item = Instantiate(TaskItem, parentTaskItem);
                //并初始化新对象的数据
                InitTaskItem(item, id);
                //添加实例到当前列表里
                currentItemList.Add(id,item.GetComponent<TaskItem>());
            }
        }
    }
    /// <summary>
    /// 更新任务进度
    /// </summary>
    public void AddProgress(string id)
    {
        if (currentTaskList.ContainsKey(id))
        { 
            currentTaskList[id].currentCount++;
            //更新描述进度
            currentItemList[id].AddProgressDes();
        }
    }
    /// <summary>
    /// 完成任务
    /// </summary>
    public void CompleteTask(string id,GameObject game)
    {
        if (currentTaskList.ContainsKey(id))
        {
            //添加奖励
            normalMoney += currentTaskList[id].awardCount;
            //移除任务实例
            Destroy(game);
            //移除当前任务列表
            currentTaskList[id].currentCount = 0;
            currentTaskList.Remove(id);
            currentItemList.Remove(id);
        }
    }
    /// <summary>
    /// 初始化对象的数据
    /// </summary>
    private void InitTaskItem(GameObject game,string id)
    {
        TaskItem taskItem = game.GetComponent<TaskItem>();
        taskItem.datas = currentTaskList[id];
        taskItem.taskNameText.text = taskItem.datas.taskName;
    }
}
