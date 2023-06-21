using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillController : MonoBehaviour
{

    CharacterSkillSystem skillSystem;
    [Header("技能1")]
    public Button magicAttack1;
    [Header("技能2")]
    public Button magicAttack2;
    private float timer;
    private void Start()
    {
        skillSystem = GetComponent<CharacterSkillSystem>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        SkillInputController();
    }
    public void OnUseSkill(int id)
    {
        //if (skill != null)
        //{ 
        //    //skillManager.GenerateSkill(skill);
        //}    
        //    //使用SkillSystem来调用
        skillSystem.AttackUseSkill(id);
    }
    private void SkillInputController()
    {
        if (Input.GetKeyDown(KeyCode.E) && timer>1f)
        {
            magicAttack1.onClick.Invoke();
            timer = 0;
        }
    }
}
