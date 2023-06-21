using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;
public abstract class SkillDeployer : MonoBehaviour
{
    private SkillData skillData;

    private IAttackSelector attackSelector;
    private IImpactEffect[] impactEffect;
    public SkillData SkillData
    {
        get {
            return skillData;
        }
        set {
            skillData = value;
            InitDeployer();
        }
    }
    /// <summary>
    /// ��ʼ���ͷ���
    /// </summary>
    private void InitDeployer()
    {
        //ѡ��

        attackSelector = DeployerConfigFactory.CreateAttackSelecor(skillData);
        //Ӱ��
        impactEffect = DeployerConfigFactory.CreateImpactEffect(skillData);

    }
    //ִ��ѡ���㷨
    public void CalculateTarget()
    {
        skillData.attackTargets=attackSelector.SelectorTarget(skillData,transform);
    }
    //Ӱ��
    public void ImpactTargets()
    {
        for (int i = 0; i < impactEffect.Length; i++)
        {
            impactEffect[i].Execute(this);
        }
    }
    //�ͷ�



    //�����ͷ������м��ܹ���������
    public abstract void DeployerSkill();
}
