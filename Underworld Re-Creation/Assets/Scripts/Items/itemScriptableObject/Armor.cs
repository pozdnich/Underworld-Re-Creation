
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
    //Количество брони что даёт данная экиперовка
    public int ArmorPowerEquipment;
    //Элементальное сопратевление (только один + (5 - 10%)
   // public ElementPower ElArmor;
    public int ElementСoefficient;

    static public int Force;//Сила
    static public int Intelligence;//Интелект
    static public int Agility;//Ловкость 
    static public int Vitality;//Живучесть

    static public int armorEquipBonus; // Коэффициент защиты от бонусов
    static public int HealthRecovery;  //Восстановление здоровья  
    static public int ManaRegeneration; //Восстановление маны
    static public int AmountOfHealthBonus; //Коэфициэнт прибавления к МАX здоровью
    static public int AmountOfManaBonus; //Коэфициэнт прибавления к МАX Маны
    static public int AmountOfHealth; //Количество здоровья прибавиться к максимальному значению
    static public int AmountOfMana; //Количество маны прибавиться к максимальному значению
    static public int Dodge; //уклонения

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