using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillManager : MonoBehaviour
{
    public SkillData[] skills;
    private void Start()
    {
        
        foreach (var skill in skills)
        {
            InitSkill(skill);
            
        }
    }
    /// <summary>
    /// ��ʼ������
    /// </summary>
    private void InitSkill(SkillData skill)
    {
        //����Ԥ���岢���ü��ܵĳ����߶���
        skill.skillPrefab = Resources.Load<GameObject>("Skill/" + skill.prefabName);
        skill.owner = gameObject;
    }
    /// <summary>
    /// ׼������
    /// </summary>
    /// <param name="id">����id</param>
    /// <returns>��������</returns>
    public SkillData PrepareSkill(int id)
    {
        //����id,���Ҽ�������
       SkillData skill= skills.Find(s => s.skillId == id);
        //��õ�ǰ��ɫ����,GetComponent<ChatacterStatus>().sp
        Damageable status = GetComponent<Damageable>();
        if (skill != null && skill.cdRemain <= 0 && skill.costMp <= status.sp)
        {
            return skill;
        }
        else
        {
            return null;
        }      
    }
    /// <summary>
    /// ���ɼ���
    /// </summary>
    public void GenerateSkill(SkillData skill)
    {
        //ʵ��������
        //GameObject skillGo = Instantiate(skill.skillPrefab, transform.position, transform.rotation);
        //���ټ���
        //Destroy(skillGo, skill.durationTime);
        //ʹ�ö���ش�������
        GameObject skillGo = GamePool.Instance.CreateObj(skill.prefabName, skill.skillPrefab, transform.position, transform.rotation);
        //���ݼ�������
        SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();  
        deployer.SkillData = skill;  //�ڲ������㷨����
        deployer.DeployerSkill();    //�ڲ�ִ���㷨����
        //ʹ�ö���ػ��ն���
        GamePool.Instance.RecoverObjDelay(skillGo, skill.durationTime);
        //������ȴ
        StartCoroutine(CoolTimeDown(skill));
    }
    /// <summary>
    /// ������ȴ
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    private  IEnumerator CoolTimeDown(SkillData skill)
    {
        Text coolTime = GameObject.Find("Canvas/SkillPanel/"+skill.name+"/Image/Text").GetComponent<Text>();
        SkillCoolFillAmount skillCoolTest = coolTime.GetComponentInParent<SkillCoolFillAmount>();
        skill.cdRemain = skill.skillCd;
        skillCoolTest.ResetCool();
        while (skill.cdRemain > 0)
        {
            yield return new WaitForSeconds(1);
            skill.cdRemain--;
            //���ð�ťͼ�����ȴ
            if (skill.cdRemain == 0)
            {
                coolTime.text = "";

            } 
            
            else coolTime.text = skill.cdRemain.ToString();
        }
    }
}
