using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skill
{
    /// <summary>
    /// ¹¥»÷Ñ¡Çø
    /// </summary>
    public interface IAttackSelector
    {
        Transform[] SelectorTarget(SkillData data, Transform skillTF);
    }

}
