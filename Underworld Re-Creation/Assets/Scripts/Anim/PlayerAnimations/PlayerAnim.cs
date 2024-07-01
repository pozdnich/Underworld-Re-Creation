using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerAnim : CharAnim
{
   // PlayerCombat combat;
  //  EquipmentManager EquiptManager;

    public int attackIndex = 0;
    GameObject errorAbility;

    public ParticleSystem AoeStan;
    bool normalAttack; // проверка на то что обычная аттака разрешена
    RaycastHit hitnormalAttack; //для направления в сторону нужного врага при нормальной атаке
    override public void Start()
    {
        base.Start();
        // EquiptManager = EquipmentManager.instance;
        // combat = GetComponentInParent<PlayerCombat>();
        // combat.OnPlayAttackMouse1 += PlayAttackMouse1;
        // combat.OnPlayAttack1Ability += OnPlayAttack1Ability;
        normalAttack=false;
    }

    override public void Update()
    {
        base.Update();

        if (GetComponentInParent<PlayerStats>().OnOffCombat)
        {
           // animator.SetFloat("OnOffCombatIdle", (float)1);
        }
        else
        {
          //  animator.SetFloat("OnOffCombatIdle", (float)0);
        }

        if (Vector3.Distance(transform.position, navmeshAgent.destination) < 0.1f && normalAttack)
        {
            navmeshAgent.Stop();//принудительная остановка перемещения
            animator.SetBool("Run", false);// отклучение бега
            animator.SetFloat("SkillNumber", 0);
            if (Inventory.instance.ProfileSlot[4] != null)
            {
                animator.SetFloat("NumWeapon", (float)Inventory.instance.ProfileSlot[4].item.specificEquipment);
            }
            else
            {
                animator.SetFloat("NumWeapon", (float)0);
            }
            Vector3 direction = (hitnormalAttack.point - transform.position).normalized;
            direction.y = 0; // Убираем наклон по вертикали, чтобы персонаж не наклонялся вверх/вниз
            transform.rotation = Quaternion.LookRotation(direction);
            animator.SetTrigger("Attack");
            normalAttack = false;
        }
    }

    IEnumerator WriteError(string TextError)
    {
        if (errorAbility != null)
        {
            errorAbility.SetActive(true);
            errorAbility.GetComponent<Text>().text = $"{TextError}";
            yield return new WaitForSeconds(4);
            errorAbility.SetActive(false);
        }
      
    }

    public void finishMyAnim()
    {
       // GetComponentInParent<PlayerCombat>().attackCountdown = true;
      //  GetComponentInParent<PlayerController>().OnOff = true;
        //  Debug.Log("Конец анимации");
    }

    //Выбор стандартной анимации атаки по наличию текущего оружия
    public void PlayAttackMouse1(float combatRange, RaycastHit hit)
    {
        if(combatRange>2&& combatRange < 6 || combatRange == -1)
        {
            navmeshAgent.SetDestination(hit.point);//вводим координаты
            animator.SetBool("Run", true);//запускаем анимацию бега
            navmeshAgent.Resume();// принудительный запуск перемещения
            hitnormalAttack = hit;
            normalAttack = true;
        }
        else if(combatRange>5&& combatRange < 11)
        {
            // Поворачиваем персонажа в сторону цели
            Vector3 direction = (hit.point - transform.position).normalized;
            direction.y = 0; // Убираем наклон по вертикали, чтобы персонаж не наклонялся вверх/вниз
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
    }

    public void OnPlayAttack1Ability1(float SkillNumber)
    {
        animator.SetFloat("SkillNumber", SkillNumber);
        animator.SetFloat("NumWeapon", (float)Inventory.instance.ProfileSlot[4].item.specificEquipment);
        animator.SetTrigger("Attack");

    }

    public void AoES()
    {
        AoeStan.transform.position = transform.position;
        AoeStan.enableEmission = true;

    }

    void ActivateAoeStan()
    {
       
        if (AoeStan != null)
        {
            AoeStan.Play();
        }
    }

    void NoActivateAoeStan()
    {

        if (AoeStan != null && AoeStan.isPlaying)
        {
            AoeStan.Stop();
        }
    }

    public virtual void momentOfAttack()
    {
      //  GetComponentInParent<PlayerCombat>().MOA();
    }

    public virtual void momentOfAttacks()
    {
      //  GetComponentInParent<PlayerCombat>().MOAs();
    }
}
