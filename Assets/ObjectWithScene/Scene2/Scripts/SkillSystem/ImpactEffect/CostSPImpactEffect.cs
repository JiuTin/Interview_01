using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skill
{
    /// <summary>
    /// 消耗法力
    /// </summary>
    public class CostSPImpactEffect : IImpactEffect
    {
        /// <summary>
        /// 执行的算法功能，  CostSP-》 消耗SP
        /// </summary>
        /// <param name="deployer">获得释放器里的效果执行算法，以便可以开启协程等功能(如：持续伤害)</param>
        public void Execute(SkillDeployer deployer)
        {
            var statue=deployer.SkillData.owner.GetComponent<Damageable>();
            //判断是否消耗过量
            statue.sp -= deployer.SkillData.costMp;
        }
    }

}
