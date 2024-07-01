using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharStats
{
    public event System.Action OnHealthReachedZero;
    public int[] RandomPercentage = { 1, 2 };
    public override void Start()
    {
        base.Start();
       // AnimationControl = GetComponent<EnemyAnim>().animator;
       // AnimationControl.speed = CalculationCurrentAttackSpeed();
    }


    public override void Update()
    {
        
    }

    public IEnumerator ElementalsNegativeEffect(object? deb)
    {
        //Создание переменных и запись в них конвертируемых данных из object
        System.Random rnd1 = new System.Random();
        //Полный финальный урон
        int AttackP = 1;
        //количество секунд что будет длиться дебаф
        int timeEffect = 1;
        //Какой тип элемента наносит урон
        int numElement = 1;
        //от скольки до скольки может быть процент от AttackP
        int[] RandomPercentage = new int[2];
        if (deb is object[] debuff)
        {
            timeEffect = (int)debuff[0];
            numElement = (int)debuff[1] - 1;
            AttackP = (int)debuff[2];
            RandomPercentage[0] = (int)debuff[3];
            RandomPercentage[1] = (int)debuff[4];

        }
        //Тут происходит выбор процента от нанесённого урона
        int Percentage = rnd1.Next(RandomPercentage[0], RandomPercentage[1]);

        //Переодическое нанесение урона
        int DamageDebuff = Convert.ToInt32((float)AttackP / (float)100 * (float)Percentage);
        for (int time = timeEffect; time > 0; time--)
        {
            if (onEDN)
            {
                UnityEngine.Debug.Log($"onEDN SSS == {onEDN.ToString()}");
                break;
            }
            // UnityEngine.Debug.Log($"onEDN == {onEDN.ToString()}");
            AmountOfHealthCurrent -= DamageDebuff;
            Debug.Log($"DamageDebuff = {DamageDebuff}");
            //UnityEngine.Debug.Log("Дебаф: У " + transform.name + " takes " + DamageDebuff + $"{(ElementPower)numElement+1} damage " + AmountOfHealthCurrent+". Осталось "+ time + " сек.");
            if (AmountOfHealthCurrent <= 0)
            {

                if (OnHealthReachedZero != null)
                {
                    OnHealthReachedZero();
                }
            }
            yield return new WaitForSeconds(1);
        }

        onEDN = true;

    }

    public IEnumerator StanCoroutine(object? deb)
    {
        //количество секунд что будет длиться дебаф
        int timeEffect = 1;
        
        if (deb is object[] debuff)
        {
            timeEffect = (int)debuff[0];
        }
       
        
        Enemy thisEnemy = GetComponent<Enemy>();
        navmeshAgent.speed = (float)0.00001;
        thisEnemy.StanEnemy = true;

        for (int time = timeEffect; time > 0; time--)
        {
            yield return new WaitForSeconds(1);
           // UnityEngine.Debug.Log("Дебаф:  У " + transform.name + " Оглушение  " + $"от злемента {(ElementPowerO)numElement + 1} будет ещё {time} с.");



        }
        navmeshAgent.speed = CalculationCurrentSpeed();
        thisEnemy.StanEnemy = false;
        GetComponentInChildren<EnemyAnim>().nextRunOn = true;

        onES = true;

    }

    public void PeriodicElementalDamage(int TEWE, int ElementD, int finalDamage, int[] RandomPercentage, int EDC)
    {
        //Шанс наложить негативный эффект(Пока что расчёт наложение только одного негативного эффекта за раз)

        if (UnityEngine.Random.Range(1, 100) <= EDC && ElementD != 0)
        {
            if (onEDN)
            {
                UnityEngine.Debug.Log("Накладываеться Стихийный Эффект на " + transform.name);
                onEDN = false;
                object[] debuff = { TEWE, ElementD, finalDamage, RandomPercentage[0], RandomPercentage[1] };
                EDN = StartCoroutine(ElementalsNegativeEffect(debuff));
            }
        }
        
    }

    public void StanToEnemy(int TEWE, int EDC)
    {
        //Шанс наложить негативный эффект(Пока что расчёт наложение только одного негативного эффекта за раз)

        if (UnityEngine.Random.Range(1, 100) <= EDC)
        {
            if (onES)
            {
                UnityEngine.Debug.Log("Накладываеться Стан на " + transform.name);
                onES = false;
                object[] debuff = { TEWE };
                ES = StartCoroutine(StanCoroutine(debuff));
            }
        }
    }

    public int TakeNormalDamage(int lvl, int AttackPower, int ElementD, int AdditionalInterest, int AccuracyEnemy)
    {

        //Начальный урон (чистый урон + (чистый урон / 100% * (Дополнительный процент от Елемента и вида врага - Процент сопратевляимости нанесённого вида урона)))
        float initialDamage = (float)AttackPower + (float)AttackPower / (float)100 * ((float)AdditionalInterest - (float)ElementResistance[ElementD].GetValue());


        //Финальный урон (Начальный урон * (Начальный урон / (Начальный урон + количество защиты)))
        int finalDamage = Convert.ToInt32((float)initialDamage * (float)(initialDamage / ((float)initialDamage + (float)CalculationCurrentArmor())));


        //Выбор расчёта Попадания с учётом разницы уровней
        if (lvl < currentLvl - 5)
        {
            // UnityEngine.Debug.Log(" Финальный урон " + finalDamage);
            if (UnityEngine.Random.Range(1, 100) <= 5)
            {
                // Вычесть урон из здоровья
                AmountOfHealthCurrent -= finalDamage;
               


                //проверка оставшегося HP, если 0 то запустить ивент смерть персонажа
                if (AmountOfHealthCurrent <= 0)
                {

                    if (OnHealthReachedZero != null)
                    {
                        OnHealthReachedZero();
                    }
                }
                if (TextHP != null && TextMP != null)
                {
                    TextHHp();
                    TextMMp();
                }
                return finalDamage;
            }
            return 0;
        }
        else
        {

            if (UnityEngine.Random.Range(1, 100) <= (100 / Dodge.GetValue() * AccuracyEnemy))
            {
                //UnityEngine.Debug.Log(" Финальный урон " + finalDamage);
                // Вычесть урон из здоровья
                AmountOfHealthCurrent -= finalDamage;
               


                //проверка оставшегося HP, если 0 то запустить ивент смерть персонажа
                if (AmountOfHealthCurrent <= 0)
                {

                    if (OnHealthReachedZero != null)
                    {
                        OnHealthReachedZero();
                    }
                }
                if (TextHP != null && TextMP != null)
                {
                    TextHHp();
                    TextMMp();
                }
                return finalDamage;
            }

            return 0;
        }

    }


}
