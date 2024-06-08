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

    public GameObject arrowPrefab; // ������ ������
    public Transform bowTransform; // �����, ������ ����������� ������
    public float arrowSpeed = 1f; // �������� ������ ������

    public GameObject Focus; //��� ����������� ��� ���� �� ������� �� ������� �� ����� ������� �� ����� ��� ����� � ���������

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


            Debug.Log(hit.collider.tag);
            // ���� ������ �� �������
            if (hit.collider.CompareTag("Item"))
            {
                Focus = hit.collider.gameObject;
                Debug.Log("����� ���� �� �������");
            }
            else
            {
                Focus = null;
            }


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

        if (Physics.Raycast(ray, out hit, 500) && Input.GetButtonDown("Ability1"))
        {
            ShootArrow(hit.point);

        }

    }
    //����� ����� �������(�� ���� ������ ��� ��������)
    void ShootArrow(Vector3 targetPosition)
    {
        // ������������ ��������� � ������� ����
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // ������� ������ �� ���������, ����� �������� �� ���������� �����/����
        transform.rotation = Quaternion.LookRotation(direction);

        GameObject arrow = Instantiate(arrowPrefab, bowTransform.position, bowTransform.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = bowTransform.forward * arrowSpeed;

        // ���������� ������ ����� 4 �������
        Destroy(arrow, 2f);
    }


    
   
}
