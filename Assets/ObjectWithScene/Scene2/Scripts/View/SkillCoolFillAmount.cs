using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolFillAmount : MonoBehaviour
{
    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0;
        this.enabled = false;
    }

    private void Update()
    {
        subtractCool();
    }

    void subtractCool()
    {
        if (image.fillAmount == 0)
        {
            this.enabled = false;
        }
        image.fillAmount -= Time.deltaTime/5;
    }

    public void ResetCool()
    {
        image.fillAmount = 1f;
        this.enabled = true;
    }
}
