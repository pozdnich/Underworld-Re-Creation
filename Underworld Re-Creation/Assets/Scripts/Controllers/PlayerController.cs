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
    // ------------------------������� ���������� ��������----------------------------------
    public Animator animatorForIR;
    public PlayerAnim playerAnim;
    NavMeshAgent agent;
    Vector3 _prevPosition;

    public GameObject dropPoint; // ����� ��� ����������� ��������
    public GameObject Focus; //��� ����������� ��� ���� �� ������� �� ������� �� ����� ������� �� ����� ��� ����� � ���������

    public SkinnedMeshRenderer[] currentMeshes; // ������ ����� ��������� ������� ������ ������ �� ���������
    public SkinnedMeshRenderer targetMesh; //��� ���������

    // ------------------------������� ���������� ������----------------------------------
    public GameObject arrowPrefab; // ������ ������
    public Transform bowTransform; // �����, ������ ����������� ������
    public float arrowSpeed = 1f; // �������� ������ ������

    public bool OnOff = true; // ���������� ������� ����������� ����� �������� ������ ���� �� ��������� ����������
    public SkillSlot[] skillSlots; // ����� ������ � ������

    void Start()
    {
        animatorForIR = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerAnim = GetComponent<PlayerAnim>();
        int numSlots = 7; // ���������� ������ �����
        currentMeshes = new SkinnedMeshRenderer[numSlots];
    }

    
    void Update()
    {
        //������ ��������� ��� �� ������ ������ ���������� ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; //
        //��� ������� ���� �������� ����� � ����� ��� ���� ������ ����
        if (Physics.Raycast(ray, out hit,500)&&Input.GetMouseButton(1)) 
        {
            
             if (hit.collider.CompareTag("Enemy")) //���� ������ ��� ����
            {
                float cr;
                if (Inventory.instance.ProfileSlot[4] != null)
                {
                    cr = (float)Inventory.instance.ProfileSlot[4].item.specificEquipment;
                }
                else
                {
                    cr = -1;
                }
               
                playerAnim.PlayAttackMouse1(cr, hit);
            }else if (hit.collider.CompareTag("Item")) //���� ������ ��� �������
            {
                Focus = hit.collider.gameObject;
                Debug.Log("����� ���� �� �������");
                agent.SetDestination(hit.point);//������ ����������
                animatorForIR.SetBool("Run", true);//��������� �������� ����
                agent.Resume();// �������������� ������ �����������
            }
            else //� ���� ������ ������ �������� � ��������� �����
            {
                agent.SetDestination(hit.point);//������ ����������
                animatorForIR.SetBool("Run", true);//��������� �������� ����
                agent.Resume();// �������������� ������ �����������
            }
            

        }

        if (Vector3.Distance(transform.position, agent.destination) < 0.1f)
        {
            agent.Stop();//�������������� ��������� �����������
            animatorForIR.SetBool("Run", false);// ���������� ����
        }

        // ������� � ������ ��� ��������� �������� (����� - ���)
        Vector3 curMove = transform.position - _prevPosition;
        float curSpeed = curMove.magnitude / Time.deltaTime;
        _prevPosition = transform.position;
        animatorForIR.SetFloat("SpeedPlayer", curSpeed);

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
                    //������ ����� �� ������ ������ (���� ������� ��� ���� �����)
                    playerAnim.OnPlayAttack1Ability1(skillSlots[0].skill.SkillNumber);
                   
                }
            }

        }

    }
    // ����� �������� ��� ��������
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
    // �����  ������� ������ ��� ��������
    public void DestroyAttachToMesh(SkinnedMeshRenderer mesh, int slotIndex)
    {
        if (currentMeshes[slotIndex] != null)
        {
            Destroy(currentMeshes[slotIndex].gameObject);
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
