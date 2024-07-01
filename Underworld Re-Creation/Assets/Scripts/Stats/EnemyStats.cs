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
        //�������� ���������� � ������ � ��� �������������� ������ �� object
        System.Random rnd1 = new System.Random();
        //������ ��������� ����
        int AttackP = 1;
        //���������� ������ ��� ����� ������� �����
        int timeEffect = 1;
        //����� ��� �������� ������� ����
        int numElement = 1;
        //�� ������� �� ������� ����� ���� ������� �� AttackP
        int[] RandomPercentage = new int[2];
        if (deb is object[] debuff)
        {
            timeEffect = (int)debuff[0];
            numElement = (int)debuff[1] - 1;
            AttackP = (int)debuff[2];
            RandomPercentage[0] = (int)debuff[3];
            RandomPercentage[1] = (int)debuff[4];

        }
        //��� ���������� ����� �������� �� ���������� �����
        int Percentage = rnd1.Next(RandomPercentage[0], RandomPercentage[1]);

        //������������� ��������� �����
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
            //UnityEngine.Debug.Log("�����: � " + transform.name + " takes " + DamageDebuff + $"{(ElementPower)numElement+1} damage " + AmountOfHealthCurrent+". �������� "+ time + " ���.");
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
        //���������� ������ ��� ����� ������� �����
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
           // UnityEngine.Debug.Log("�����:  � " + transform.name + " ���������  " + $"�� �������� {(ElementPowerO)numElement + 1} ����� ��� {time} �.");



        }
        navmeshAgent.speed = CalculationCurrentSpeed();
        thisEnemy.StanEnemy = false;
        GetComponentInChildren<EnemyAnim>().nextRunOn = true;

        onES = true;

    }

    public void PeriodicElementalDamage(int TEWE, int ElementD, int finalDamage, int[] RandomPercentage, int EDC)
    {
        //���� �������� ���������� ������(���� ��� ������ ��������� ������ ������ ����������� ������� �� ���)

        if (UnityEngine.Random.Range(1, 100) <= EDC && ElementD != 0)
        {
            if (onEDN)
            {
                UnityEngine.Debug.Log("�������������� ��������� ������ �� " + transform.name);
                onEDN = false;
                object[] debuff = { TEWE, ElementD, finalDamage, RandomPercentage[0], RandomPercentage[1] };
                EDN = StartCoroutine(ElementalsNegativeEffect(debuff));
            }
        }
        
    }

    public void StanToEnemy(int TEWE, int EDC)
    {
        //���� �������� ���������� ������(���� ��� ������ ��������� ������ ������ ����������� ������� �� ���)

        if (UnityEngine.Random.Range(1, 100) <= EDC)
        {
            if (onES)
            {
                UnityEngine.Debug.Log("�������������� ���� �� " + transform.name);
                onES = false;
                object[] debuff = { TEWE };
                ES = StartCoroutine(StanCoroutine(debuff));
            }
        }
    }

    public int TakeNormalDamage(int lvl, int AttackPower, int ElementD, int AdditionalInterest, int AccuracyEnemy)
    {

        //��������� ���� (������ ���� + (������ ���� / 100% * (�������������� ������� �� �������� � ���� ����� - ������� ���������������� ���������� ���� �����)))
        float initialDamage = (float)AttackPower + (float)AttackPower / (float)100 * ((float)AdditionalInterest - (float)ElementResistance[ElementD].GetValue());


        //��������� ���� (��������� ���� * (��������� ���� / (��������� ���� + ���������� ������)))
        int finalDamage = Convert.ToInt32((float)initialDamage * (float)(initialDamage / ((float)initialDamage + (float)CalculationCurrentArmor())));


        //����� ������� ��������� � ������ ������� �������
        if (lvl < currentLvl - 5)
        {
            // UnityEngine.Debug.Log(" ��������� ���� " + finalDamage);
            if (UnityEngine.Random.Range(1, 100) <= 5)
            {
                // ������� ���� �� ��������
                AmountOfHealthCurrent -= finalDamage;
               


                //�������� ����������� HP, ���� 0 �� ��������� ����� ������ ���������
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
                //UnityEngine.Debug.Log(" ��������� ���� " + finalDamage);
                // ������� ���� �� ��������
                AmountOfHealthCurrent -= finalDamage;
               


                //�������� ����������� HP, ���� 0 �� ��������� ����� ������ ���������
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
