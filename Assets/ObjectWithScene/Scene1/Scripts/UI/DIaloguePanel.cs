using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DIaloguePanel : BasePanel
{
    private bool isScroll=false;
    [TextArea(1,3)]
    public string[] dialogueLines;
    private int currentLine;
    private WaitForSeconds waitSecond;
    public Text contentText;
    public Text nameText;
    public override void Awake()
    {
        base.Awake();
        waitSecond = new WaitForSeconds(0.2f);
    }

    private void Update()
    {
        //优化为点击鼠标,快速显示对话
        if (Input.GetMouseButtonDown(0) && !IsHide())
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
                    StartCoroutine("ScrollDialogue");
                }
            }
            //滚动对话时,
            else
            {
                //停止协程,显示内容,滚动标志为false
                StopCoroutine("ScrollDialogue");
                contentText.text = dialogueLines[currentLine];
                isScroll = false;
            }
            //补充,当currentLine == dialogueLines.Length,即对话结束后,可以显示对话选择框(接受或拒绝,接受添加任
            //务到任务列表)
            if (currentLine>=dialogueLines.Length)
            {
                UIManager.Instance.PopPanel();
            }
        }
    }
    public override void OnEnter()
    {
        base.OnEnter();
        ShowDialogue(dialogueLines);
    }
    public void ShowDialogue(string[] contentLines)
    {
        dialogueLines = contentLines;
        currentLine = 0;
        //直接显示名称并继续显示对话内容
        CheckName();
        currentLine++;
        //逐字显示
        StartCoroutine("ScrollDialogue");

    }
    private  IEnumerator ScrollDialogue()
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
        if (currentLine<dialogueLines.Length && dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            return true;
        }
        return false;
    }
}
