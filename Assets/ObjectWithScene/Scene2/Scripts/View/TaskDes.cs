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

    //任务按钮
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
    /// 设置描述
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
    /// 更新进度
    /// </summary>
    /// <param name="item">任务数据</param>
    public void CheckTask(TaskData item)
    {
        //判断增加的任务是否与当前显示的任务一致
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
            //跟新
            taskProgress.text = item.targetCount + "/" + item.targetCount;
            //设置按钮为完成状态，并激活按钮
            stateText.text = "完成";
            stateButton.enabled = true;
        }
        else
        {
            //跟新进度
            taskProgress.text= item.currentCount + "/" + item.targetCount;
            //设置按钮为未完成状态，并禁用按钮
            stateText.text = "未完成";
            stateButton.enabled = false;
        }
    }
    /// <summary>
    /// 完成任务
    /// </summary>
    public void OnCompleteTask()
    {
        if (taskTarget != null)
        {
            //清除任务列表里的任务
            TaskManager.Instance.CompleteTask(taskTarget.GetComponent<TaskItem>().datas.id.ToString(),taskTarget);
            //并重置描述信息
            ResetDes();
        }
    }
    /// <summary>
    /// 重置描述
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
        stateText.text = "未完成";
    }
}
