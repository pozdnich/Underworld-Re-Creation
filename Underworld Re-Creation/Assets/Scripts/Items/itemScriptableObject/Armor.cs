
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName = "Equipment/Armor")]
public class Armor : Equipment
{
    public ArmorSlot ArmorEquipSlot;
    //���������� ����� ��� ��� ������ ����������
    public int ArmorPowerEquipment;
    //������������� ������������� (������ ���� + (5 - 10%)
   // public ElementPower ElArmor;
    public int Element�oefficient;

    static public int Force;//����
    static public int Intelligence;//��������
    static public int Agility;//�������� 
    static public int Vitality;//���������

    static public int armorEquipBonus; // ����������� ������ �� �������
    static public int HealthRecovery;  //�������������� ��������  
    static public int ManaRegeneration; //�������������� ����
    static public int AmountOfHealthBonus; //���������� ����������� � ��X ��������
    static public int AmountOfManaBonus; //���������� ����������� � ��X ����
    static public int AmountOfHealth; //���������� �������� ����������� � ������������� ��������
    static public int AmountOfMana; //���������� ���� ����������� � ������������� ��������
    static public int Dodge; //���������

    public List<int> StatsValues = new List<int>() { Force, Intelligence, Agility, Vitality, armorEquipBonus, HealthRecovery, ManaRegeneration, AmountOfHealthBonus, AmountOfManaBonus, AmountOfHealth, AmountOfMana, Dodge };
    public List<bool> StatsAccess;
    public override void Use()
    {
        // Equip

        
        // Remove from inventory
       

    }

    public override void UnEquipUse()
    {
        
        
    }
}
public enum ArmorSlot { Head, Chest, Legs, Feet }