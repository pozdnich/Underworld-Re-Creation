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
        //������ ��������� ��� �� ������ ������ ���������� ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; //
        //��� ������� ���� �������� ����� � ����� ��� ���� ������ ����
        if (Physics.Raycast(ray, out hit,500)&&Input.GetMouseButtonDown(1)) 
        {         
            agent.SetDestination(hit.point);//������ ����������
            animator.SetBool("Run", true);//��������� �������� ����
            agent.Resume();// �������������� ������ �����������

        }

        if (Vector3.Distance(transform.position, agent.destination) < 0.1f)
        {
            agent.Stop();//�������������� ��������� �����������
            animator.SetBool("Run", false);// ���������� ����
        }

        // ������� � ������ ��� ��������� �������� (����� - ���)
        Vector3 curMove = transform.position - _prevPosition;
        float curSpeed = curMove.magnitude / Time.deltaTime;
        _prevPosition = transform.position;
        animator.SetFloat("SpeedPlayer", curSpeed);
    }
}
