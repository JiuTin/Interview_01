using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Dialogue : UIBase
{

    public string taskId;   //����id
    private bool isScroll = false;
    [TextArea(1, 3)]
    public string[] dialogueLines;
    private int currentLine;
    private WaitForSeconds waitSecond;
    public Text contentText;
    public Text nameText;
    public PlayableDirector director; 
    bool isShow;         //����Ƿ���ʾ
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
        //�Ż�Ϊ������,������ʾ�Ի�
        if (Input.GetMouseButtonDown(0) )
        {
            //�ǹ����Ի�ʱ
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
            //�����Ի�ʱ,
            else
            {
                //ֹͣЭ��,��ʾ����,������־Ϊfalse
                StopCoroutine("ScrollDialogue2");
                contentText.text = dialogueLines[currentLine];
                isScroll = false;
            }
            //����,��currentLine == dialogueLines.Length,���Ի�������,������ʾ�Ի�ѡ���(���ܻ�ܾ�,���������
            //�������б�)
            if (currentLine >= dialogueLines.Length)
            {
                //ѡ�����ʱ��������񣬹رնԻ�,���ָ��Խ�ɫ�Ŀ���
                TaskManager.Instance.AddTask(taskId);
                HidePanel();
                isShow = false;
                StartTimeLine();
                //GameObject.Find("Player").GetComponent<PlayerController>().isIdle = false;
            }
        }
    }

    /// <summary>
    /// ����timeLine
    /// </summary>
    public void StartTimeLine()
    {
        director.Play();
    }

    /// <summary>
    /// ȡ��TimeLine
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
        //ֱ����ʾ���Ʋ�������ʾ�Ի�����
        CheckName();
        currentLine++;
        //������ʾ
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
    /// ������ʾ
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
    //�������
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
