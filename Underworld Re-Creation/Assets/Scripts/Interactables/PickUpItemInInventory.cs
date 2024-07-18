using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInInventory : MonoBehaviour
{
    public void Start()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Игрок соприкоснулся с колайдером!");
        // Проверяем, соприкасается ли коллайдер игрока с предметом
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок соприкоснулся с предметом!");
            playerController player = other.GetComponent<playerController>();
            if (player != null && player.Focus == gameObject)
            {
               
                Debug.Log("Предмет был взят в фокус");
                Inventory.instance.AddItem(GetComponentInChildren<ItemInCanvas>());

                // Пишем надпись о соприкосновении
                Debug.Log($"Игрок положил в инвентарь предмет!{GetComponentInChildren<ItemInCanvas>().item.name}");
                Destroy(gameObject);
            }
        }
    }
}
