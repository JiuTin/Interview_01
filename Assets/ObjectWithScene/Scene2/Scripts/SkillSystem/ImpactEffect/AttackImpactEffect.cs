using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{

    public class AttackImpactEffect : IImpactEffect
    {
        //SkillData data;         //ʹ�û���󣬻ᱻ�����ļ��ܸ�ֵ��ʹ���ݴ���
        DamageMessage damageMessage=new DamageMessage();
        public void Execute(SkillDeployer deployer)
        {
            //data = deployer.SkillData;
            deployer.StartCoroutine(RepeatDamage(deployer));
        }
        /// <summary>
        /// ����˺�
        /// </summary>
        /// <param name="deployer">SkillDeployer�ͷ���,����Э��(�ű����ӿ��޷�����Э��),�����ѡ���㷨</param>
        /// <returns></returns>
        private IEnumerator RepeatDamage(SkillDeployer deployer)
        {
            float atkTime = 0;
            do
            {
                OnceDamage(deployer.SkillData);
                yield return new WaitForSeconds(deployer.SkillData.attackInterval);
                //���¼������Ŀ�꣬���磺��ɫ��һ�������ļ��ܺ󣬵����뿪��Χ����ʱ���˲�Ӧ���ܵ��˺�����Ҫ���¼���Ŀ��
                deployer.CalculateTarget();
                atkTime += deployer.SkillData.attackInterval;
            } while (atkTime < deployer.SkillData.durationTime);
        }
        /// <summary>
        /// �����˺�
        ///      ���˺����ݸ�OnDamage ��,���ܻ��д���   //TODO
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

            //����������Ч......
        }
    }
}
