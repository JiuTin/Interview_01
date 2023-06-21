using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill 
{
    /// <summary>
    /// 影响效果
    /// </summary>
    public interface IImpactEffect
    {
        //执行影响效果， 可以是其他的效果：增加buff或造成伤害
        void Execute(SkillDeployer deployer);
    }
}
