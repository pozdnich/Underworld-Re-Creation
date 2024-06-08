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
    //сила атаки от оружия
    public int AttackPowerEquipment;
    //Элементальный урон (только один + (10 - 25%)
   // public ElementPower ElWeapon;
    public int ElementСoefficient;
    public int[] RandomPercentage;


    static public int Force;//Сила
    static public int Intelligence;//Интелект
    static public int Agility;//Ловкость 
    static public int Luck;//Удача

   
    static public int AbilityСastingSpeed;  //Процент скорости атаки
    static public int Accuracy; //Попадание
    static public int CriticalHitPercentage; //Процент Критического удара

    public List<int> StatsValues = new List<int>() { Force, Intelligence, Agility, Luck, AbilityСastingSpeed, Accuracy, CriticalHitPercentage };
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

