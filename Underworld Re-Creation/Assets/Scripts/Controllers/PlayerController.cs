using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class playerController : MonoBehaviour
{
    
    Animator animator;
    NavMeshAgent agent;

    Vector3 _prevPosition;

    public GameObject arrowPrefab; // ѕрефаб стрелы
    public Transform bowTransform; // “очка, откуда выпускаетс€ стрела
    public float arrowSpeed = 1f; // —корость полета стрелы

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        //захват координат где на данный момент находитьс€ мышь
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; //
        //при нажатии мыши персонаж бежит в точку где была нажата мышь
        if (Physics.Raycast(ray, out hit,500)&&Input.GetMouseButtonDown(1)) 
        {         
            agent.SetDestination(hit.point);//вводим координаты
            animator.SetBool("Run", true);//запускаем анимацию бега
            agent.Resume();// принудительный запуск перемещени€

        }

        if (Vector3.Distance(transform.position, agent.destination) < 0.1f)
        {
            agent.Stop();//принудительна€ остановка перемещени€
            animator.SetBool("Run", false);// отклучение бега
        }

        // переход в дереве дл€ плавности анимации (отдых - бег)
        Vector3 curMove = transform.position - _prevPosition;
        float curSpeed = curMove.magnitude / Time.deltaTime;
        _prevPosition = transform.position;
        animator.SetFloat("SpeedPlayer", curSpeed);

        if (Physics.Raycast(ray, out hit, 500) && Input.GetButtonDown("Ability1"))
        {
            ShootArrow(hit.point);

        }

    }
    //метод полЄта обьекта(по типу стрелы или фаербола)
    void ShootArrow(Vector3 targetPosition)
    {
        // ѕоворачиваем персонажа в сторону цели
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // ”бираем наклон по вертикали, чтобы персонаж не наклон€лс€ вверх/вниз
        transform.rotation = Quaternion.LookRotation(direction);

        GameObject arrow = Instantiate(arrowPrefab, bowTransform.position, bowTransform.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = bowTransform.forward * arrowSpeed;

        // ”ничтожаем стрелу через 4 секунды
        Destroy(arrow, 2f);
    }
}
