using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TaskItem : MonoBehaviour, IPointerClickHandler
{
    public TaskData datas;
    public Text taskNameText;
    public TaskDes taskDes;
    public void OnPointerClick(PointerEventData eventData)
    {
        //设置TaskDes的数据
        taskDes.SetDes(datas,this.gameObject);
    }

    /// <summary>
    /// 鼠标点击事件，触发设置TaskDes的方法
    /// </summary>
    /// <param name="eventData"></param>

    private void Awake()
    {
        taskNameText = transform.Find("TaskName").GetComponent<Text>();
        taskDes = GameObject.Find("Canvas/TaskPanel/TaskDes").GetComponent<TaskDes>();
    }
    /// <summary>
    /// 更新进度的描述
    /// </summary>
    public void AddProgressDes()
    {
        taskDes.CheckTask(datas);
    }

}
