using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillAttackType
{ 
    Aoe,
    Single
}
public enum SelectorType
{ 
    None,
    Sector,
    Rectangular
}
public enum DisappearType
{ 
    TimeOver,
    CheckOver,
}
[Serializable]
public class SkillData
{

    public int skillId;  //技能ID
    public string name; //技能名称
    public string description;  //描述
    public int skillCd; //技能CD
    public int cdRemain;  //技能冷却
    public int costMp;   //法力值消耗
    public float attackDistance; //攻击距离
    public float attackAngle;  //技能攻击角度
    public string[] attackTargetTags = { "Enemy" }; //目标的Tab
    [HideInInspector]
    public Transform[] attackTargets;  //作用的对象数组
    public string[] impactType = { "CostMp", "Damage" }; //技能影像类型
    public int nextBatterld;  //连击的技能ID
    public float attackRatio;   //伤害数值
    public float durationTime; //持续时间
    public float attackInterval; //伤害间隔
    [HideInInspector]
    public GameObject owner;  //技能所属的角色
    public string prefabName; //技能预制体名称
    [HideInInspector]
    public GameObject skillPrefab;  //预制体对象
    public string animationName; //动画名称
    public string hitFxName;  //受伤特效名称
    [HideInInspector]
    public GameObject hitFxPrefab;  //受击特效预制体
    public int level;  //技能等级
    public SkillAttackType attackType;  //AOE或者单体
    public SelectorType selectorType;  //释放范围类型(圆形，扇形,矩形)
    public string skillIndicator;  //技能指示器名字
    public string skillconName; //技能显示图标名字
    [HideInInspector]
    public Image skillIcon;   //技能事件图标
    public DisappearType disappearType; //技能预制体消失方式
}
