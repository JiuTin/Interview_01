
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TaskData
{
    public int id;
    public string describe;
    public string taskName;
    public int targetCount;
    public int currentCount;
    public string awardSprite;
    public int awardCount;
    public void SetProperty(int id,string describe,string taskName,int targetCount,int currentCount,string awardSprite,int awardCount)
    {
        this.id = id;
        this.describe = describe;
        this.taskName = taskName;
        this.targetCount = targetCount;
        this.currentCount = currentCount;
        this.awardCount = awardCount;
        this.awardSprite = awardSprite;
        this.currentCount = currentCount;
    }
}

public class TaskCfg
{
    public string filePath;
    public Dictionary<string, TaskData> taskList = new Dictionary<string, TaskData>();
    private static TaskCfg _instance;
    public static TaskCfg Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TaskCfg();
            }
            return _instance;
        }

    }
    public TaskCfg()
    {
        filePath = Application.dataPath + "/Resources/Xml/TaskList.xml";
    }
    public Dictionary<string,TaskData> ReadTaskCfg()
    {
        if (File.Exists(filePath))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);
            XmlNodeList nodeList = xml.SelectSingleNode("TaskList").ChildNodes;
            foreach (XmlElement item in nodeList)
            {
                string taskId = item.GetAttribute("id");
                TaskData taskItem = SetElement(item);
                if (taskList.ContainsKey(taskId))
                {
                    continue;
                }
                else
                {
                    taskList.Add(taskId, taskItem);
                }
            }
            return taskList;
        }
        return null;
    }
    TaskData SetElement(XmlElement element)
    {
        if (element != null)
        {
            TaskData item = new TaskData();
            item.id = int.Parse(element.GetAttribute("id"));
            item.describe = element.GetAttribute("describe");
            item.taskName = element.GetAttribute("taskName");
            item.targetCount = int.Parse(element.GetAttribute("targetCount"));
            item.awardCount = int.Parse(element.GetAttribute("awardCount"));
            item.awardSprite = element.GetAttribute("awardSprite");
            item.currentCount = int.Parse(element.GetAttribute("currentCount"));
            return item;
        }
        return null;
    }
}
