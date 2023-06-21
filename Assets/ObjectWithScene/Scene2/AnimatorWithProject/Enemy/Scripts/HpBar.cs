using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    //public Gradient gradient;
    Slider slider;
    private void Awake()
    {
        //gradient = GetComponent<Gradient>();
        slider = GetComponent<Slider>();
    }
    public void SetHpValue(float damage)
    {
        slider.value = damage;
    }
    public void SetMaxValue(float hp)
    {
        slider.maxValue = hp;
        slider.value = hp; 
    }
}
