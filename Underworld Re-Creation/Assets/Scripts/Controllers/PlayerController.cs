using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public class playerController : MonoBehaviour
{
    #region Singleton

    public static playerController instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    // ------------------------Область Параметров Обьектов----------------------------------
    public Animator animatorForIR;
    public PlayerAnim playerAnim;
    NavMeshAgent agent;
    Vector3 _prevPosition;

    public GameObject dropPoint; // точка для выкидывания предмета
    public GameObject Focus; //для определения что если мы указали на предмет то когда подойдём то нужно его взять в инвентарь

    public SkinnedMeshRenderer[] currentMeshes; // массив мешей предметов которые должны весеть на персонаже
    public SkinnedMeshRenderer targetMesh; //меш персонажа

    public PlayerStats playerStats; //параметр для управления статами игрока

    public GameObject EnemyHP;
    public GameObject EnemyHPImage;
    public GameObject EnemyHPText;

    // ------------------------Область Параметров Абилок----------------------------------
    public GameObject arrowPrefab; // Префаб стрелы
    public Transform bowTransform; // Точка, откуда выпускается стрела
    public Transform StartThreeArrowBowTransform; // Точка, откуда выпускается 3 стрелы
    public float arrowSpeed = 1f; // Скорость полета стрелы

    public bool OnOff = true; // переменная которая недомускает запус следущей абилки если не закончена придедущая
    public SkillSlot[] skillSlots; // слоты умений у игрока
    
    void Start()
    {
        animatorForIR = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerAnim = GetComponent<PlayerAnim>();
        playerStats = GetComponent<PlayerStats>();
        int numSlots = 7; // количество слотов мешей
        currentMeshes = new SkinnedMeshRenderer[numSlots];
    }

    
    void Update()
    {
        //захват координат где на данный момент находиться мышь
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; //



        //при нажатии мыши персонаж бежит в точку где была нажата мышь
        if (Physics.Raycast(ray, out hit, 500) && Input.GetMouseButton(1))
        {

            if (hit.collider.CompareTag("Enemy")) //если обьект это враг
            { 
                Focus = hit.collider.gameObject;
                float cr; // параметр для определения дальний бой или ближний
                if (Inventory.instance.ProfileSlot[4] != null)
                {
                    cr = (float)Inventory.instance.ProfileSlot[4].item.specificEquipment;
                }
                else
                {
                    cr = -1;
                }
                
                if (cr > 2 && cr < 6 || cr == -1)
                {
                    Vector3 direction = (hit.collider.gameObject.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.2f);

                    if (playerAnim.EnemyAndPlayerCollidersTouching && AreCollidersTouching(this.GetComponent<Collider>(), Focus.GetComponent<Collider>()))
                    {
                        direction = (hit.point - transform.position).normalized;
                        direction.y = 0; // Убираем наклон по вертикали, чтобы персонаж не наклонялся вверх/вниз
                        transform.rotation = Quaternion.LookRotation(direction);
                        Focus.GetComponent<Enemy>().ShowEnemyHP = ((float)4);
                        agent.Stop();//принудительная остановка перемещения
                        animatorForIR.SetBool("Run", false);// отклучение бега
                        playerAnim.PlayAttackMouse1();
                       
                    }
                    else
                    {

                        agent.SetDestination(hit.point);//вводим координаты
                        animatorForIR.SetBool("Run", true);//запускаем анимацию бега
                        agent.Resume();// принудительный запуск перемещения
                        
                    }
                    
                }
                else if (cr > 5 && cr < 11)
                {
                    // Поворачиваем персонажа в сторону цели
                    Vector3 direction = (hit.point - transform.position).normalized;
                    direction.y = 0; // Убираем наклон по вертикали, чтобы персонаж не наклонялся вверх/вниз
                    transform.rotation = Quaternion.LookRotation(direction);
                    playerAnim.PlayAttackMouse1AtADistance();
                    
                }

            }
            else if (hit.collider.CompareTag("Item")) //если обьект это предмет
            {
                Focus = hit.collider.gameObject;
                Debug.Log("Фокус взят на предмет");
                agent.SetDestination(hit.point);//вводим координаты
                animatorForIR.SetBool("Run", true);//запускаем анимацию бега
                agent.Resume();// принудительный запуск перемещения
            }
            else //в ином случае просто движение в указанную точку
            {
                agent.SetDestination(hit.point);//вводим координаты
                animatorForIR.SetBool("Run", true);//запускаем анимацию бега
                agent.Resume();// принудительный запуск перемещения
            }

        }

        if (Vector3.Distance(transform.position, agent.destination) < 0.3f )
        {
            agent.Stop();//принудительная остановка перемещения
            animatorForIR.SetBool("Run", false);// отклучение бега
           
        }

        // переход в дереве для плавности анимации (отдых - бег)
        Vector3 curMove = transform.position - _prevPosition;
        float curSpeed = curMove.magnitude / Time.deltaTime;
        _prevPosition = transform.position;
        animatorForIR.SetFloat("SpeedPlayer", curSpeed);

        if (Physics.Raycast(ray, out hit, 500) && Input.GetButtonDown("Ability1"))
        {
           
            if (Inventory.instance.ProfileSlot[4]!=null && skillSlots[0].skill!=null)
            {
                bool DoesTheWeaponMatch = false;
                foreach (SpecificEquipment se in skillSlots[0].skill.NecessaryEquipment)
                {
                    if(se== Inventory.instance.ProfileSlot[4].item.specificEquipment)
                    {
                        DoesTheWeaponMatch=true;
                        break;
                    }
                }
                if (DoesTheWeaponMatch)
                {
                    Vector3 direction = (hit.point - transform.position).normalized;
                    direction.y = 0; // Убираем наклон по вертикали, чтобы персонаж не наклонялся вверх/вниз
                    transform.rotation = Quaternion.LookRotation(direction);
                    //запуск скила из первой ячейки (надо сделать для всех ячеек)
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Focus = hit.collider.gameObject;
                        Focus.GetComponent<Enemy>().ShowEnemyHP = ((float)4);
                        playerAnim.OnPlayAttack1Ability1(skillSlots[0].skill.SkillNumber);
                    }
                    
                   
                }
            }

        }

    }
    // метод одевания меш предмета
    public void AttachToMesh(SkinnedMeshRenderer mesh, int slotIndex)
    {
        if (currentMeshes[slotIndex] != null)
        {
            Destroy(currentMeshes[slotIndex].gameObject);
        }

        SkinnedMeshRenderer newMesh = Instantiate(mesh) as SkinnedMeshRenderer;
        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }
    // метод  полного снятия меш предмета
    public void DestroyAttachToMesh(SkinnedMeshRenderer mesh, int slotIndex)
    {
        if (currentMeshes[slotIndex] != null)
        {
            Destroy(currentMeshes[slotIndex].gameObject);
        }

    }

    bool AreCollidersTouching(Collider col1, Collider col2)
    {
        return col1.bounds.Intersects(col2.bounds);
    }

    ////метод полёта обьекта(по типу стрелы или фаербола)
    //void ShootArrow(Vector3 targetPosition)
    //{
    //    //// Поворачиваем персонажа в сторону цели
    //    //Vector3 direction = (targetPosition - transform.position).normalized;
    //    //direction.y = 0; // Убираем наклон по вертикали, чтобы персонаж не наклонялся вверх/вниз
    //    //transform.rotation = Quaternion.LookRotation(direction);

    //    GameObject arrow = Instantiate(arrowPrefab, bowTransform.position, bowTransform.rotation);
    //    Rigidbody rb = arrow.GetComponent<Rigidbody>();
    //    rb.velocity = bowTransform.forward * arrowSpeed;

    //    // Уничтожаем стрелу через 4 секунды
    //    Destroy(arrow, 2f);
    //}




}
