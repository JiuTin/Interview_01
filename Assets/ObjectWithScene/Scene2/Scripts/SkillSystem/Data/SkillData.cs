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

    public int skillId;  //����ID
    public string name; //��������
    public string description;  //����
    public int skillCd; //����CD
    public int cdRemain;  //������ȴ
    public int costMp;   //����ֵ����
    public float attackDistance; //��������
    public float attackAngle;  //���ܹ����Ƕ�
    public string[] attackTargetTags = { "Enemy" }; //Ŀ���Tab
    [HideInInspector]
    public Transform[] attackTargets;  //���õĶ�������
    public string[] impactType = { "CostMp", "Damage" }; //����Ӱ������
    public int nextBatterld;  //�����ļ���ID
    public float attackRatio;   //�˺���ֵ
    public float durationTime; //����ʱ��
    public float attackInterval; //�˺����
    [HideInInspector]
    public GameObject owner;  //���������Ľ�ɫ
    public string prefabName; //����Ԥ��������
    [HideInInspector]
    public GameObject skillPrefab;  //Ԥ�������
    public string animationName; //��������
    public string hitFxName;  //������Ч����
    [HideInInspector]
    public GameObject hitFxPrefab;  //�ܻ���ЧԤ����
    public int level;  //���ܵȼ�
    public SkillAttackType attackType;  //AOE���ߵ���
    public SelectorType selectorType;  //�ͷŷ�Χ����(Բ�Σ�����,����)
    public string skillIndicator;  //����ָʾ������
    public string skillconName; //������ʾͼ������
    [HideInInspector]
    public Image skillIcon;   //�����¼�ͼ��
    public DisappearType disappearType; //����Ԥ������ʧ��ʽ
}
