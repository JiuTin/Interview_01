using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrol_Rotation : AutoScrol_Scale
{
    public float rotateAngle;
    protected override void Update()
    {
        base.Update();
        ListenerRotation();
    }
    public void ListenerRotation()
    {
        if (lastPage == nextPage)
        {
            return;
        }
        //对上一个页面和下个页面旋转一定的角度
        float percent = (rect.horizontalNormalizedPosition - horizontalInterval[lastPage]) / (horizontalInterval[nextPage] - rect.horizontalNormalizedPosition);
        games[lastPage].transform.localRotation = Quaternion.Euler(-Vector3.Lerp(Vector3.zero,new Vector3(0,rotateAngle,0),percent));
        games[nextPage].transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, rotateAngle, 0),1-percent));
        //设置其它页面的角度
        for (int i = 0; i < pageCount; i++)
        {
            if (i != lastPage && i != nextPage)
            {
                if (i < currentIndex)
                {
                    games[i].transform.localRotation = Quaternion.Euler(new Vector3(0, -rotateAngle, 0));
                }
                else if (i == currentIndex)
                {
                    games[i].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                else if (i > currentIndex)
                {
                    games[i].transform.localRotation = Quaternion.Euler(new Vector3(0, rotateAngle, 0));
                }
            }
        }
    }
}
