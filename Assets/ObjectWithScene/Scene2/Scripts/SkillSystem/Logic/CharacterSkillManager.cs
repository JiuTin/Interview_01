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
    /// 初始化技能
    /// </summary>
    private void InitSkill(SkillData skill)
    {
        //加载预制体并设置技能的持有者对象
        skill.skillPrefab = Resources.Load<GameObject>("Skill/" + skill.prefabName);
        skill.owner = gameObject;
    }
    /// <summary>
    /// 准备技能
    /// </summary>
    /// <param name="id">技能id</param>
    /// <returns>技能数据</returns>
    public SkillData PrepareSkill(int id)
    {
        //根据id,查找技能数据
       SkillData skill= skills.Find(s => s.skillId == id);
        //获得当前角色法力,GetComponent<ChatacterStatus>().sp
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
    /// 生成技能
    /// </summary>
    public void GenerateSkill(SkillData skill)
    {
        //实例化技能
        //GameObject skillGo = Instantiate(skill.skillPrefab, transform.position, transform.rotation);
        //销毁技能
        //Destroy(skillGo, skill.durationTime);
        //使用对象池创建对象
        GameObject skillGo = GamePool.Instance.CreateObj(skill.prefabName, skill.skillPrefab, transform.position, transform.rotation);
        //传递技能数据
        SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();  
        deployer.SkillData = skill;  //内部创建算法对象
        deployer.DeployerSkill();    //内部执行算法对象
        //使用对象池回收对象
        GamePool.Instance.RecoverObjDelay(skillGo, skill.durationTime);
        //技能冷却
        StartCoroutine(CoolTimeDown(skill));
    }
    /// <summary>
    /// 技能冷却
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
            //设置按钮图标的冷却
            if (skill.cdRemain == 0)
            {
                coolTime.text = "";

            } 
            
            else coolTime.text = skill.cdRemain.ToString();
        }
    }
}
