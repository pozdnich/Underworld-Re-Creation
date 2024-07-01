using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EquipmentManager;


/*
	This component is derived from CharacterStats. It adds two things:
		- Gaining modifiers when equipping items
		- Restarting the game when dying
Этот компонент является производным от CharacterStats. Он добавляет две вещи:
- Получение модификаторов при экипировке предметов
- Перезапуск игры при смерти
*/

public class PlayerStats : CharStats{

    // Use this for initialization // Используйте это для инициализации
    public GameObject HealthImage;
    public GameObject ManaImage;

    public event System.Action OnHealthReachedZero;

    public bool OnOffCombat = false;

    public int maxLvl = 10;

    public int maxExperience = 10;
    public int currentExperience = 0;

    public int skillPoints = 0;
    public int abilityPoints = 0;

    

    bool ChangeStat=false;

    public float TimeHealthRecovery = 0;
    public float TimeManaRegeneration = 0;
    public override void Start () {

		base.Start();
        AnimationControl = GetComponent<PlayerAnim>().animator;
        AnimationControl.speed = CalculationCurrentAttackSpeed();
       
        
    }


    public override void Update()
	{
		if(currentExperience >= maxExperience)
		{
			if(currentLvl!= maxLvl)
			{
                currentLvl++;
				currentExperience = currentExperience - maxExperience;
                maxExperience = maxExperience + maxExperience / 2;
				skillPoints+=3;
                abilityPoints += 1;
            }
			
        }

        if (TextHP != null && TextMP != null)
        {
            if(AmountOfHealthCurrent < CalculationCurrentAmountOfHealthMAX())
            {
                if (TimeHealthRecovery <= 0)
                {
                    if((AmountOfHealthCurrent + HealthRecovery.GetValue())> CalculationCurrentAmountOfHealthMAX())
                    {
                        AmountOfHealthCurrent += (CalculationCurrentAmountOfHealthMAX()- AmountOfHealthCurrent);
                        if (HealthImage != null)
                        {
                            HealthImage.GetComponent<Image>().fillAmount += 1.0f;
                        }
                    }
                    else
                    {
                        AmountOfHealthCurrent += HealthRecovery.GetValue();
                        if (HealthImage != null)
                        {
                            HealthImage.GetComponent<Image>().fillAmount += 1.0f / (float)CalculationCurrentAmountOfHealthMAX() * (float)HealthRecovery.GetValue();
                        }
                    }
                 

                    TextHHp();
                    TextMMp();
                    TimeHealthRecovery = 3;
                }
                else
                {
                    TimeHealthRecovery -= Time.deltaTime;
                }
            }
            if (AmounOfManaCurrent < CalculationCurrentAmountOfManaMAX())
            {
                if (TimeHealthRecovery <= 0)
                {
                    if ((AmounOfManaCurrent + ManaRegeneration.GetValue()) > CalculationCurrentAmountOfManaMAX())
                    {
                        AmounOfManaCurrent += (CalculationCurrentAmountOfManaMAX() - AmounOfManaCurrent);
                        if (ManaImage != null)
                        {
                            ManaImage.GetComponent<Image>().fillAmount = 1.0f ;
                        }
                    }
                    else
                    {
                        AmounOfManaCurrent += ManaRegeneration.GetValue();
                        if (ManaImage != null)
                        {
                            ManaImage.GetComponent<Image>().fillAmount += 1.0f / (float)CalculationCurrentAmountOfManaMAX() * (float)ManaRegeneration.GetValue();
                        }
                    }
                    

                    TextHHp();
                    TextMMp();
                    TimeManaRegeneration = 3;
                }
                else
                {
                    TimeManaRegeneration -= Time.deltaTime;
                }
            }


            
        }

        if (ChangeStat)
        {
            TextHHp();
            TextMMp();

            ChangeStat = false;
        }
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
        int[] RandomPercentage = { 1, 2 };
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
                if (HealthImage != null)
                {
                    HealthImage.GetComponent<Image>().fillAmount -= 1.0f / CalculationCurrentAmountOfHealthMAX() * finalDamage;
                }
               

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
                if (HealthImage != null)
                {
                    HealthImage.GetComponent<Image>().fillAmount -= 1.0f / CalculationCurrentAmountOfHealthMAX() * finalDamage;
                }
               

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

    //void OnArmorChanged(Armor newItem, Armor oldItem)
    //{
    //    if (newItem != null)
    //    {
    //        float CofH = (float)100 / (float)CalculationCurrentAmountOfHealthMAX() * (float)AmountOfHealthCurrent;
    //        float CofM = (float)100 / (float)CalculationCurrentAmountOfManaMAX() * (float)AmounOfManaCurrent;

    //        armorEquip.AddModifier(newItem.ArmorPowerEquipment);
    //        ElementResistance[(int)newItem.ElArmor].AddModifier(newItem.ElementСoefficient);
    //        for (int i = 0; i < StatsValuesArmor.Count; i++)
    //        {
    //            if (newItem.StatsAccess[i])
    //            {
    //                StatsValuesArmor[i].AddModifier(newItem.StatsValues[i]);
    //            }

    //        }

            
    //        AmountOfHealthCurrent = Convert.ToInt32((float)CalculationCurrentAmountOfHealthMAX() / (float)100 * CofH);
    //        AmounOfManaCurrent = Convert.ToInt32((float)CalculationCurrentAmountOfManaMAX() / (float)100 * CofM);
    //        Debug.Log("Здоровье " + AmountOfHealthCurrent);

    //        ChangeStat = true;
    //    }

    //    if (oldItem != null)
    //    {
    //        float CofH = (float)100 / (float)CalculationCurrentAmountOfHealthMAX() * (float)AmountOfHealthCurrent;
    //        float CofM = (float)100 / (float)CalculationCurrentAmountOfManaMAX() * (float)AmounOfManaCurrent;


    //        armorEquip.RemoveModifier(oldItem.ArmorPowerEquipment);
    //        ElementResistance[(int)oldItem.ElArmor].RemoveModifier(oldItem.ElementСoefficient);
    //        for (int i = 0; i < StatsValuesArmor.Count; i++)
    //        {
    //            if (oldItem.StatsAccess[i])
    //            {
    //                StatsValuesArmor[i].RemoveModifier(oldItem.StatsValues[i]);
    //            }

    //        }

           
    //        AmountOfHealthCurrent = Convert.ToInt32((float)CalculationCurrentAmountOfHealthMAX() / (float)100 * CofH);
    //        AmounOfManaCurrent = Convert.ToInt32((float)CalculationCurrentAmountOfManaMAX() / (float)100 * CofM);

    //        ChangeStat = true;
    //    }

    //}

    //void OnWeaponChanged(Weapon newItem, Weapon oldItem)
    //{
    //    if (newItem != null)
    //    {
    //        AttackPowerEquip.AddModifier(newItem.AttackPowerEquipment);
    //        ElementPowerCharacter = newItem.ElWeapon;
    //        ElementalDamage[(int)newItem.ElWeapon].AddModifier(newItem.ElementСoefficient);
    //        for (int i = 0;i< StatsValuesWeapon.Count; i++)
    //        {
    //            if (newItem.StatsAccess[i])
    //            {
    //                StatsValuesWeapon[i].AddModifier(newItem.StatsValues[i]);
    //            }

    //        }

    //    }

    //    if (oldItem != null)
    //    {
    //        AttackPowerEquip.RemoveModifier(oldItem.AttackPowerEquipment);
    //        ElementPowerCharacter = 0;
    //        ElementalDamage[(int)oldItem.ElWeapon].RemoveModifier(oldItem.ElementСoefficient);
    //        for (int i = 0; i < StatsValuesWeapon.Count; i++)
    //        {
    //            if (oldItem.StatsAccess[i])
    //            {
    //                StatsValuesWeapon[i].RemoveModifier(oldItem.StatsValues[i]);
    //            }

    //        }
           
    //    }

    //}

    //void OnSecondaryWeaponChanged(SecondaryWeapon newItem, SecondaryWeapon oldItem)
    //{
    //    if (newItem != null)
    //    {

    //        ElementResistance[(int)newItem.ElArmor].AddModifier(newItem.ElementСoefficientArmor);
    //        ElementalDamage[(int)newItem.ElWeapon].AddModifier(newItem.ElementСoefficientWeapon);
    //        for (int i = 0; i < StatsValuesWeapon.Count; i++)
    //        {
    //            if (newItem.StatsAccess[i])
    //            {
    //                StatsValuesWeapon[i].AddModifier(newItem.StatsValues[i]);
    //            }

    //        }

    //    }

    //    if (oldItem != null)
    //    {

    //        ElementResistance[(int)newItem.ElArmor].RemoveModifier(newItem.ElementСoefficientArmor);
    //        ElementalDamage[(int)newItem.ElWeapon].RemoveModifier(newItem.ElementСoefficientWeapon);
          
    //        for (int i = 0; i < StatsValuesWeapon.Count; i++)
    //        {
    //            if (oldItem.StatsAccess[i])
    //            {
    //                StatsValuesSecondaryWeapon[i].RemoveModifier(oldItem.StatsValues[i]);
    //            }

    //        }

    //    }

    //}


    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        //if (newItem != null)
        //{
        //    armor.AddModifier(newItem.armorModifier);
        //    damage.AddModifier(newItem.damageModifier);
        //}

        //if (oldItem != null)
        //{
        //    armor.RemoveModifier(oldItem.armorModifier);
        //    damage.RemoveModifier(oldItem.damageModifier);
        //}

    }



}
