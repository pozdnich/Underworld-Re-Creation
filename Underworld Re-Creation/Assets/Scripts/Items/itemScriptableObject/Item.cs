using UnityEngine;

/* The base item class. All items should derive from this.  Базовый класс предметов. Все предметы должны исходить из этого.*/

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	new public string name = "New Item";    // Название предмета
    public Sprite icon = null;              // Значок предмета
    public bool showInInventory = true;
    public GameObject AllThisItem;  // Образ этого итема когда он на земле, требуется для сохранения его параметров
    


   
    public virtual void Use ()
	{
        // Использовать предмет
       
    }

    

}
