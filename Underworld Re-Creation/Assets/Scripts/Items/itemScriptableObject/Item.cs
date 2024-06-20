using UnityEngine;

/* The base item class. All items should derive from this.  Базовый класс предметов. Все предметы должны исходить из этого.*/

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	new public string name = "New Item";    // Name of the item Название предмета
    public Sprite icon = null;              // Item icon Значок предмета
    public bool showInInventory = true;
    public GameObject AllThisItem;  // Образ этого итема когда он на земле, требуется для сохранения его параметров
    


    // Called when the item is pressed in the inventory Вызывается при нажатии предмета в инвентаре
    public virtual void Use ()
	{
        // Use the item Использовать предмет
        // Something may happen Что-то может случиться
    }

    public virtual void UnEquipUse()
    {
        // Use the item Использовать предмет
        // Something may happen Что-то может случиться
    }

    // Call this method to remove the item from inventory Вызовите этот метод, чтобы удалить предмет из инвентаря
    public void RemoveFromInventory ()
	{
		
	}
    // Функция выбрасывания предмета
    

}
