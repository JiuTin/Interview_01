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
    /// 初始化释放器
    /// </summary>
    private void InitDeployer()
    {
        //选区

        attackSelector = DeployerConfigFactory.CreateAttackSelecor(skillData);
        //影响
        impactEffect = DeployerConfigFactory.CreateImpactEffect(skillData);

    }
    //执行选区算法
    public void CalculateTarget()
    {
        skillData.attackTargets=attackSelector.SelectorTarget(skillData,transform);
    }
    //影响
    public void ImpactTargets()
    {
        for (int i = 0; i < impactEffect.Length; i++)
        {
            impactEffect[i].Execute(this);
        }
    }
    //释放



    //技能释放器，有技能管理器调用
    public abstract void DeployerSkill();
}
