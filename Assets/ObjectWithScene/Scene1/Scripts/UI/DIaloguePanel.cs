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
        //�Ż�Ϊ������,������ʾ�Ի�
        if (Input.GetMouseButtonDown(0) && !IsHide())
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
                    StartCoroutine("ScrollDialogue");
                }
            }
            //�����Ի�ʱ,
            else
            {
                //ֹͣЭ��,��ʾ����,������־Ϊfalse
                StopCoroutine("ScrollDialogue");
                contentText.text = dialogueLines[currentLine];
                isScroll = false;
            }
            //����,��currentLine == dialogueLines.Length,���Ի�������,������ʾ�Ի�ѡ���(���ܻ�ܾ�,���������
            //�������б�)
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
        //ֱ����ʾ���Ʋ�������ʾ�Ի�����
        CheckName();
        currentLine++;
        //������ʾ
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
    //�������
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
