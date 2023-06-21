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
        //����TaskDes������
        taskDes.SetDes(datas,this.gameObject);
    }

    /// <summary>
    /// ������¼�����������TaskDes�ķ���
    /// </summary>
    /// <param name="eventData"></param>

    private void Awake()
    {
        taskNameText = transform.Find("TaskName").GetComponent<Text>();
        taskDes = GameObject.Find("Canvas/TaskPanel/TaskDes").GetComponent<TaskDes>();
    }
    /// <summary>
    /// ���½��ȵ�����
    /// </summary>
    public void AddProgressDes()
    {
        taskDes.CheckTask(datas);
    }

}
