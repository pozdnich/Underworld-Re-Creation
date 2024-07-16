using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharStats : MonoBehaviour
{
    public int currentLvl = 0;


    
    public Animator AnimationControl;
    //------------------------------------------------------------------------------

    //public delegate void EventNegativeEffect();
    //public event EventNegativeEffect eventNegativeEffect;
    public NavMeshAgent navmeshAgent;

    public GameObject TextHP;
   

    public GameObject TextMP;

    public GameObject AttackPowerText;
    public GameObject ArmorText;
    public GameObject ForceText;
    public GameObject DexterityText;
    public GameObject IntelligenceText;
    public GameObject VitalityText;
    public GameObject LuckText;

    public ElementPower ElementPowerCharacter;


    //--------------------------------------Здоровье и мана-----------------------------------------------------
    public int DfaultAmountOfHealthMAX;                              //Дефолтное  количество здоровья
    public int AmountOfHealthCurrent;                              //Текущее количество здоровья

    public int DfaultAmounOfManaMAX;                            //Дефолтное  количество маны
    public int AmounOfManaCurrent;                               //Текущее количество маны

    //--------------------------------------Основные статы-----------------------------------------------------
    public Stat Force;         //Сила
    public Stat Intelligence;  //Интеллект

    string nameClasStat;
    public Stat ClasStat;      //Переменная принимающая в себя основной стат персонажа

    public Stat Agility;       //Ловкость  
    public Stat Vitality;      //Живучесть
    public Stat Luck;          //Удача

    //---------------------------------------Динамические статы---------------------------

    //основной стат персонажа

    public Stat IncreasedAttackPowerFromStrength;                       //прирост Сила атаки от Силы (Для Физической)
    public int DfaultAttackPowerFromStrength;                          //Сила атаки по умолчании (Для Физической)

    public Stat IncreasedAttackPowerFromIntelligence;                       //прирост Сила атаки от Интелект (Для Магической)
    public int DfaultAttackPowerFromIntelligence;                          //Сила атаки по умолчании (Для Магической)
    //Ловкость 
    public Stat AbilityСastingSpeed;         //Процент скорости атаки
    public float DfaultAttackSpeed;            //Скорость атаки по умолчании
    public Stat Dodge;                       //уклонения

    //Живучесть
    public Stat AmountOfHealth;                              //Количество здоровья прибавиться к максимальному значению
    public Stat AmountOfMana;                                //Количество маны прибавиться к максимальному значению
    public Stat CarriedWeight;                               //Переносимый вес

    //Удача
    public Stat CriticalStrikeChance;                              //Шанс критического удара
    public Stat Accuracy;                                          //Попадание

    //-----------------------Статы зависищие полностью от шмоток----------------------------------

    public Stat AmountOfHealthBonus;          //Коэфициэнт прибавления к МАX здоровью

    public Stat AmountOfManaBonus;          //Коэфициэнт прибавления к МАX Маны


    public Stat AttackPowerEquip;                   //Сила атаки от оружия


    public Stat HealthRecovery;                    // Восстановление здоровья  
    public Stat ManaRegeneration;                  //Восстановление маны

    public Stat CriticalHitPercentage;       //Процент Критического удара
    public Stat ElementalDebuffChance;       //Шанс наложения стихийного дебафа

    public Stat armorEquip;                              // количество защиты от шмоток
    public Stat armorEquipBonus;                         // Коэффициент защиты от бонусов в шмотоках(если есть)

    public int DfaultArmor;                         // количество защиты по умолчанию

    public Stat MovementFactor;              // Коэффициент передвижения
    public int DfaultMovementSpeed;              //  скорость передвежения по умолчанию
    public int timeDE;
    


    public List<Stat> ElementalDamage;
    //Элементальный урон
    public Stat PhysicalA;      //Без элемента
    public Stat FireA;          //Огонь
    public Stat EarthA;         //Земля
    public Stat AirA;           //Воздух
    public Stat WaterA;         //Вода
    public Stat HolyA;          //Святая
    public Stat DarkA;          //Тёмная
    public Stat VenomA;         //Яд

    public List<Stat> ElementResistance;
    //Уязвимость к Элементам
    public Stat PhysicalG;      //Без элемента
    public Stat FireG;          //Огонь
    public Stat EarthG;         //Земля
    public Stat AirG;           //Воздух
    public Stat WaterG;         //Вода
    public Stat HolyG;          //Святая
    public Stat DarkG;          //Тёмная
    public Stat VenomG;         //Яд


      
    public bool onEDN = true;          //Показатель активности навешеного временого негативного стихийного эффекта
    public Coroutine EDN;
    public bool onES = true;          //Показатель активности навешеного временого негативного стихийного эффекта
    public Coroutine ES;

    public List<Stat> StatsValues;
    //public List<Stat> StatsValuesWeapon;
    //public List<Stat> StatsValuesArmor;
    //public List<Stat> StatsValuesSecondaryWeapon;
    // Start with max HP. Начните с максимального HP.
    public virtual void Awake()
    {
        AmountOfHealthCurrent = CalculationCurrentAmountOfHealthMAX();
        AmounOfManaCurrent = CalculationCurrentAmountOfManaMAX();
    }

    //метод для постоянного показания (текущего здоровья/максимального здоровья)
    public void TextHHp()
    {

        TextHP.GetComponent<Text>().text = $"{AmountOfHealthCurrent}/{CalculationCurrentAmountOfHealthMAX()}";


    }
   

    //метод для постоянного показания (текущего здоровья/максимального здоровья)
    public void TextMMp()
    {

        TextMP.GetComponent<Text>().text = $"{AmounOfManaCurrent}/{CalculationCurrentAmountOfManaMAX()}";


    }
   
    public virtual void Start()
    {
        
        
        //Выбор основного атрибута в зависимости от класса персонажа или если это Enemy
        //switch ((int)classCharacter)
        //{
        //    case 0:
        //        nameClasStat = "Сила";
        //        ClasStat = Force;
               
        //        break;
        //    case 1:
        //        nameClasStat = "Сила";
        //        ClasStat = Force;
               
        //        break;
        //    case 2:
        //        nameClasStat = "Сила";
        //        ClasStat = Force;
        //        break;
        //    case 3:
        //        nameClasStat = "Интелект";
        //        ClasStat = Intelligence;
        //        break;


        //}

        //Увелечение коэффициента прироста силы атаки на соответствующее количество Основной характеристики Силы
        for (int i = 1; i <= Force.GetValue(); i++)
        {
            IncreasedAttackPowerFromStrength.AddModifier(1);
        }
        //Увелечение коэффициента прироста силы атаки на соответствующее количество Основной характеристики Интеллект
        for (int i = 1; i <= Intelligence.GetValue(); i++)
        {
            IncreasedAttackPowerFromIntelligence.AddModifier(1);
        }
        //Увелечение коэффициента прироста скорости атаки на соответствующее количество Основной характеристики ловкости
        for (int i = 1; i <= Agility.GetValue(); i++)
        {
            AbilityСastingSpeed.AddModifier(1);
            Dodge.AddModifier(1);
        }
        //Увелечение коэффициента прироста попадания на соответствующее количество Основной характеристики живучесть
        for (int i = 1; i <= Vitality.GetValue(); i++)
        {
            AmountOfHealth.AddModifier(2);
            AmountOfMana.AddModifier(1);

        }
        //Увелечение коэффициента прироста попадания на соответствующее количество Основной характеристики удача
        for (int i = 1; i <= Luck.GetValue(); i++)
        {
            Accuracy.AddModifier(1);

        }

        //Присвоение для манипуляции со скоростью бега
        navmeshAgent = GetComponent<NavMeshAgent>();
        //navmeshAgent.speed = CalculationCurrentSpeed();


        if (TextHP != null && TextMP != null)
        {
            TextHHp();
            TextMMp();
        }
       

        //Добавление в единый список Стихийного урона
        ElementalDamage.Add(PhysicalA);
        ElementalDamage.Add(FireA);
        ElementalDamage.Add(EarthA);
        ElementalDamage.Add(AirA);
        ElementalDamage.Add(WaterA);
        ElementalDamage.Add(HolyA);
        ElementalDamage.Add(DarkA);
        ElementalDamage.Add(VenomA);

        //Добавление в единый список Стихийного сопротивления
        ElementResistance.Add(PhysicalG);
        ElementResistance.Add(FireG);
        ElementResistance.Add(EarthG);
        ElementResistance.Add(AirG);
        ElementResistance.Add(WaterG);
        ElementResistance.Add(HolyG);
        ElementResistance.Add(DarkG);
        ElementResistance.Add(VenomG);


        //Добавление указателей на статы которые модифицируються при одевании экиперовки Weapon
        //StatsValuesWeapon.Add(Force);
        //StatsValuesWeapon.Add(Intelligence);
        //StatsValuesWeapon.Add(Agility);
        //StatsValuesWeapon.Add(Luck);
        //StatsValuesWeapon.Add(AbilityСastingSpeed);
        //StatsValuesWeapon.Add(Accuracy);
        //StatsValuesWeapon.Add(CriticalHitPercentage);

        //Force, Intelligence, Agility, Vitality, armorEquipBonus, HealthRecovery, ManaRegeneration, AmountOfHealthBonus, AmountOfManaBonus, AmountOfHealth, AmountOfMana, Dodge, Luck, AbilityСastingSpeed, Accuracy, CriticalHitPercentage
        //Добавление указателей на статы которые модифицируються при одевании экиперовки Weapon
        StatsValues.Add(Force);
        StatsValues.Add(Intelligence);
        StatsValues.Add(Agility);
        StatsValues.Add(Vitality);
        StatsValues.Add(armorEquipBonus);
        StatsValues.Add(HealthRecovery);
        StatsValues.Add(ManaRegeneration);
        StatsValues.Add(AmountOfHealthBonus);
        StatsValues.Add(AmountOfManaBonus);
        StatsValues.Add(AmountOfHealth);
        StatsValues.Add(AmountOfMana);
        StatsValues.Add(Dodge);
        StatsValues.Add(Luck);
        StatsValues.Add(AbilityСastingSpeed);
        StatsValues.Add(Accuracy);
        StatsValues.Add(CriticalHitPercentage);
        

        //Добавление указателей на статы которые модифицируються при одевании экиперовки Armor
        //StatsValuesArmor.Add(Force);
        //StatsValuesArmor.Add(Intelligence);
        //StatsValuesArmor.Add(Agility);
        //StatsValuesArmor.Add(Vitality);
        //StatsValuesArmor.Add(armorEquipBonus);
        //StatsValuesArmor.Add(HealthRecovery);
        //StatsValuesArmor.Add(ManaRegeneration);
        //StatsValuesArmor.Add(AmountOfHealthBonus);
        //StatsValuesArmor.Add(AmountOfManaBonus);
        //StatsValuesArmor.Add(AmountOfHealth);
        //StatsValuesArmor.Add(AmountOfMana);
        //StatsValuesArmor.Add(Dodge);

        //Agility, Vitality, Luck, AbilityСastingSpeed, AmountOfMana, CarriedWeight, CriticalStrikeChance, HealthRecovery, ManaRegeneration , ElementalDebuffChance
        //Добавление указателей на статы которые модифицируються при одевании экиперовки SecondaryWeapon
        //StatsValuesSecondaryWeapon.Add(Agility);
        //StatsValuesSecondaryWeapon.Add(Vitality);
        //StatsValuesSecondaryWeapon.Add(Luck);
        //StatsValuesSecondaryWeapon.Add(AbilityСastingSpeed);
        //StatsValuesSecondaryWeapon.Add(AmountOfMana);
        //StatsValuesSecondaryWeapon.Add(CarriedWeight);
        //StatsValuesSecondaryWeapon.Add(CriticalStrikeChance);
        //StatsValuesSecondaryWeapon.Add(HealthRecovery);
        //StatsValuesSecondaryWeapon.Add(ManaRegeneration);
        //StatsValuesSecondaryWeapon.Add(ElementalDebuffChance);

    }

    public virtual void Update()
    {

    }

    //Вычесление текущей силы атаки от статы Сила
    public int CalculationCurrentAttackPowerFromStrength()
    {
        IncreasedAttackPowerFromStrength.modifiers.Clear();
        for (int i = 1; i <= ClasStat.GetValue(); i++)
        {
            IncreasedAttackPowerFromStrength.AddModifier(1);
        }
        return Convert.ToInt32(((float)DfaultAttackPowerFromStrength + (float)AttackPowerEquip.GetValue()) + ((float)DfaultAttackPowerFromStrength + (float)AttackPowerEquip.GetValue()) / (float)100 * (float)IncreasedAttackPowerFromStrength.GetValue());
    }

    //Вычесление текущей силы атаки от статы Интелект
    public int CalculationCurrentAttackPowerIncreasedAttackPowerFromIntelligence()
    {
        IncreasedAttackPowerFromIntelligence.modifiers.Clear();
        for (int i = 1; i <= ClasStat.GetValue(); i++)
        {
            IncreasedAttackPowerFromIntelligence.AddModifier(1);
        }
        return Convert.ToInt32(((float)DfaultAttackPowerFromIntelligence + (float)AttackPowerEquip.GetValue()) + ((float)DfaultAttackPowerFromIntelligence + (float)AttackPowerEquip.GetValue()) / (float)100 * (float)IncreasedAttackPowerFromIntelligence.GetValue());
    }

    //Вычесление текущего количества брони
    public int CalculationCurrentArmor()
    {
        return Convert.ToInt32(((float)DfaultArmor + (float)armorEquip.GetValue()) + ((float)DfaultArmor + (float)armorEquip.GetValue()) / (float)100 * (float)armorEquipBonus.GetValue());
    }
    //Вычесление текущей скорости атаки
    public float CalculationCurrentAttackSpeed()
    {


        return ((DfaultAttackSpeed + DfaultAttackSpeed / (float)100 * (float)AbilityСastingSpeed.GetValue()));
    }
    //Вычесление текущей скорости передвежения
    public float CalculationCurrentSpeed()
    {
        return (DfaultMovementSpeed + DfaultMovementSpeed / 100 * MovementFactor.GetValue());
    }
    //Вычесление Максимального количества здоровья
    public int CalculationCurrentAmountOfHealthMAX()
    {
        AmountOfHealth.modifiers.Clear();
        for (int i = 1; i <= Vitality.GetValue(); i++)
        {
            AmountOfHealth.AddModifier(2);

        }

        return Convert.ToInt32(((float)DfaultAmountOfHealthMAX + (float)AmountOfHealth.GetValue()) + ((float)DfaultAmountOfHealthMAX + (float)(float)AmountOfHealth.GetValue()) / (float)100 * (float)AmountOfHealthBonus.GetValue());
    }

    //Вычесление Максимального количества Маны
    public int CalculationCurrentAmountOfManaMAX()
    {
        AmountOfMana.modifiers.Clear();
        for (int i = 1; i <= Vitality.GetValue(); i++)
        {
            AmountOfMana.AddModifier(1);

        }
        return Convert.ToInt32(((float)DfaultAmounOfManaMAX + (float)AmountOfMana.GetValue()) + ((float)DfaultAmounOfManaMAX + (float)AmountOfMana.GetValue()) / 100 * (float)AmountOfManaBonus.GetValue());
    }
   
}

public enum ElementPower { Physical, Fire, Earth, Air, Water, Holy, Dark, Venom }


