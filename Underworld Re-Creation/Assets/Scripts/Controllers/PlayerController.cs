using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


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
    Animator animator;
    NavMeshAgent agent;
    Vector3 _prevPosition;

    public GameObject dropPoint; // точка для выкидывания предмета
    public GameObject Focus; //для определения что если мы указали на предмет то когда подойдём то нужно его взять в инвентарь

    public SkinnedMeshRenderer[] currentMeshes;
    public SkinnedMeshRenderer targetMesh;

    // ------------------------Область Параметров Абилок----------------------------------
    public GameObject arrowPrefab; // Префаб стрелы
    public Transform bowTransform; // Точка, откуда выпускается стрела
    public float arrowSpeed = 1f; // Скорость полета стрелы

    public bool OnOff = true; // переменная которая недомускает запус следущей абилки если не закончена придедущая
    public SkillSlot[] skillSlots;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        int numSlots = 7;
        currentMeshes = new SkinnedMeshRenderer[numSlots];
    }

    
    void Update()
    {
        //захват координат где на данный момент находиться мышь
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; //
        //при нажатии мыши персонаж бежит в точку где была нажата мышь
        if (Physics.Raycast(ray, out hit,500)&&Input.GetMouseButton(1)) 
        {         
            agent.SetDestination(hit.point);//вводим координаты
            animator.SetBool("Run", true);//запускаем анимацию бега
            agent.Resume();// принудительный запуск перемещения


            Debug.Log(hit.collider.tag);
            // Если нажали на предмет
            if (hit.collider.CompareTag("Item"))
            {
                Focus = hit.collider.gameObject;
                Debug.Log("Фокус взят на предмет");
            }
            else
            {
                Focus = null;
            }


        }

        if (Vector3.Distance(transform.position, agent.destination) < 0.1f)
        {
            agent.Stop();//принудительная остановка перемещения
            animator.SetBool("Run", false);// отклучение бега
        }

        // переход в дереве для плавности анимации (отдых - бег)
        Vector3 curMove = transform.position - _prevPosition;
        float curSpeed = curMove.magnitude / Time.deltaTime;
        _prevPosition = transform.position;
        animator.SetFloat("SpeedPlayer", curSpeed);

        if (Physics.Raycast(ray, out hit, 500) && Input.GetButtonDown("Ability1"))
        {
            // ShootArrow(hit.point);
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
                    animator.SetFloat("SkillNumber", skillSlots[0].skill.SkillNumber);
                    animator.SetFloat("NumWeapon", (float)Inventory.instance.ProfileSlot[4].item.specificEquipment);
                    animator.SetTrigger("Attack");
                }
            }

        }

    }

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



    //метод полёта обьекта(по типу стрелы или фаербола)
    void ShootArrow(Vector3 targetPosition)
    {
        // Поворачиваем персонажа в сторону цели
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Убираем наклон по вертикали, чтобы персонаж не наклонялся вверх/вниз
        transform.rotation = Quaternion.LookRotation(direction);

        GameObject arrow = Instantiate(arrowPrefab, bowTransform.position, bowTransform.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = bowTransform.forward * arrowSpeed;

        // Уничтожаем стрелу через 4 секунды
        Destroy(arrow, 2f);
    }


    
   
}
