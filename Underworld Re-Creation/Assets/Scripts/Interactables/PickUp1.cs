using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PickUp1 : MonoBehaviour
{
    int AttackPower;

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
        if (other.CompareTag("TestCube"))
        {
            
            // Пишем надпись о соприкосновении
            Debug.Log("Стрела попала в камень!");
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            playerController.instance.playerAnim.PlayAttackMouse1СalculationD(other.GetComponent<EnemyStats>());
            other.GetComponent<Enemy>().ShowEnemyHP = 4f;
            Destroy(gameObject);
        }
    }
}
