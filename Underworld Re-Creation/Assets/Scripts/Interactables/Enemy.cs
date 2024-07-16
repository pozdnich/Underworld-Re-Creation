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


        if (ShowEnemyHP > 0)
        {
            if (playerController.instance.EnemyHP.activeSelf != true && playerController.instance.Focus == this.gameObject)
            {
            Debug.Log("ѕоказ здоровь€ противника");
              
                playerController.instance.EnemyHP.SetActive(true);
                playerController.instance.EnemyHP.GetComponent<UnityEngine.UI.Image>().fillAmount = (float)stats.CalculationCurrentAmountOfHealthMAX();
            }
            else if (playerController.instance.EnemyHP.activeSelf)
            {
                playerController.instance.EnemyHPImage.GetComponent<UnityEngine.UI.Image>().fillAmount = 1.0f / (float)stats.CalculationCurrentAmountOfHealthMAX() * (float)stats.AmountOfHealthCurrent;
                playerController.instance.EnemyHPText.GetComponent<UnityEngine.UI.Text>().text = $"{stats.AmountOfHealthCurrent}/{stats.CalculationCurrentAmountOfHealthMAX()}";

            }
            ShowEnemyHP -= Time.deltaTime;
            if (ShowEnemyHP <= 0 || playerController.instance.Focus != this.gameObject)
            {
                playerController.instance.EnemyHP.SetActive(false);
                ShowEnemyHP = 0;
            }
        }

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
    //проверка работоспособности выпадени€ предмета при смерти врага
   
   
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("»грок соприкоснулс€ с колайдером ¬рага!");
        // ѕровер€ем, соприкасаетс€ ли коллайдер игрока с предметом
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<playerController>().Focus != null && other.GetComponent<playerController>().Focus == this.gameObject) {
                other.GetComponent<PlayerAnim>().EnemyAndPlayerCollidersTouching = true;
            }
           
            // Die();

        }
    }
    private void OnTriggerExit(Collider other)
    {
       
        // ѕровер€ем, соприкасаетс€ ли коллайдер игрока с предметом
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerAnim>().EnemyAndPlayerCollidersTouching = false;
            // Die();

        }
    }

    public void Die() {
        Debug.Log("«дох скелет");

        //ragdoll.transform.parent = null;
        //ragdoll.Setup();
        playerController.instance.EnemyHP.SetActive(false);
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
