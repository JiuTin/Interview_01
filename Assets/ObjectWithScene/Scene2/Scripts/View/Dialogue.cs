using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Dialogue : UIBase
{

    public string taskId;   //任务id
    private bool isScroll = false;
    [TextArea(1, 3)]
    public string[] dialogueLines;
    private int currentLine;
    private WaitForSeconds waitSecond;
    public Text contentText;
    public Text nameText;
    public PlayableDirector director; 
    bool isShow;         //面板是否显示
    //public static Dialogue Instance;
    protected override void Awake()
    {
        //Instance = this;
        base.Awake();
        waitSecond = new WaitForSeconds(0.2f);
        HidePanel();
    }
    private void Update()
    {
        if (!isShow)
        {
            return;
        }
        //优化为点击鼠标,快速显示对话
        if (Input.GetMouseButtonDown(0) )
        {
            //非滚动对话时
            if (!isScroll)
            {
                currentLine++;
                if (CheckName())
                {
                    currentLine++;
                }
                if (currentLine < dialogueLines.Length)
                {
                    StartCoroutine("ScrollDialogue2");
                }
            }
            //滚动对话时,
            else
            {
                //停止协程,显示内容,滚动标志为false
                StopCoroutine("ScrollDialogue2");
                contentText.text = dialogueLines[currentLine];
                isScroll = false;
            }
            //补充,当currentLine == dialogueLines.Length,即对话结束后,可以显示对话选择框(接受或拒绝,接受添加任
            //务到任务列表)
            if (currentLine >= dialogueLines.Length)
            {
                //选择接受时，添加任务，关闭对话,并恢复对角色的控制
                TaskManager.Instance.AddTask(taskId);
                HidePanel();
                isShow = false;
                StartTimeLine();
                //GameObject.Find("Player").GetComponent<PlayerController>().isIdle = false;
            }
        }
    }

    /// <summary>
    /// 激活timeLine
    /// </summary>
    public void StartTimeLine()
    {
        director.Play();
    }

    /// <summary>
    /// 取消TimeLine
    /// </summary>
    public void EndTimeLine()
    {
        director.Stop();
        GameObject.Find("Player").GetComponent<PlayerController>().isIdle = false;
    }
    public void ShowDialogue(string[] contentLines)
    {
        dialogueLines = contentLines;
        currentLine = 0;
        //直接显示名称并继续显示对话内容
        CheckName();
        currentLine++;
        //逐字显示
        if (gameObject.activeSelf)
        {
            StartCoroutine("ScrollDialogue2");
            isShow = true;
        }
        else
        {
            StopAllCoroutines();
        }

    }
    /// <summary>
    /// 逐字显示
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScrollDialogue2()
    {
        isScroll = true;
        contentText.text = "";
        foreach (char i in dialogueLines[currentLine].ToCharArray())
        {
            contentText.text += i;
            yield return waitSecond;
        }
        isScroll = false;
    }
    //检查名字
    private bool CheckName()
    {
        if (currentLine < dialogueLines.Length && dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            return true;
        }
        return false;
    }
}
