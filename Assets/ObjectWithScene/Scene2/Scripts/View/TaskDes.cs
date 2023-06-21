using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskDes : MonoBehaviour
{
    Text taskName;
    Text taskDes;
    Text taskProgress;
    Text awardCount;
    Image awardSprite;
    GameObject taskTarget;

    //����ť
    Button stateButton;
    Text stateText;
    private void Awake()
    {
        taskName = transform.Find("TaskName_Text").GetComponent<Text>();
        taskDes = transform.Find("TaskDes_Text").GetComponent<Text>();
        taskProgress = transform.Find("Progress_Text").GetComponent<Text>();
        awardCount = transform.Find("AwardCount_Text").GetComponent<Text>();
        awardSprite = transform.Find("Award_Image").GetComponent<Image>();
        stateButton = transform.Find("TaskState_Button").GetComponent<Button>();
        stateText = transform.Find("TaskState_Button/TaskState_Text").GetComponent<Text>();
    }
    private void Start()
    {
        stateButton.enabled = false;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="item"></param>
    public void SetDes(TaskData item,GameObject game)
    {
        taskName.text = item.taskName;
        taskDes.text = item.describe;
        taskProgress.text = item.currentCount + "/" + item.targetCount;
        awardCount.text ="X"+ item.awardCount.ToString();
        awardSprite.sprite = Resources.Load(item.awardSprite, typeof(Sprite)) as Sprite;
        taskTarget = game;
        CheckTask(item);
    }
    /// <summary>
    /// ���½���
    /// </summary>
    /// <param name="item">��������</param>
    public void CheckTask(TaskData item)
    {
        //�ж����ӵ������Ƿ��뵱ǰ��ʾ������һ��
        if (taskTarget !=null && taskTarget.GetComponent<TaskItem>().datas != item)
        {
            return;
        }
        if (taskTarget == null)
        {
            return;
        }
        if (item.currentCount >= item.targetCount)
        {
            //����
            taskProgress.text = item.targetCount + "/" + item.targetCount;
            //���ð�ťΪ���״̬�������ť
            stateText.text = "���";
            stateButton.enabled = true;
        }
        else
        {
            //���½���
            taskProgress.text= item.currentCount + "/" + item.targetCount;
            //���ð�ťΪδ���״̬�������ð�ť
            stateText.text = "δ���";
            stateButton.enabled = false;
        }
    }
    /// <summary>
    /// �������
    /// </summary>
    public void OnCompleteTask()
    {
        if (taskTarget != null)
        {
            //��������б��������
            TaskManager.Instance.CompleteTask(taskTarget.GetComponent<TaskItem>().datas.id.ToString(),taskTarget);
            //������������Ϣ
            ResetDes();
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    public void ResetDes()
    {
        taskName.text = "TaskName";
        taskDes.text = "TaskDescribe";
        taskProgress.text = "TaskProgress";
        awardCount.text = "awardCount";
        awardSprite.sprite = null;
        taskTarget = null;
        stateButton.enabled = false;
        stateText.text = "δ���";
    }
}
