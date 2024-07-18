using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInInventoryCon : MonoBehaviour
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
                Inventory.instance.AddItemCon(GetComponentInChildren<itemInCanvasCon>());

               
                Destroy(gameObject);
            }
        }
    }
}
