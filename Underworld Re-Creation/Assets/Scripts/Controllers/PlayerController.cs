using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class playerController : MonoBehaviour
{
    
    Animator animator;
    NavMeshAgent agent;

    Vector3 _prevPosition;
    
   
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        //захват координат где на данный момент находиться мышь
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; //
        //при нажатии мыши персонаж бежит в точку где была нажата мышь
        if (Physics.Raycast(ray, out hit,500)&&Input.GetMouseButtonDown(1)) 
        {         
            agent.SetDestination(hit.point);//вводим координаты
            animator.SetBool("Run", true);//запускаем анимацию бега
            agent.Resume();// принудительный запуск перемещения

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
    }
}
