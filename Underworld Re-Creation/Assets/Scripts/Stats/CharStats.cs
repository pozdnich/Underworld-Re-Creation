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


    //--------------------------------------�������� � ����-----------------------------------------------------
    public int DfaultAmountOfHealthMAX;                              //���������  ���������� ��������
    public int AmountOfHealthCurrent;                              //������� ���������� ��������

    public int DfaultAmounOfManaMAX;                            //���������  ���������� ����
    public int AmounOfManaCurrent;                               //������� ���������� ����

    //--------------------------------------�������� �����-----------------------------------------------------
    public Stat Force;         //����
    public Stat Intelligence;  //���������

    string nameClasStat;
    public Stat ClasStat;      //���������� ����������� � ���� �������� ���� ���������

    public Stat Agility;       //��������  
    public Stat Vitality;      //���������
    public Stat Luck;          //�����

    //---------------------------------------������������ �����---------------------------

    //�������� ���� ���������

    public Stat IncreasedAttackPowerFromStrength;                       //������� ���� ����� �� ���� (��� ����������)
    public int DfaultAttackPowerFromStrength;                          //���� ����� �� ��������� (��� ����������)

    public Stat IncreasedAttackPowerFromIntelligence;                       //������� ���� ����� �� �������� (��� ����������)
    public int DfaultAttackPowerFromIntelligence;                          //���� ����� �� ��������� (��� ����������)
    //�������� 
    public Stat Ability�astingSpeed;         //������� �������� �����
    public float DfaultAttackSpeed;            //�������� ����� �� ���������
    public Stat Dodge;                       //���������

    //���������
    public Stat AmountOfHealth;                              //���������� �������� ����������� � ������������� ��������
    public Stat AmountOfMana;                                //���������� ���� ����������� � ������������� ��������
    public Stat CarriedWeight;                               //����������� ���

    //�����
    public Stat CriticalStrikeChance;                              //���� ������������ �����
    public Stat Accuracy;                                          //���������

    //-----------------------����� ��������� ��������� �� ������----------------------------------

    public Stat AmountOfHealthBonus;          //���������� ����������� � ��X ��������

    public Stat AmountOfManaBonus;          //���������� ����������� � ��X ����


    public Stat AttackPowerEquip;                   //���� ����� �� ������


    public Stat HealthRecovery;                    // �������������� ��������  
    public Stat ManaRegeneration;                  //�������������� ����

    public Stat CriticalHitPercentage;       //������� ������������ �����
    public Stat ElementalDebuffChance;       //���� ��������� ���������� ������

    public Stat armorEquip;                              // ���������� ������ �� ������
    public Stat armorEquipBonus;                         // ����������� ������ �� ������� � ��������(���� ����)

    public int DfaultArmor;                         // ���������� ������ �� ���������

    public Stat MovementFactor;              // ����������� ������������
    public int DfaultMovementSpeed;              //  �������� ������������ �� ���������
    public int timeDE;
    


    public List<Stat> ElementalDamage;
    //������������� ����
    public Stat PhysicalA;      //��� ��������
    public Stat FireA;          //�����
    public Stat EarthA;         //�����
    public Stat AirA;           //������
    public Stat WaterA;         //����
    public Stat HolyA;          //������
    public Stat DarkA;          //Ҹ����
    public Stat VenomA;         //��

    public List<Stat> ElementResistance;
    //���������� � ���������
    public Stat PhysicalG;      //��� ��������
    public Stat FireG;          //�����
    public Stat EarthG;         //�����
    public Stat AirG;           //������
    public Stat WaterG;         //����
    public Stat HolyG;          //������
    public Stat DarkG;          //Ҹ����
    public Stat VenomG;         //��


      
    public bool onEDN = true;          //���������� ���������� ���������� ��������� ����������� ���������� �������
    public Coroutine EDN;
    public bool onES = true;          //���������� ���������� ���������� ��������� ����������� ���������� �������
    public Coroutine ES;

    public List<Stat> StatsValues;
    //public List<Stat> StatsValuesWeapon;
    //public List<Stat> StatsValuesArmor;
    //public List<Stat> StatsValuesSecondaryWeapon;
    // Start with max HP. ������� � ������������� HP.
    public virtual void Awake()
    {
        AmountOfHealthCurrent = CalculationCurrentAmountOfHealthMAX();
        AmounOfManaCurrent = CalculationCurrentAmountOfManaMAX();
    }

    //����� ��� ����������� ��������� (�������� ��������/������������� ��������)
    public void TextHHp()
    {

        TextHP.GetComponent<Text>().text = $"{AmountOfHealthCurrent}/{CalculationCurrentAmountOfHealthMAX()}";


    }
   

    //����� ��� ����������� ��������� (�������� ��������/������������� ��������)
    public void TextMMp()
    {

        TextMP.GetComponent<Text>().text = $"{AmounOfManaCurrent}/{CalculationCurrentAmountOfManaMAX()}";


    }
   
    public virtual void Start()
    {
        
        
        //����� ��������� �������� � ����������� �� ������ ��������� ��� ���� ��� Enemy
        //switch ((int)classCharacter)
        //{
        //    case 0:
        //        nameClasStat = "����";
        //        ClasStat = Force;
               
        //        break;
        //    case 1:
        //        nameClasStat = "����";
        //        ClasStat = Force;
               
        //        break;
        //    case 2:
        //        nameClasStat = "����";
        //        ClasStat = Force;
        //        break;
        //    case 3:
        //        nameClasStat = "��������";
        //        ClasStat = Intelligence;
        //        break;


        //}

        //���������� ������������ �������� ���� ����� �� ��������������� ���������� �������� �������������� ����
        for (int i = 1; i <= Force.GetValue(); i++)
        {
            IncreasedAttackPowerFromStrength.AddModifier(1);
        }
        //���������� ������������ �������� ���� ����� �� ��������������� ���������� �������� �������������� ���������
        for (int i = 1; i <= Intelligence.GetValue(); i++)
        {
            IncreasedAttackPowerFromIntelligence.AddModifier(1);
        }
        //���������� ������������ �������� �������� ����� �� ��������������� ���������� �������� �������������� ��������
        for (int i = 1; i <= Agility.GetValue(); i++)
        {
            Ability�astingSpeed.AddModifier(1);
            Dodge.AddModifier(1);
        }
        //���������� ������������ �������� ��������� �� ��������������� ���������� �������� �������������� ���������
        for (int i = 1; i <= Vitality.GetValue(); i++)
        {
            AmountOfHealth.AddModifier(2);
            AmountOfMana.AddModifier(1);

        }
        //���������� ������������ �������� ��������� �� ��������������� ���������� �������� �������������� �����
        for (int i = 1; i <= Luck.GetValue(); i++)
        {
            Accuracy.AddModifier(1);

        }

        //���������� ��� ����������� �� ��������� ����
        navmeshAgent = GetComponent<NavMeshAgent>();
        //navmeshAgent.speed = CalculationCurrentSpeed();


        if (TextHP != null && TextMP != null)
        {
            TextHHp();
            TextMMp();
        }
       

        //���������� � ������ ������ ���������� �����
        ElementalDamage.Add(PhysicalA);
        ElementalDamage.Add(FireA);
        ElementalDamage.Add(EarthA);
        ElementalDamage.Add(AirA);
        ElementalDamage.Add(WaterA);
        ElementalDamage.Add(HolyA);
        ElementalDamage.Add(DarkA);
        ElementalDamage.Add(VenomA);

        //���������� � ������ ������ ���������� �������������
        ElementResistance.Add(PhysicalG);
        ElementResistance.Add(FireG);
        ElementResistance.Add(EarthG);
        ElementResistance.Add(AirG);
        ElementResistance.Add(WaterG);
        ElementResistance.Add(HolyG);
        ElementResistance.Add(DarkG);
        ElementResistance.Add(VenomG);


        //���������� ���������� �� ����� ������� ��������������� ��� �������� ���������� Weapon
        //StatsValuesWeapon.Add(Force);
        //StatsValuesWeapon.Add(Intelligence);
        //StatsValuesWeapon.Add(Agility);
        //StatsValuesWeapon.Add(Luck);
        //StatsValuesWeapon.Add(Ability�astingSpeed);
        //StatsValuesWeapon.Add(Accuracy);
        //StatsValuesWeapon.Add(CriticalHitPercentage);

        //Force, Intelligence, Agility, Vitality, armorEquipBonus, HealthRecovery, ManaRegeneration, AmountOfHealthBonus, AmountOfManaBonus, AmountOfHealth, AmountOfMana, Dodge, Luck, Ability�astingSpeed, Accuracy, CriticalHitPercentage
        //���������� ���������� �� ����� ������� ��������������� ��� �������� ���������� Weapon
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
        StatsValues.Add(Ability�astingSpeed);
        StatsValues.Add(Accuracy);
        StatsValues.Add(CriticalHitPercentage);
        

        //���������� ���������� �� ����� ������� ��������������� ��� �������� ���������� Armor
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

        //Agility, Vitality, Luck, Ability�astingSpeed, AmountOfMana, CarriedWeight, CriticalStrikeChance, HealthRecovery, ManaRegeneration , ElementalDebuffChance
        //���������� ���������� �� ����� ������� ��������������� ��� �������� ���������� SecondaryWeapon
        //StatsValuesSecondaryWeapon.Add(Agility);
        //StatsValuesSecondaryWeapon.Add(Vitality);
        //StatsValuesSecondaryWeapon.Add(Luck);
        //StatsValuesSecondaryWeapon.Add(Ability�astingSpeed);
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

    //���������� ������� ���� ����� �� ����� ����
    public int CalculationCurrentAttackPowerFromStrength()
    {
        IncreasedAttackPowerFromStrength.modifiers.Clear();
        for (int i = 1; i <= ClasStat.GetValue(); i++)
        {
            IncreasedAttackPowerFromStrength.AddModifier(1);
        }
        return Convert.ToInt32(((float)DfaultAttackPowerFromStrength + (float)AttackPowerEquip.GetValue()) + ((float)DfaultAttackPowerFromStrength + (float)AttackPowerEquip.GetValue()) / (float)100 * (float)IncreasedAttackPowerFromStrength.GetValue());
    }

    //���������� ������� ���� ����� �� ����� ��������
    public int CalculationCurrentAttackPowerIncreasedAttackPowerFromIntelligence()
    {
        IncreasedAttackPowerFromIntelligence.modifiers.Clear();
        for (int i = 1; i <= ClasStat.GetValue(); i++)
        {
            IncreasedAttackPowerFromIntelligence.AddModifier(1);
        }
        return Convert.ToInt32(((float)DfaultAttackPowerFromIntelligence + (float)AttackPowerEquip.GetValue()) + ((float)DfaultAttackPowerFromIntelligence + (float)AttackPowerEquip.GetValue()) / (float)100 * (float)IncreasedAttackPowerFromIntelligence.GetValue());
    }

    //���������� �������� ���������� �����
    public int CalculationCurrentArmor()
    {
        return Convert.ToInt32(((float)DfaultArmor + (float)armorEquip.GetValue()) + ((float)DfaultArmor + (float)armorEquip.GetValue()) / (float)100 * (float)armorEquipBonus.GetValue());
    }
    //���������� ������� �������� �����
    public float CalculationCurrentAttackSpeed()
    {


        return ((DfaultAttackSpeed + DfaultAttackSpeed / (float)100 * (float)Ability�astingSpeed.GetValue()));
    }
    //���������� ������� �������� ������������
    public float CalculationCurrentSpeed()
    {
        return (DfaultMovementSpeed + DfaultMovementSpeed / 100 * MovementFactor.GetValue());
    }
    //���������� ������������� ���������� ��������
    public int CalculationCurrentAmountOfHealthMAX()
    {
        AmountOfHealth.modifiers.Clear();
        for (int i = 1; i <= Vitality.GetValue(); i++)
        {
            AmountOfHealth.AddModifier(2);

        }

        return Convert.ToInt32(((float)DfaultAmountOfHealthMAX + (float)AmountOfHealth.GetValue()) + ((float)DfaultAmountOfHealthMAX + (float)(float)AmountOfHealth.GetValue()) / (float)100 * (float)AmountOfHealthBonus.GetValue());
    }

    //���������� ������������� ���������� ����
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


