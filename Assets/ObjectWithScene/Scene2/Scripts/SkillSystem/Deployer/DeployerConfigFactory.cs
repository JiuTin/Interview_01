using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 释放器配置工厂
    /// 将对象的创建和使用分离
    /// </summary>
    public class DeployerConfigFactory
    {
        public static IAttackSelector CreateAttackSelecor(SkillData data)
        {
            //创建算法对象:skillData.selectorType
            //选取对象命名规则：Skill.SectorAttackSelector
            //反射创建选区对象
            string className = string.Format("Skill.{0}AttackSelector", data.selectorType);
            return CreateObject<IAttackSelector>(className);
        }
        public static IImpactEffect[] CreateImpactEffect(SkillData data)
        {
            IImpactEffect[] impacts = new IImpactEffect[data.impactType.Length];
            //创建算法对象：skillData.impactType[?]
            //选取对象命名规则：Skill.[?]ImpactEffect
            //反射创建影响效果对象
            for (int i = 0; i < data.impactType.Length; i++)
            {
                string impactEffectName = string.Format("Skill.{0}ImpactEffect", data.impactType[i]);
                impacts[i] = CreateObject<IImpactEffect>(impactEffectName);
            }
            return impacts;
        }
        private static Dictionary<string, object> cache;       //反射缓存

        /// <summary>
        /// 初始化cache缓存
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
