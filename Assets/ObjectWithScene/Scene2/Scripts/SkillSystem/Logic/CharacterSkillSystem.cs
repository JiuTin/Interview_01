using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 技能系统
/// </summary>
[RequireComponent(typeof(CharacterSkillManager))]
public class CharacterSkillSystem : MonoBehaviour
{
    private CharacterSkillManager manager;
    private Animator animator;
    private SkillData skill;
    private void Start()
    {
        manager = GetComponent<CharacterSkillManager>();
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// 为玩家提供的使用技能的方法
    /// </summary>
    public void AttackUseSkill(int id)
    {
        //准备技能
        skill = manager.PrepareSkill(id);
        if (skill == null) return;
        //播放动画,   可以将动画的播放与动画事件结合起来使用
        animator.SetTrigger(skill.animationName);
        //生成技能,由动画调用DeployerAnimationEvent

        //如果单攻
        //if (skill.attackType != SkillAttackType.Single) return;
        //查找目标
        //Transform targetTF = SelectTarget();
        //--朝向目标
        //transform.LookAt()
        //选中目标
    }
    /// <summary>
    /// 动画事件,在对应动画Clip的相应时间调用
    /// </summary>
    public void DeployerAnimatorEven()
    {
        manager.GenerateSkill(skill);
    }
    /// <summary>
    /// 为AI提供的使用技能的方法
    /// </summary>
    public void UseRandomSkill()
    {
        //1.思路：先获得随机数，再获得技能。  缺点：获得随机数后，使用的技能有可能不满足条件(SP不足等)
        //2.思路：先筛选可释放的技能，再获得随机数。  推荐使用思路2
          //FindAll可能存在错误  *********TODO
        var usableSkills = manager.skills.FindAll(s => manager.PrepareSkill(s.skillId) != null);
        if (usableSkills.Length == 0) return;
        int index = Random.Range(0, usableSkills.Length);
        AttackUseSkill(usableSkills[index].skillId);
    }

    //private void SelectTarget()
    //{ 
    //    var targetsTF=new Sector
    //}
}
