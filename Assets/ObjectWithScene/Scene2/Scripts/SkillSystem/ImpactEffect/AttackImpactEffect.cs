using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{

    public class AttackImpactEffect : IImpactEffect
    {
        //SkillData data;         //使用缓存后，会被其它的技能赋值，使数据错误。
        DamageMessage damageMessage=new DamageMessage();
        public void Execute(SkillDeployer deployer)
        {
            //data = deployer.SkillData;
            deployer.StartCoroutine(RepeatDamage(deployer));
        }
        /// <summary>
        /// 多次伤害
        /// </summary>
        /// <param name="deployer">SkillDeployer释放器,开启协程(脚本，接口无法开启协程),并获得选区算法</param>
        /// <returns></returns>
        private IEnumerator RepeatDamage(SkillDeployer deployer)
        {
            float atkTime = 0;
            do
            {
                OnceDamage(deployer.SkillData);
                yield return new WaitForSeconds(deployer.SkillData.attackInterval);
                //重新计算敌人目标，例如：角色放一个持续的技能后，敌人离开范围，此时敌人不应该受到伤害，需要重新计算目标
                deployer.CalculateTarget();
                atkTime += deployer.SkillData.attackInterval;
            } while (atkTime < deployer.SkillData.durationTime);
        }
        /// <summary>
        /// 单次伤害
        ///      将伤害传递给OnDamage 后,可能会有错误   //TODO
        /// </summary>
        private void OnceDamage(SkillData data)
        {
            float atk = data.attackRatio * data.owner.GetComponent<Damageable>().atk;
            damageMessage.damage = atk;
            for (int i = 0; i < data.attackTargets.Length; i++)
            {
                var status = data.attackTargets[i].GetComponent<Damageable>();
                status.OnDamage(damageMessage);
            }

            //创建攻击特效......
        }
    }
}
