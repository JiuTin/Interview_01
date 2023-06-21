using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Skill
{
    /// <summary>
    /// 扇形/圆形选区
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        GameObject[] tempGoArray;

        /// <summary>
        /// 选区算法          (可能存在问题)----------------->  单攻TODO
        /// </summary>
        /// <param name="data">技能数据</param>
        /// <param name="skillTF">技能预制体的Transform组件</param>
        /// <returns></returns>
        public Transform[] SelectorTarget(SkillData data, Transform skillTF)
        {
            //根据技能数据中的标签 获取所有目标
            List<Transform> targets = new List<Transform>();
            for (int i = 0; i < data.attackTargetTags.Length; i++)
            {
                GameObject[] tempGoArray = GameObject.FindGameObjectsWithTag(data.attackTargetTags[i]);
                //使用数组的扩展方法Select
                targets.AddRange(tempGoArray.Select(p => p.transform));
            }

            //判断攻击范围
            targets = targets.FindAll(t =>
                  Vector3.Distance(t.position, skillTF.position) <= data.attackDistance &&
                  Vector3.Angle(skillTF.forward, t.position - skillTF.position) <= data.attackAngle / 2
                );
            //筛选出活的角色
            targets = targets.FindAll(t =>
                t.GetComponent<Damageable>().hp > 0
            );
            //返回目标(单攻/群攻) ，SkillData.attackType
            Transform[] result = targets.ToArray();
            if (data.attackType == SkillAttackType.Aoe && result.Length==0)
                return result;
            //单攻的时候，选择具体的算法来执行是返回hp最少的
            //还是距离最近等效果

            //距离最近的敌人
            //Transform min=result.GetMin(t=>Vectpr3.Distance(t.position,skillTF.position))
            return result;
        }
    }

}
