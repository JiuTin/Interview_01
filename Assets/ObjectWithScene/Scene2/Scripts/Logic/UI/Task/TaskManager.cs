using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TaskManager : UIBase
{
    Dictionary<string, TaskData> currentTaskList = new Dictionary<string, TaskData>();  //��ǰ�������б�
    Dictionary<string, TaskItem> currentItemList = new Dictionary<string, TaskItem>();  //��ǰ��ʵ�������б�,��������
    public static TaskManager Instance;
    //Ԥ����
    public GameObject TaskItem;
    Transform parentTaskItem;   //Ԥ����ĸ�����
    //��������

    //���������
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
    /// ������񵽵�ǰ�����б�
    /// </summary>
    /// <param name="id"></param>
    public void AddTask(string id)
    {
        //���������Ƿ��ж�Ӧ�����ݣ��о����
        if (TaskCfg.Instance.taskList.ContainsKey(id))
        {
            //�жϵ�ǰ�����б����Ƿ��ж�Ӧ������
            if (currentTaskList.ContainsKey(id))
            {
                return;
            }
            else
            {
                currentTaskList.Add(id, TaskCfg.Instance.taskList[id]);
                //ͬʱʵ����һ������
                GameObject item = Instantiate(TaskItem, parentTaskItem);
                //����ʼ���¶��������
                InitTaskItem(item, id);
                //���ʵ������ǰ�б���
                currentItemList.Add(id,item.GetComponent<TaskItem>());
            }
        }
    }
    /// <summary>
    /// �����������
    /// </summary>
    public void AddProgress(string id)
    {
        if (currentTaskList.ContainsKey(id))
        { 
            currentTaskList[id].currentCount++;
            //������������
            currentItemList[id].AddProgressDes();
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    public void CompleteTask(string id,GameObject game)
    {
        if (currentTaskList.ContainsKey(id))
        {
            //��ӽ���
            normalMoney += currentTaskList[id].awardCount;
            //�Ƴ�����ʵ��
            Destroy(game);
            //�Ƴ���ǰ�����б�
            currentTaskList[id].currentCount = 0;
            currentTaskList.Remove(id);
            currentItemList.Remove(id);
        }
    }
    /// <summary>
    /// ��ʼ�����������
    /// </summary>
    private void InitTaskItem(GameObject game,string id)
    {
        TaskItem taskItem = game.GetComponent<TaskItem>();
        taskItem.datas = currentTaskList[id];
        taskItem.taskNameText.text = taskItem.datas.taskName;
    }
}
