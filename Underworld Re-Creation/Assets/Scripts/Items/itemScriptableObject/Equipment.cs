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

   

	public EquipmentSlot equipSlot;     //  В какой слот установить

    public SpecificEquipment specificEquipment; // определение типа предмета
    public SkinnedMeshRenderer prefab;
    //Количество брони что даёт данная экиперовка
    public int ArmorPowerEquipment;
    //сила атаки от экипировки
    public int AttackPowerEquipment;
    //Элементальный процент (только один + (10 - 25%)
    // public ElementPower ElWeapon;
    public int ElementСoefficient;
    public int[] RandomPercentage;


    static public int Force;//Сила
    static public int Intelligence;//Интелект
    static public int Agility;//Ловкость 
    static public int Luck;//Удача
    static public int Vitality;//Живучесть


    static public int AbilityСastingSpeed;  //Процент скорости атаки
    static public int Accuracy; //Попадание
    static public int CriticalHitPercentage; //Процент Критического удара
    static public int armorEquipBonus; // Коэффициент защиты от бонусов
    static public int HealthRecovery;  //Восстановление здоровья  
    static public int ManaRegeneration; //Восстановление маны
    static public int AmountOfHealthBonus; //Коэфициэнт прибавления к МАX здоровью
    static public int AmountOfManaBonus; //Коэфициэнт прибавления к МАX Маны
    static public int AmountOfHealth; //Количество здоровья прибавиться к максимальному значению
    static public int AmountOfMana; //Количество маны прибавиться к максимальному значению
    static public int Dodge; //уклонения
    

    public List<int> StatsValues = new List<int>() { Force, Intelligence, Agility, Vitality, armorEquipBonus, HealthRecovery, ManaRegeneration, AmountOfHealthBonus, AmountOfManaBonus, AmountOfHealth, AmountOfMana, Dodge, Luck, AbilityСastingSpeed, Accuracy, CriticalHitPercentage };
    public List<bool> StatsAccess;
    

    // Вызывается при нажатии в инвентаре
    public override void Use ()
	{
		//  Оборудовать


        // Удалить из инвентаря


    }

    

   
}
                        // Голова, Грудь, Ноги, Ступни, Оружие, Вторичное оружие, Амулет
public enum EquipmentSlot { Head, Chest, Legs, Feet, Weapon, SecondaryWeapon, Amulet}
                            //Тканевая броня, Кожаная броня, Тяжёлая броня, Топор, Меч, Молот, Палочка, Посох, Двуручный лук, Одноручный арбалет, Двуручный арбалет, Щит, Книга заклинаний, Колчан
public enum SpecificEquipment { ClothArmor, LeatherArmor, HeavyArmor, AxeOne, SwordOne, Hammer, Wand, Staff, TwoHandedBow, OneHandedCrossbow, TwoHandedCrossbow, Shield, Spellbook, Quiver }
