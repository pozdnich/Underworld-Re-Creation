using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.GraphicsBuffer;

/* This makes our enemy interactable.  Ёто делает нашего врага интерактивным.*/

[RequireComponent(typeof(EnemyStats))]
public class Enemy : MonoBehaviour
{

	EnemyStats stats;
	public int Experience = 10;

  
	public GameObject loots;
    public List<GameObject> Loot;
    public bool StanEnemy = false;
    public float ShowEnemyHP = 0;

    public GameObject LootStart;
    public float lookRadius = 3f;
    public void Start ()
	{
		stats = GetComponent<EnemyStats>();
		stats.OnHealthReachedZero += Die;
    }

    public void Update()
    {
        
        
        //if (ShowEnemyHP>0)
        //{
        //    if (Player.instance.GetComponent<PlayerController>().EnemyHP.activeSelf != true && Player.instance.GetComponent<PlayerController>().focus == this)
        //    {
        //        Player.instance.GetComponent<PlayerController>().EnemyHP.SetActive(true);
        //        Player.instance.GetComponent<PlayerController>().EnemyHP.GetComponent<UnityEngine.UI.Image>().fillAmount =(float)stats.CalculationCurrentAmountOfHealthMAX();
        //    }
        //    else if (Player.instance.GetComponent<PlayerController>().EnemyHP.activeSelf)
        //    {
        //        Player.instance.GetComponent<PlayerController>().EnemyHPImage.GetComponent<UnityEngine.UI.Image>().fillAmount = 1.0f / (float)stats.CalculationCurrentAmountOfHealthMAX() * (float)stats.AmountOfHealthCurrent;
        //        Player.instance.GetComponent<PlayerController>().EnemyHPText.GetComponent<UnityEngine.UI.Text>().text = $"{stats.AmountOfHealthCurrent}/{stats.CalculationCurrentAmountOfHealthMAX()}";

        //    }
        //    ShowEnemyHP -= Time.deltaTime;
        //    if (ShowEnemyHP <= 0||Player.instance.GetComponent<PlayerController>().focus != this)
        //    {
        //        Player.instance.GetComponent<PlayerController>().EnemyHP.SetActive(false);
        //        ShowEnemyHP = 0;
        //    }
        //}
       
    }

   
    // When we interact with the enemy: We attack it.  огда мы взаимодействуем с противником: ћы атакуем его.
    public  void Interact()
	{
        // Debug.Log("Enemy");
        //print ("Interact");
        //Debug.Log($"{stats.currentHealth} здоровье скелета1");
        //PlayerCombat combatManager = Player.instance.playerCombatManager;
        //if (StanEnemy)
        //{

        //    GetComponent<EnemyCombat>().Stan();

        //}



        //combatManager.AttackToEnemy(stats);
        //if (Player.instance.GetComponent<PlayerController>().focus == this)
        //{
        //    ShowEnemyHP = 5f;
        //}
        //else
        //{
        //    ShowEnemyHP = 0f;
        //}
        //  Debug.Log($"AttackToEnemy1");

       

    }
    // проверка работоспособности выпадени€ предмета при смерти врага
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("»грок соприкоснулс€ с колайдером ¬рага!");
        // ѕровер€ем, соприкасаетс€ ли коллайдер игрока с предметом
        if (other.CompareTag("Player"))
        {

            Die();

        }
    }

    public void Die() {
        Debug.Log("«дох скелет");

        //ragdoll.transform.parent = null;
        //ragdoll.Setup();
        //Player.instance.GetComponent<PlayerController>().EnemyHP.SetActive(false);
        Destroy (gameObject);
		DropLoot();

        //PlayerCombat combatManager = Player.instance.playerCombatManager;
		//combatManager.ExperienceBoost(Experience, Player.instance);

    }

	void DropLoot()
	{
        Vector3 poz = LootStart.GetComponent<Transform>().position;
        int l = UnityEngine.Random.Range(0, Loot.Count - 1);
        Loot[l].GetComponentInChildren<ItemInCanvas>().item.AllThisItem = Loot[l]; // присвоили его образ его партаметру AllThisItem чтобы можно было его спокойно выкидывать из инвентор€ и обратно подымать в инвентарь

        Loot[l].GetComponentInChildren<ItemInCanvas>().item.AllThisItem.GetComponentInChildren<ItemInCanvas>().item.ArmorPowerEquipment = 200; // пример рамндомной настройки предмета

        GameObject loot = Instantiate(Loot[l].GetComponentInChildren<ItemInCanvas>().item.AllThisItem, poz, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}
