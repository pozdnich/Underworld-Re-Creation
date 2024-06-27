using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item {

   

	public EquipmentSlot equipSlot;     //  � ����� ���� ����������

    public SpecificEquipment specificEquipment; // ����������� ���� ��������
    public SkinnedMeshRenderer prefab;
    //���������� ����� ��� ��� ������ ����������
    public int ArmorPowerEquipment;
    //���� ����� �� ����������
    public int AttackPowerEquipment;
    //������������� ������� (������ ���� + (10 - 25%)
    // public ElementPower ElWeapon;
    public int Element�oefficient;
    public int[] RandomPercentage;


    static public int Force;//����
    static public int Intelligence;//��������
    static public int Agility;//�������� 
    static public int Luck;//�����
    static public int Vitality;//���������


    static public int Ability�astingSpeed;  //������� �������� �����
    static public int Accuracy; //���������
    static public int CriticalHitPercentage; //������� ������������ �����
    static public int armorEquipBonus; // ����������� ������ �� �������
    static public int HealthRecovery;  //�������������� ��������  
    static public int ManaRegeneration; //�������������� ����
    static public int AmountOfHealthBonus; //���������� ����������� � ��X ��������
    static public int AmountOfManaBonus; //���������� ����������� � ��X ����
    static public int AmountOfHealth; //���������� �������� ����������� � ������������� ��������
    static public int AmountOfMana; //���������� ���� ����������� � ������������� ��������
    static public int Dodge; //���������
    

    public List<int> StatsValues = new List<int>() { Force, Intelligence, Agility, Vitality, armorEquipBonus, HealthRecovery, ManaRegeneration, AmountOfHealthBonus, AmountOfManaBonus, AmountOfHealth, AmountOfMana, Dodge, Luck, Ability�astingSpeed, Accuracy, CriticalHitPercentage };
    public List<bool> StatsAccess;
    

    // ���������� ��� ������� � ���������
    public override void Use ()
	{
		//  �����������


        // ������� �� ���������


    }

    

   
}
                        // ������, �����, ����, ������, ������, ��������� ������, ������
public enum EquipmentSlot { Head, Chest, Legs, Feet, Weapon, SecondaryWeapon, Amulet}
                            //�������� �����, ������� �����, ������ �����, �����, ���, �����, �������, �����, ��������� ���, ���������� �������, ��������� �������, ���, ����� ����������, ������
public enum SpecificEquipment { ClothArmor, LeatherArmor, HeavyArmor, AxeOne, SwordOne, Hammer, Wand, Staff, TwoHandedBow, OneHandedCrossbow, TwoHandedCrossbow, Shield, Spellbook, Quiver }
