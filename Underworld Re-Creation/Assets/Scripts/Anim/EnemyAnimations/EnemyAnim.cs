using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : CharAnim
{
    //EnemyCombat combat;
    

    override public void Start()
    {
        base.Start();
      //  combat = GetComponentInParent<EnemyCombat>();
       // combat.OnPlayAttackEnemy += AttackDefault;
       // combat.OnStan += OnStan;
    }

    override public void Update()
    {
        base.Update();

      

    }

    public void AttackDefault()
    {
        animator.SetTrigger("Attack");


    }
    public void OnStan()
    {
        animator.SetTrigger("Stan");
        Debug.Log("СТАН!");
    }

    protected virtual void momentOfAttack()
    {
       // GetComponentInParent<EnemyCombat>().MOA();
    }

    public void finishOfAttackEnemys()
    {

       // GetComponentInParent<EnemyCombat>().attackCountdown = true;
       // GetComponentInParent<EnemyCombat>().StanTime = true;
        // Debug.Log("Конец анимации атаки от " + transform.name);
    }
}
