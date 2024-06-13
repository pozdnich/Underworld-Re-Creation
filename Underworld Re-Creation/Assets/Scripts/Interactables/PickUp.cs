using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, соприкасается ли коллайдер игрока с предметом
        if (other.CompareTag("Player"))
        {
            Inventory.instance.AddInitialItems();
            // Пишем надпись о соприкосновении
            Debug.Log("Игрок соприкоснулся с предметом!");
        }
    }
}
