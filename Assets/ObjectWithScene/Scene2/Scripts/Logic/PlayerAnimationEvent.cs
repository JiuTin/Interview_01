using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public WeaponAttack weaponAttack;
    public void AttackBegin()
    {
        weaponAttack.BeginAttack();
    }
    public void AttackEnd()
    {
        weaponAttack.EndAttack();
    }
}
