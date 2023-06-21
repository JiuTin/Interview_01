using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// �ͷ������ù���
    /// ������Ĵ�����ʹ�÷���
    /// </summary>
    public class DeployerConfigFactory
    {
        public static IAttackSelector CreateAttackSelecor(SkillData data)
        {
            //�����㷨����:skillData.selectorType
            //ѡȡ������������Skill.SectorAttackSelector
            //���䴴��ѡ������
            string className = string.Format("Skill.{0}AttackSelector", data.selectorType);
            return CreateObject<IAttackSelector>(className);
        }
        public static IImpactEffect[] CreateImpactEffect(SkillData data)
        {
            IImpactEffect[] impacts = new IImpactEffect[data.impactType.Length];
            //�����㷨����skillData.impactType[?]
            //ѡȡ������������Skill.[?]ImpactEffect
            //���䴴��Ӱ��Ч������
            for (int i = 0; i < data.impactType.Length; i++)
            {
                string impactEffectName = string.Format("Skill.{0}ImpactEffect", data.impactType[i]);
                impacts[i] = CreateObject<IImpactEffect>(impactEffectName);
            }
            return impacts;
        }
        private static Dictionary<string, object> cache;       //���仺��

        /// <summary>
        /// ��ʼ��cache����
        /// </summary>
        static DeployerConfigFactory()
        {
            cache = new Dictionary<string, object>();
        }
            
        public static T CreateObject<T>(string className) where T : class
        {
            if (!cache.ContainsKey(className))
            {
                Type type = Type.GetType(className);
                object instance=Activator.CreateInstance(type) as T;
                cache.Add(className, instance);
            }
            return cache[className] as T;
        }
    }

}
