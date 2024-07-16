using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : CharAnim
{
    //EnemyCombat combat;
    EnemyStats EnemyStats;
    public bool attackCountdown;
    override public void Start()
    {
        base.Start();
        //  combat = GetComponentInParent<EnemyCombat>();
        // combat.OnPlayAttackEnemy += AttackDefault;
        // combat.OnStan += OnStan;
        EnemyStats=GetComponent<EnemyStats>();
        attackCountdown = true;
    }

    override public void Update()
    {
        base.Update();

      

    }
    private IEnumerator initialCountdownMetod()
    {
        attackCountdown = false;
        // Ждём одну секунду
        yield return new WaitForSeconds(2f);
        attackCountdown = true;
        // Вызываем OnDrop

    }
    public void AttackDefault()
    {
       
        if (attackCountdown)
        {
            Debug.Log("Происходит атака");
            animator.SetTrigger("AttackTrigger");
            StartCoroutine(initialCountdownMetod());


        }
      


    }
    public void OnStan()
    {
        animator.SetTrigger("Stan");
        Debug.Log("СТАН!");
    }


    public void DoDamage()
    {
        PlayerStats stats = playerController.instance.playerStats;
        int AttackPower = 0;
        if (UnityEngine.Random.Range(1, 100) <= EnemyStats.CriticalStrikeChance.GetValue())
        {
            AttackPower = EnemyStats.CalculationCurrentAttackPowerFromStrength() + EnemyStats.CalculationCurrentAttackPowerFromStrength() / 100 * EnemyStats.CriticalHitPercentage.GetValue();
            //  Debug.Log(transform.name + " наносит крит урон в размере " + AttackPower + " damage");


        }
        else
        {
            AttackPower = EnemyStats.CalculationCurrentAttackPowerFromStrength();
            //  Debug.Log(transform.name + " наносит урон в размере " + AttackPower + " damage");

        }

        int FD = stats.TakeNormalDamage(EnemyStats.currentLvl, AttackPower, ((int)EnemyStats.ElementPowerCharacter), EnemyStats.ElementalDamage[(int)EnemyStats.ElementPowerCharacter].GetValue(), EnemyStats.Accuracy.GetValue());


        if (((int)EnemyStats.ElementPowerCharacter) != 0 && FD != 0)
        {
            int[] NullMas = { 0, 0 };
            stats.PeriodicElementalDamage(EnemyStats.timeDE, ((int)EnemyStats.ElementPowerCharacter), FD, EnemyStats.RandomPercentage, EnemyStats.ElementalDebuffChance.GetValue());
        }


    }


    public void finishOfAttackEnemys()
    {

       // GetComponentInParent<EnemyCombat>().attackCountdown = true;
       // GetComponentInParent<EnemyCombat>().StanTime = true;
        // Debug.Log("Конец анимации атаки от " + transform.name);
    }


}
