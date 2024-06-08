using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item {

	public EquipmentSlot equipSlot;     // What slot to equip it in В какой слот установить
    public SkinnedMeshRenderer prefab;


    // Called when pressed in the inventory Вызывается при нажатии в инвентаре
    public override void Use ()
	{
		// Equip Оборудовать


        // Remove from inventory Удалить из инвентаря


    }

    

   
}

public enum EquipmentSlot { Armor, Weapon, SecondaryWeapon}
