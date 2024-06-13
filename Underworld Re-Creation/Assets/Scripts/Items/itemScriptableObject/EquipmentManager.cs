using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentManager : MonoBehaviour {

	#region Singleton


	public static EquipmentManager instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<EquipmentManager> ();
			}
			return _instance;
		}
	}
	static EquipmentManager _instance;

	void Awake ()
	{
		_instance = this;
	}

	#endregion

	public Equipment[] defaultWear;

	//public Armor[] defaultWear1;
	
 //   public InventorySlotProfile[] ArmorSlotShow;
 //   public InventorySlotProfile WeaponSlotShow;
 //   public InventorySlotProfile SecondaryWeaponSlotShow;

 //   Equipment[] currentEquipment;
	//SkinnedMeshRenderer[] currentMeshes;

 //   Armor[] currentArmor;
 //   SkinnedMeshRenderer[] currentMeshesArmor;

 //   public Weapon currentWeapon;
 //   public SkinnedMeshRenderer currentMeshesWeapon;

 //   public SecondaryWeapon currentSecondaryWeapon;
 //   public SkinnedMeshRenderer currentMeshesSecondaryWeapon;


 //   public SkinnedMeshRenderer targetMesh;

 //   // Callback for when an item is equipped

 //   public delegate void OnArmorChanged(Armor newItem, Armor oldItem);
 //   public event OnArmorChanged onArmorChanged;

 //   public delegate void OnWeaponChanged(Weapon newItem, Weapon oldItem);
 //   public event OnWeaponChanged onWeaponChanged;

 //   public delegate void OnSecondaryWeaponChanged(SecondaryWeapon newItem, SecondaryWeapon oldItem);
 //   public event OnSecondaryWeaponChanged onSecondaryWeaponChanged;


 //   public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
	//public event OnEquipmentChanged onEquipmentChanged;

	//Inventory inventory;

 //   public GameObject NotificationSA;
 //   public Text TextNotification;

 //   void Start ()
	//{
        
 //       inventory = Inventory.instance;


	//	int numSlotsArmor = System.Enum.GetNames(typeof(ArmorSlot)).Length;
	//	currentArmor = new Armor[numSlotsArmor];
 //       currentMeshesArmor = new SkinnedMeshRenderer[numSlotsArmor];



        


 //       int numSlots = System.Enum.GetNames (typeof(EquipmentSlot)).Length;
	//	currentEquipment = new Equipment[numSlots];
	//	currentMeshes = new SkinnedMeshRenderer[numSlots];
        
	//	//EquipAllDefault ();
	//}

	//void Update() {
 //       if (Input.GetKeyDown(KeyCode.U))
 //       {
 //           UnequipAll();
 //       }
 //   }


	//public Equipment GetEquipment(EquipmentSlot slot) {
	//	return currentEquipment [(int)slot];
	//}

	//// Equip a new item

	//public void Equip (Equipment newItem)
	//{
	//		Equipment oldItem = null;

	//		// Find out what slot the item fits in
	//		// and put it there.
	//		int slotIndex = (int)newItem.equipSlot;

	//		// If there was already an item in the slot
	//		// make sure to put it back in the inventory
	//		if (currentEquipment[slotIndex] != null)
	//		{
	//			oldItem = currentEquipment[slotIndex];

	//			inventory.Add(oldItem);

	//		}

 //       // An item has been equipped so we trigger the callback Предмет был экипирован, поэтому мы запускаем обратный вызов
 //       if (onEquipmentChanged != null)
	//			onEquipmentChanged.Invoke(newItem, oldItem);

	//		currentEquipment[slotIndex] = newItem;
	//		Debug.Log(newItem.name + " equipped!");

	//		if (newItem.prefab)
	//		{
	//			AttachToMesh(newItem.prefab, slotIndex);
	//		}
 //       //equippedItems [itemIndex] = newMesh.gameObject;
 //   }
   
 //   void Unequip(int slotIndex) {
	//	if (currentEquipment[slotIndex] != null)
	//	{
	//		Equipment oldItem = currentEquipment [slotIndex];
	//		inventory.Add(oldItem);
				
	//		currentEquipment [slotIndex] = null;
	//		if (currentMeshes [slotIndex] != null) {
	//			Destroy (currentMeshes [slotIndex].gameObject);
	//		}


	//		// Equipment has been removed so we trigger the callback
	//		if (onEquipmentChanged != null)
	//			onEquipmentChanged.Invoke(null, oldItem);
			
	//	}

	
	//}



 //   void UnequipAll()
 //   {
 //       for (int i = 0; i < currentEquipment.Length; i++)
 //       {
 //           Unequip(i);
 //       }
 //       EquipAllDefault();
 //   }

 //   void EquipAllDefault() {
	//	foreach (Equipment e in defaultWear) {
	//		Equip (e);
	//	}
	//}

	//void AttachToMesh(SkinnedMeshRenderer mesh, int slotIndex) {

	//	if (currentMeshes [slotIndex] != null) {
	//		Destroy (currentMeshes [slotIndex].gameObject);
	//	}

	//	SkinnedMeshRenderer newMesh = Instantiate(mesh) as SkinnedMeshRenderer;
	//	newMesh.bones = targetMesh.bones;
	//	newMesh.rootBone = targetMesh.rootBone;
	//	currentMeshes [slotIndex] = newMesh;
	//}

 //   //--------------------------------Armor------------------------------------

 //   public Armor GetArmor(ArmorSlot slot)
 //   {
 //       return currentArmor[(int)slot];
 //   }
 //   public void Equip(Armor newItem)
 //   {
 //       if (Player.instance.playerStats.AccessEquipArmor[newItem.ArmorEquipSlot])
 //       {
            
 //           Armor oldItem = null;

 //           // Find out what slot the item fits in
 //           // and put it there.
 //           int slotIndex = (int)newItem.ArmorEquipSlot;
            

 //           // If there was already an item in the slot
 //           // make sure to put it back in the inventory
 //           if (currentArmor[slotIndex] != null)
 //           {
 //               oldItem = currentArmor[slotIndex];
 //               ArmorSlotShow[slotIndex].ClearSlot();
 //               inventory.Add(oldItem);

 //           }

 //           // An item has been equipped so we trigger the callback Предмет был экипирован, поэтому мы запускаем обратный вызов
 //           if (onArmorChanged != null)
 //           {
 //               onArmorChanged.Invoke(newItem, oldItem);
 //           }


 //           currentArmor[slotIndex] = newItem;
 //           ArmorSlotShow[slotIndex].AddItem(newItem);
 //           Debug.Log(newItem.name + " equipped!");

 //           if (newItem.prefab)
 //           {
 //               AttachToMeshArmor(newItem.prefab, slotIndex);
 //           }

 //       }
 //       else
 //       {
	//		//Здесь можно добавить описание причины не одевания

 //           NotificationSA.SetActive(true);
 //           TextNotification.text = $"Вы не можете экиперовать {newItem.name}, так как у вас нет нужного навыка!";

 //       }

        
 //   }
 //   public void UnequipThisBTN(Armor item)
 //   {
 //       int slotIndex = ((int)item.ArmorEquipSlot);

 //       Armor oldItem = currentArmor[slotIndex];
 //       inventory.Add(oldItem);

 //       currentArmor[slotIndex] = null;
 //       if (currentMeshesArmor[slotIndex] != null)
 //       {
 //           Destroy(currentMeshesArmor[slotIndex].gameObject);
 //       }

 //       if (onArmorChanged != null)
 //           onArmorChanged.Invoke(null, oldItem);
 //       ArmorSlotShow[slotIndex].ClearSlot();


 //   }
 //   void AttachToMeshArmor(SkinnedMeshRenderer mesh, int slotIndex)
 //   {

 //       if (currentMeshesArmor[slotIndex] != null)
 //       {
 //           Destroy(currentMeshesArmor[slotIndex].gameObject);
 //       }

 //       SkinnedMeshRenderer newMesh = Instantiate(mesh) as SkinnedMeshRenderer;
 //       newMesh.bones = targetMesh.bones;
 //       newMesh.rootBone = targetMesh.rootBone;
 //       currentMeshesArmor[slotIndex] = newMesh;
 //   }

 //   //--------------------------------Weapon------------------------------------

 //   public Weapon GetWeapon()
 //   {
 //       return currentWeapon;
 //   }
 //   public void Equip(Weapon newItem)
 //   {
 //       if (Player.instance.playerStats.AccessEquipWeapon[newItem.WeaponequipSlot])
 //       {

 //           Weapon oldItem = null;


 //           // If there was already an item in the slot
 //           // make sure to put it back in the inventory
 //           if (currentWeapon != null)
 //           {
 //               oldItem = currentWeapon;
 //               WeaponSlotShow.ClearSlot();
 //               inventory.Add(oldItem);

 //           }

 //           // An item has been equipped so we trigger the callback Предмет был экипирован, поэтому мы запускаем обратный вызов
 //           if (onWeaponChanged != null)
 //           {
 //               onWeaponChanged.Invoke(newItem, oldItem);
 //           }


 //           currentWeapon = newItem;
 //           WeaponSlotShow.AddItem(newItem);
 //           Debug.Log(newItem.name + " equipped!");

 //           if (newItem.prefab)
 //           {
 //               AttachToMeshWeapon(newItem.prefab);
 //           }

 //       }
 //       else
 //       {
 //           //Здесь можно добавить описание причины не одевания

 //           NotificationSA.SetActive(true);
 //           TextNotification.text = $"Вы не можете экиперовать {newItem.name}, так как у вас нет нужного навыка!";

 //       }


 //   }
 //   public void UnequipThisBTN(Weapon item)
 //   {
        
 //       Weapon oldItem = currentWeapon;
 //       inventory.Add(oldItem);

 //       currentWeapon = null;
 //       if (currentMeshesWeapon != null)
 //       {
 //           Destroy(currentMeshesWeapon.gameObject);
 //       }

 //       if (onWeaponChanged != null)
 //           onWeaponChanged.Invoke(null, oldItem);
 //       WeaponSlotShow.ClearSlot();


 //   }
 //   void AttachToMeshWeapon(SkinnedMeshRenderer mesh)
 //   {

 //       if (currentMeshesWeapon != null)
 //       {
 //           Destroy(currentMeshesWeapon.gameObject);
 //       }

 //       SkinnedMeshRenderer newMesh = Instantiate(mesh) as SkinnedMeshRenderer;
 //       newMesh.bones = targetMesh.bones;
 //       newMesh.rootBone = targetMesh.rootBone;
 //       currentMeshesWeapon = newMesh;
 //   }

 //   //--------------------------------SecondaryWeapon------------------------------------


 //   public SecondaryWeapon GetSecondaryWeapon()
 //   {
 //       return currentSecondaryWeapon;
 //   }
 //   public void Equip(SecondaryWeapon newItem)
 //   {
 //       if (Player.instance.playerStats.AccessEquipSecondaryWeapon[newItem.SecondaryWeaponEquipSlot])
 //       {

 //           SecondaryWeapon oldItem = null;


 //           // If there was already an item in the slot
 //           // make sure to put it back in the inventory
 //           if (currentWeapon != null)
 //           {
 //               oldItem = currentSecondaryWeapon;
 //               SecondaryWeaponSlotShow.ClearSlot();
 //               inventory.Add(oldItem);

 //           }

 //           // An item has been equipped so we trigger the callback Предмет был экипирован, поэтому мы запускаем обратный вызов
 //           if (onSecondaryWeaponChanged != null)
 //           {
 //               onSecondaryWeaponChanged.Invoke(newItem, oldItem);
 //           }


 //           currentSecondaryWeapon = newItem;
 //           SecondaryWeaponSlotShow.AddItem(newItem);
 //           Debug.Log(newItem.name + " equipped!");

 //           if (newItem.prefab)
 //           {
 //               AttachToMeshSecondaryWeapon(newItem.prefab);
 //           }

 //       }
 //       else
 //       {
 //           //Здесь можно добавить описание причины не одевания

 //           NotificationSA.SetActive(true);
 //           TextNotification.text = $"Вы не можете экиперовать {newItem.name}, так как у вас нет нужного навыка!";

 //       }


 //   }
 //   public void UnequipThisBTN(SecondaryWeapon item)
 //   {
 //       ;

 //       SecondaryWeapon oldItem = currentSecondaryWeapon;
 //       inventory.Add(oldItem);

 //       currentSecondaryWeapon = null;
 //       if (currentMeshesSecondaryWeapon != null)
 //       {
 //           Destroy(currentMeshesSecondaryWeapon.gameObject);
 //       }

 //       if (onSecondaryWeaponChanged != null)
 //           onSecondaryWeaponChanged.Invoke(null, oldItem);
 //       SecondaryWeaponSlotShow.ClearSlot();


 //   }
 //   void AttachToMeshSecondaryWeapon(SkinnedMeshRenderer mesh)
 //   {

 //       if (currentMeshesSecondaryWeapon != null)
 //       {
 //           Destroy(currentMeshesSecondaryWeapon.gameObject);
 //       }

 //       SkinnedMeshRenderer newMesh = Instantiate(mesh) as SkinnedMeshRenderer;
 //       newMesh.bones = targetMesh.bones;
 //       newMesh.rootBone = targetMesh.rootBone;
 //       currentMeshesSecondaryWeapon = newMesh;
 //   }










}
