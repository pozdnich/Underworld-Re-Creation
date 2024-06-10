
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName = "Equipment/SecondaryWeapon")]
public class SecondaryWeapon : Equipment
{
    public SecondaryWeaponSlot SecondaryWeaponEquipSlot;
    //���������� ����� ��� ��� ������ ����������
    public int ArmorPowerEquipment;
    //���� ����� �� ������
    public int AttackPowerEquipment;
    //������������� ������������� (������ ���� + (5 - 10%)
    // public ElementPower ElArmor;
    public int Element�oefficient;

    static public int Force;//����
    static public int Intelligence;//��������
    static public int Agility;//�������� 
    static public int Vitality;//���������
    static public int Luck;//�����

    static public int armorEquipBonus; // ����������� ������ �� �������
    static public int HealthRecovery;  //�������������� ��������  
    static public int ManaRegeneration; //�������������� ����
    static public int AmountOfHealthBonus; //���������� ����������� � ��X ��������
    static public int AmountOfManaBonus; //���������� ����������� � ��X ����
    static public int AmountOfHealth; //���������� �������� ����������� � ������������� ��������
    static public int AmountOfMana; //���������� ���� ����������� � ������������� ��������
    static public int Dodge; //���������

    static public int Ability�astingSpeed;  //������� �������� �����
    static public int Accuracy; //���������
    static public int CriticalHitPercentage; //������� ������������ �����

    public List<int> AttackStatsValues = new List<int>() { Force, Intelligence, Agility, Luck, Ability�astingSpeed, Accuracy, CriticalHitPercentage };
    public List<bool> AttackStatsAccess;

    public List<int> ArmorStatsValues = new List<int>() { Force, Intelligence, Agility, Vitality, armorEquipBonus, HealthRecovery, ManaRegeneration, AmountOfHealthBonus, AmountOfManaBonus, AmountOfHealth, AmountOfMana, Dodge };
    public List<bool> ArmorStatsAccess;
    public override void Use()
    {
        // Equip

        
        // Remove from inventory
       

    }

    public override void UnEquipUse()
    {
        
        
    }
}
public enum SecondaryWeaponSlot { Shield, Quiver }