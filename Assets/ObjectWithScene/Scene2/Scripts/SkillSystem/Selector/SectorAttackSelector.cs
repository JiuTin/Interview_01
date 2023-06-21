using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Skill
{
    /// <summary>
    /// ����/Բ��ѡ��
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        GameObject[] tempGoArray;

        /// <summary>
        /// ѡ���㷨          (���ܴ�������)----------------->  ����TODO
        /// </summary>
        /// <param name="data">��������</param>
        /// <param name="skillTF">����Ԥ�����Transform���</param>
        /// <returns></returns>
        public Transform[] SelectorTarget(SkillData data, Transform skillTF)
        {
            //���ݼ��������еı�ǩ ��ȡ����Ŀ��
            List<Transform> targets = new List<Transform>();
            for (int i = 0; i < data.attackTargetTags.Length; i++)
            {
                GameObject[] tempGoArray = GameObject.FindGameObjectsWithTag(data.attackTargetTags[i]);
                //ʹ���������չ����Select
                targets.AddRange(tempGoArray.Select(p => p.transform));
            }

            //�жϹ�����Χ
            targets = targets.FindAll(t =>
                  Vector3.Distance(t.position, skillTF.position) <= data.attackDistance &&
                  Vector3.Angle(skillTF.forward, t.position - skillTF.position) <= data.attackAngle / 2
                );
            //ɸѡ����Ľ�ɫ
            targets = targets.FindAll(t =>
                t.GetComponent<Damageable>().hp > 0
            );
            //����Ŀ��(����/Ⱥ��) ��SkillData.attackType
            Transform[] result = targets.ToArray();
            if (data.attackType == SkillAttackType.Aoe && result.Length==0)
                return result;
            //������ʱ��ѡ�������㷨��ִ���Ƿ���hp���ٵ�
            //���Ǿ��������Ч��

            //��������ĵ���
            //Transform min=result.GetMin(t=>Vectpr3.Distance(t.position,skillTF.position))
            return result;
        }
    }

}
