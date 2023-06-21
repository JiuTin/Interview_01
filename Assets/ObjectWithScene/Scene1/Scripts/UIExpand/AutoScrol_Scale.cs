using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrol_Scale : AutoScrol
{
    //������С�ı����ز���
    public float currentScale = 1.0f;
    public float otherScale = 0.6f;
    public int lastPage;
    public int nextPage;
    public GameObject[] games;
    protected override void Start()
    {
        base.Start();
        games = new GameObject[pageCount];
        for (int i = 0; i < pageCount; i++)
        {
            games[i] = transform.Find("Viewport/Content").GetChild(i).gameObject;
        }
    }
    protected override void Update()
    {
        base.Update();
        ListenerScale();
    }
    public void ListenerScale()
    {
        //�ҵ���һ��ҳ�����һ��ҳ��
        for (int i = 0; i < horizontalInterval.Length; i++)
        {
            if (horizontalInterval[i] < rect.horizontalNormalizedPosition)
            {
                lastPage = i;
            }
        }
        for (int i = 0; i < horizontalInterval.Length; i++)
        {
            if (horizontalInterval[i] > rect.horizontalNormalizedPosition)
            {
                nextPage = i;
                break;
            }
        }
        if (lastPage == nextPage)
        {
            return;
        }
        //�޸�ҳ���Scale��С
        float percent = (rect.horizontalNormalizedPosition - horizontalInterval[lastPage]) / (horizontalInterval[nextPage] - rect.horizontalNormalizedPosition);
        games[lastPage].transform.localScale = Vector3.Lerp(Vector3.one*currentScale,Vector3.one*otherScale,percent);
        games[nextPage].transform.localScale = Vector3.Lerp(Vector3.one * currentScale, Vector3.one * otherScale,1-percent);
        //����������ҳ��Ϊ��С��״̬
        for (int i = 0; i < games.Length; i++)
        {
            if (i != lastPage && i != nextPage)
            {
                games[i].transform.localScale = Vector3.one * otherScale;
            }
        }
    }
}
