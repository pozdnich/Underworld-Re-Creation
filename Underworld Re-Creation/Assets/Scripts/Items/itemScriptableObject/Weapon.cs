using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName = "Equipment/Weapon")]
public class Weapon : Equipment
{
    public WeaponSlot WeaponequipSlot;
    //���� ����� �� ������
    public int AttackPowerEquipment;
    //������������� ���� (������ ���� + (10 - 25%)
   // public ElementPower ElWeapon;
    public int Element�oefficient;
    public int[] RandomPercentage;


    static public int Force;//����
    static public int Intelligence;//��������
    static public int Agility;//�������� 
    static public int Luck;//�����

   
    static public int Ability�astingSpeed;  //������� �������� �����
    static public int Accuracy; //���������
    static public int CriticalHitPercentage; //������� ������������ �����

    public List<int> StatsValues = new List<int>() { Force, Intelligence, Agility, Luck, Ability�astingSpeed, Accuracy, CriticalHitPercentage };
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
public enum WeaponSlot { Sword, Axe }

