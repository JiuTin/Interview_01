using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill 
{
    /// <summary>
    /// Ӱ��Ч��
    /// </summary>
    public interface IImpactEffect
    {
        //ִ��Ӱ��Ч���� ������������Ч��������buff������˺�
        void Execute(SkillDeployer deployer);
    }
}
