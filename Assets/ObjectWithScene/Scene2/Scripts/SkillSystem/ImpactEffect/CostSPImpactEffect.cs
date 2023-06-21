using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skill
{
    /// <summary>
    /// ���ķ���
    /// </summary>
    public class CostSPImpactEffect : IImpactEffect
    {
        /// <summary>
        /// ִ�е��㷨���ܣ�  CostSP-�� ����SP
        /// </summary>
        /// <param name="deployer">����ͷ������Ч��ִ���㷨���Ա���Կ���Э�̵ȹ���(�磺�����˺�)</param>
        public void Execute(SkillDeployer deployer)
        {
            var statue=deployer.SkillData.owner.GetComponent<Damageable>();
            //�ж��Ƿ����Ĺ���
            statue.sp -= deployer.SkillData.costMp;
        }
    }

}
