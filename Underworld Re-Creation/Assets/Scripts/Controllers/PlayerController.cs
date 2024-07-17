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
    // ------------------------������� ���������� ��������----------------------------------
    public Animator animatorForIR;
    public PlayerAnim playerAnim;
    NavMeshAgent agent;
    Vector3 _prevPosition;

    public GameObject dropPoint; // ����� ��� ����������� ��������
    public GameObject Focus; //��� ����������� ��� ���� �� ������� �� ������� �� ����� ������� �� ����� ��� ����� � ���������

    public SkinnedMeshRenderer[] currentMeshes; // ������ ����� ��������� ������� ������ ������ �� ���������
    public SkinnedMeshRenderer targetMesh; //��� ���������

    public PlayerStats playerStats; //�������� ��� ���������� ������� ������

    public GameObject EnemyHP;
    public GameObject EnemyHPImage;
    public GameObject EnemyHPText;

    // ------------------------������� ���������� ������----------------------------------
    public GameObject arrowPrefab; // ������ ������
    public Transform bowTransform; // �����, ������ ����������� ������
    public Transform StartThreeArrowBowTransform; // �����, ������ ����������� 3 ������
    public float arrowSpeed = 1f; // �������� ������ ������

    public bool OnOff = true; // ���������� ������� ����������� ����� �������� ������ ���� �� ��������� ����������
    public SkillSlot[] skillSlots; // ����� ������ � ������
    
    void Start()
    {
        animatorForIR = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerAnim = GetComponent<PlayerAnim>();
        playerStats = GetComponent<PlayerStats>();
        int numSlots = 7; // ���������� ������ �����
        currentMeshes = new SkinnedMeshRenderer[numSlots];
    }

    
    void Update()
    {
        //������ ��������� ��� �� ������ ������ ���������� ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; //



        //��� ������� ���� �������� ����� � ����� ��� ���� ������ ����
        if (Physics.Raycast(ray, out hit, 500) && Input.GetMouseButton(1))
        {

            if (hit.collider.CompareTag("Enemy")) //���� ������ ��� ����
            { 
                Focus = hit.collider.gameObject;
                float cr; // �������� ��� ����������� ������� ��� ��� �������
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
                        direction.y = 0; // ������� ������ �� ���������, ����� �������� �� ���������� �����/����
                        transform.rotation = Quaternion.LookRotation(direction);
                        Focus.GetComponent<Enemy>().ShowEnemyHP = ((float)4);
                        agent.Stop();//�������������� ��������� �����������
                        animatorForIR.SetBool("Run", false);// ���������� ����
                        playerAnim.PlayAttackMouse1();
                       
                    }
                    else
                    {

                        agent.SetDestination(hit.point);//������ ����������
                        animatorForIR.SetBool("Run", true);//��������� �������� ����
                        agent.Resume();// �������������� ������ �����������
                        
                    }
                    
                }
                else if (cr > 5 && cr < 11)
                {
                    // ������������ ��������� � ������� ����
                    Vector3 direction = (hit.point - transform.position).normalized;
                    direction.y = 0; // ������� ������ �� ���������, ����� �������� �� ���������� �����/����
                    transform.rotation = Quaternion.LookRotation(direction);
                    playerAnim.PlayAttackMouse1AtADistance();
                    
                }

            }
            else if (hit.collider.CompareTag("Item")) //���� ������ ��� �������
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

        if (Vector3.Distance(transform.position, agent.destination) < 0.3f )
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
                    direction.y = 0; // ������� ������ �� ���������, ����� �������� �� ���������� �����/����
                    transform.rotation = Quaternion.LookRotation(direction);
                    //������ ����� �� ������ ������ (���� ������� ��� ���� �����)
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

    bool AreCollidersTouching(Collider col1, Collider col2)
    {
        return col1.bounds.Intersects(col2.bounds);
    }

    ////����� ����� �������(�� ���� ������ ��� ��������)
    //void ShootArrow(Vector3 targetPosition)
    //{
    //    //// ������������ ��������� � ������� ����
    //    //Vector3 direction = (targetPosition - transform.position).normalized;
    //    //direction.y = 0; // ������� ������ �� ���������, ����� �������� �� ���������� �����/����
    //    //transform.rotation = Quaternion.LookRotation(direction);

    //    GameObject arrow = Instantiate(arrowPrefab, bowTransform.position, bowTransform.rotation);
    //    Rigidbody rb = arrow.GetComponent<Rigidbody>();
    //    rb.velocity = bowTransform.forward * arrowSpeed;

    //    // ���������� ������ ����� 4 �������
    //    Destroy(arrow, 2f);
    //}




}
