using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment : Item {

	public EquipmentSlot equipSlot;     // What slot to equip it in � ����� ���� ����������
    public SkinnedMeshRenderer prefab;


    // Called when pressed in the inventory ���������� ��� ������� � ���������
    public override void Use ()
	{
		// Equip �����������


        // Remove from inventory ������� �� ���������


    }

    

   
}

public enum EquipmentSlot { Armor, Weapon, SecondaryWeapon}
