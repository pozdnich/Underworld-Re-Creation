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

    public bool EnemyAndPlayerCollidersTouching;

    public ParticleSystem AoeStan;
    bool normalAttack; // �������� �� �� ��� ������� ������ ���������
    RaycastHit hitnormalAttack; //��� ����������� � ������� ������� ����� ��� ���������� �����

    public bool attackCountdown;
    PlayerStats myStats;
    override public void Start()
    {
        base.Start();
        // EquiptManager = EquipmentManager.instance;
        // combat = GetComponentInParent<PlayerCombat>();
        // combat.OnPlayAttackMouse1 += PlayAttackMouse1;
        // combat.OnPlayAttack1Ability += OnPlayAttack1Ability;
        normalAttack=false;
        EnemyAndPlayerCollidersTouching =false;
        attackCountdown = true;
        myStats = GetComponent<PlayerStats>();
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

        
    }

    private IEnumerator initialCountdownMetod()
    {
        attackCountdown=false;
         // ��� ���� �������
         yield return new WaitForSeconds(2f);
        attackCountdown = true;
        // �������� OnDrop

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
        //  Debug.Log("����� ��������");
    }

    //����� ����������� �������� ����� �� ������� �������� ������
    public void PlayAttackMouse1()
    {
        if (EnemyAndPlayerCollidersTouching && attackCountdown)
        {
            if (Inventory.instance.ProfileSlot[4] != null)
            {
                Debug.Log($" �����  ������ {(float)Inventory.instance.ProfileSlot[4].item.specificEquipment}");
                animator.SetFloat("NumWeapon", (float)Inventory.instance.ProfileSlot[4].item.specificEquipment);
            }
            else
            {
                animator.SetFloat("NumWeapon", (float)0);
            }
            animator.SetFloat("SkillNumber", (float)0);
            animator.SetTrigger("Attack");
            
            StartCoroutine(initialCountdownMetod());
            

        }
       
        

    }

    public void PlayAttackMouse1�alculationD(EnemyStats stats)
    {
        

        // Debug.Log($"DoDamage");
        int AttackPower = 0;
        attackIndex = 0;
        PlayerAnim CurrentPlayerAnim = GetComponentInChildren<PlayerAnim>();


        if (Random.Range(1, 100) <= myStats.CriticalStrikeChance.GetValue())
        {
            AttackPower = myStats.CalculationCurrentAttackPowerFromStrength() + myStats.CalculationCurrentAttackPowerFromStrength() / 100 * myStats.CriticalHitPercentage.GetValue();
            //  Debug.Log(transform.name + " ������� ���� ���� � ������� " + AttackPower + " damage");


        }
        else
        {
            AttackPower = myStats.CalculationCurrentAttackPowerFromStrength();
            //  Debug.Log(transform.name + " ������� ���� � ������� " + AttackPower + " damage");

        }



        // Debug.Log(" attackIndex " + CurrentPlayerAnim.attackIndex);
        int FD = 0;
        switch (CurrentPlayerAnim.attackIndex)
        {
            case 0:
                FD = stats.TakeNormalDamage(myStats.currentLvl, AttackPower, ((int)myStats.ElementPowerCharacter), myStats.ElementalDamage[(int)myStats.ElementPowerCharacter].GetValue(), myStats.Accuracy.GetValue());
                Debug.Log(" ����� ���� ������� ������" + FD);
                if (Inventory.instance.ProfileSlot[5] != null && FD != 0)
                {

                    stats.PeriodicElementalDamage(myStats.timeDE, ((int)myStats.ElementPowerCharacter), FD, Inventory.instance.ProfileSlot[5].item.RandomPercentage, myStats.ElementalDebuffChance.GetValue());
                }
                Debug.Log($"TakeNormalDamage");
                break;
            case 1:
                stats.StanToEnemy(3, 100);

                break;
            case 2:
                FD = stats.TakeNormalDamage(myStats.currentLvl, AttackPower + AttackPower, ((int)myStats.ElementPowerCharacter), myStats.ElementalDamage[(int)myStats.ElementPowerCharacter].GetValue(), myStats.Accuracy.GetValue());
                Debug.Log(" ����� ���� ������� ������ " + FD);
                break;
        }



    }

    public void PlayAttackMouse1�alculation()
    {
        EnemyStats stats =playerController.instance.Focus.GetComponent<EnemyStats>();
        
            // Debug.Log($"DoDamage");
            int AttackPower = 0;
     
            PlayerAnim CurrentPlayerAnim = GetComponentInChildren<PlayerAnim>();


            if (Random.Range(1, 100) <= myStats.CriticalStrikeChance.GetValue())
            {
                AttackPower = myStats.CalculationCurrentAttackPowerFromStrength() + myStats.CalculationCurrentAttackPowerFromStrength() / 100 * myStats.CriticalHitPercentage.GetValue();
                //  Debug.Log(transform.name + " ������� ���� ���� � ������� " + AttackPower + " damage");


            }
            else
            {
                AttackPower = myStats.CalculationCurrentAttackPowerFromStrength();
                //  Debug.Log(transform.name + " ������� ���� � ������� " + AttackPower + " damage");

            }



            // Debug.Log(" attackIndex " + CurrentPlayerAnim.attackIndex);
            int FD = 0;
           
                    FD = stats.TakeNormalDamage(myStats.currentLvl, AttackPower, ((int)myStats.ElementPowerCharacter), myStats.ElementalDamage[(int)myStats.ElementPowerCharacter].GetValue(), myStats.Accuracy.GetValue());
                    Debug.Log(" ����� ���� ������� ������" + FD);
                    if (Inventory.instance.ProfileSlot[5] != null && FD != 0)
                    {

                        stats.PeriodicElementalDamage(myStats.timeDE, ((int)myStats.ElementPowerCharacter), FD, Inventory.instance.ProfileSlot[5].item.RandomPercentage, myStats.ElementalDebuffChance.GetValue());
                    }
                    Debug.Log($"TakeNormalDamage");
              


        
    }

    public void PlayAttackMouse1AtADistance()
    {
        if (attackCountdown)
        {
            if (Inventory.instance.ProfileSlot[4] != null)
            {
                Debug.Log($" �����  ������ {(float)Inventory.instance.ProfileSlot[4].item.specificEquipment}");
                animator.SetFloat("NumWeapon", (float)Inventory.instance.ProfileSlot[4].item.specificEquipment);
            }
            else
            {
                animator.SetFloat("NumWeapon", (float)0);
            }
            animator.SetTrigger("Attack");

            StartCoroutine(initialCountdownMetod());
            
        }
    }

    public void OnPlayAttack1Ability1(float SkillNumber)
    {
        animator.SetFloat("SkillNumber", SkillNumber);
        animator.SetFloat("NumWeapon", (float)Inventory.instance.ProfileSlot[4].item.specificEquipment);
        animator.SetTrigger("Attack");

    }

    public void OnPlayPuncture�alculation()
    {
        EnemyStats stats = playerController.instance.Focus.GetComponent<EnemyStats>();

        // Debug.Log($"DoDamage");
        int AttackPower = 0;

        PlayerAnim CurrentPlayerAnim = GetComponentInChildren<PlayerAnim>();


        if (Random.Range(1, 100) <= myStats.CriticalStrikeChance.GetValue())
        {
            AttackPower = myStats.CalculationCurrentAttackPowerFromStrength() + myStats.CalculationCurrentAttackPowerFromStrength() / 100 * myStats.CriticalHitPercentage.GetValue();
            //  Debug.Log(transform.name + " ������� ���� ���� � ������� " + AttackPower + " damage");


        }
        else
        {
            AttackPower = myStats.CalculationCurrentAttackPowerFromStrength();
            //  Debug.Log(transform.name + " ������� ���� � ������� " + AttackPower + " damage");

        }



        // Debug.Log(" attackIndex " + CurrentPlayerAnim.attackIndex);
        int FD = 0;
        
                FD = stats.TakeNormalDamage(myStats.currentLvl, AttackPower + AttackPower, ((int)myStats.ElementPowerCharacter), myStats.ElementalDamage[(int)myStats.ElementPowerCharacter].GetValue(), myStats.Accuracy.GetValue());
                Debug.Log(" ����� ���� ������� ������ " + FD);
               


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

    //public virtual void momentOfAttack()
    //{
    //  //  GetComponentInParent<PlayerCombat>().MOA();
    //}

    //public virtual void momentOfAttacks()
    //{
    //  //  GetComponentInParent<PlayerCombat>().MOAs();
    //}

    //����� ����� �������(�� ���� ������ ��� ��������)
    void ShootArrow()
    {
        GameObject arrow = Instantiate(playerController.instance.arrowPrefab, playerController.instance.bowTransform.position, playerController.instance.bowTransform.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = playerController.instance.bowTransform.forward * playerController.instance.arrowSpeed;

        // ���������� ������ ����� 4 �������
        Destroy(arrow, 2f);
    }

}
