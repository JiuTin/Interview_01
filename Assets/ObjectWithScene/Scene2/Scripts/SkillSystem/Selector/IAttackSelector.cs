using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skill
{
    /// <summary>
    /// ����ѡ��
    /// </summary>
    public interface IAttackSelector
    {
        Transform[] SelectorTarget(SkillData data, Transform skillTF);
    }

}
