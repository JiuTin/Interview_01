using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkillDeployer : SkillDeployer
{
    public override void DeployerSkill()
    {
        //执行选区算法
        CalculateTarget();

        //如果有需要运动，则在SkillDeployer子类里实现运动相关的算法

        //执行影响算法
        ImpactTargets();
    }
}
