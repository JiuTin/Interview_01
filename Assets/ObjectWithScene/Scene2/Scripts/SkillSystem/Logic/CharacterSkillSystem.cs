using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����ϵͳ
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
    /// Ϊ����ṩ��ʹ�ü��ܵķ���
    /// </summary>
    public void AttackUseSkill(int id)
    {
        //׼������
        skill = manager.PrepareSkill(id);
        if (skill == null) return;
        //���Ŷ���,   ���Խ������Ĳ����붯���¼��������ʹ��
        animator.SetTrigger(skill.animationName);
        //���ɼ���,�ɶ�������DeployerAnimationEvent

        //�������
        //if (skill.attackType != SkillAttackType.Single) return;
        //����Ŀ��
        //Transform targetTF = SelectTarget();
        //--����Ŀ��
        //transform.LookAt()
        //ѡ��Ŀ��
    }
    /// <summary>
    /// �����¼�,�ڶ�Ӧ����Clip����Ӧʱ�����
    /// </summary>
    public void DeployerAnimatorEven()
    {
        manager.GenerateSkill(skill);
    }
    /// <summary>
    /// ΪAI�ṩ��ʹ�ü��ܵķ���
    /// </summary>
    public void UseRandomSkill()
    {
        //1.˼·���Ȼ����������ٻ�ü��ܡ�  ȱ�㣺����������ʹ�õļ����п��ܲ���������(SP�����)
        //2.˼·����ɸѡ���ͷŵļ��ܣ��ٻ���������  �Ƽ�ʹ��˼·2
          //FindAll���ܴ��ڴ���  *********TODO
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
