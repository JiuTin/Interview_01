using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hallin : EnemyBase
{
    public string taskProgress;
    protected override void Attack()
    {
        if (isAttack)
        { 
            SetDirection();
            base.Attack();
            animator.SetTrigger(attackHash);
        }
    }
    /// <summary>
    /// …Ë÷√π•ª˜∑ΩœÚ
    /// </summary>
    private void SetDirection()
    {
        if (target != null)
        { 
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    public void BeginAttack()
    { 
        
    }

    public void EndAttack()
    {
        animator.ResetTrigger(attackHash);
        isAttack = false;
    }

    public void OnDeathEvent()
    {   
        if (taskProgress != "" || taskProgress != null)
        { 
            TaskManager.Instance.AddProgress(taskProgress);
        }
        gameObject.SetActive(false);
    }
}
