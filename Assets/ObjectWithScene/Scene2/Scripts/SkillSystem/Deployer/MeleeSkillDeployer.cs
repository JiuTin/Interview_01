using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkillDeployer : SkillDeployer
{
    public override void DeployerSkill()
    {
        //ִ��ѡ���㷨
        CalculateTarget();

        //�������Ҫ�˶�������SkillDeployer������ʵ���˶���ص��㷨

        //ִ��Ӱ���㷨
        ImpactTargets();
    }
}
