using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/* Controls the player. Here we chose our "focus" and where to move. ��������� �������. ����� �� ������� ��� ������ � ���� ���������. */

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;

	public Interactable focus;  // Our current focus: Item, Enemy etc. ��� ������� �����: �������, ���� � �. �.

    public LayerMask movementMask;      // The ground �����
    public LayerMask interactionMask;   // Everything we can interact with ���, � ��� �� ����� �����������������

    PlayerMotor motor;      // Reference to our motor ������ �� ��� �����
    Camera cam;             // Reference to our camera ������ �� ���� ������

    public PlayerAnim intAnim;

    public bool Stan = false;
    public bool silence = true; // ���������� �������� �������� ����������� ������
    public bool OnOff = true; // ���������� ������� ����������� ����� �������� ������ ���� �� ��������� ����������
  //  public int KeyUsed=0;

    public Collider Area;
    public List<Enemy> AbilityArea;
    public bool focusAbility = false;

    public GameObject EnemyHP;
    public GameObject EnemyHPImage;
    public GameObject EnemyHPText;

    // Get references �������� ������������
    void Start ()
	{
        AbilityArea =new List<Enemy>();
        intAnim = GetComponentInChildren<PlayerAnim>();
        motor = GetComponent<PlayerMotor>();
		cam = Camera.main;
	}

    // Update is called once per frame ���������� ���������� ���� ��� �� ����
    void Update () {

		if (EventSystem.current.IsPointerOverGameObject())
			return;
        if (Stan)
        {

            GetComponent<CharacterCombat>().Stan();

        }
        else
        {
           

            //If we press left mouse ���� �� ������ ����� ������ ����
            if (Input.GetMouseButton(0))
            {
                // Shoot out a ray ������� �����
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we hit ���� �� ������
                if (Physics.Raycast(ray, out hit, movementMask))
                {
                    motor.MoveToPoint(hit.point);

                    SetFocus(null);
                }
            }

            if (OnOff)
            {


                // If we press right mouse ���� �� ������ ������ ������ ����
                if (Input.GetMouseButton(1))
                {

                   // KeyUsed = 0;
                    //��� ��������� �����, ��� ��� attackIndex ������ 0
                    intAnim.attackIndex = 0;
                    // Shoot out a ray ������� �����
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // If we hit ���� �� ������
                    if (Physics.Raycast(ray, out hit, 100f, interactionMask))
                    {
                        if (hit.collider.GetComponent<Interactable>().tag == "Enemy")
                        {
                            OnOff = false;
                           
                        }
                        SetFocus(hit.collider.GetComponent<Interactable>());
                       
                    }
                }


                if (silence)
                {
                    if (Input.GetButton("Panel1Ability"))
                    {
                        //�������� ������� ������
                        if (true)
                        {
                            //if(((int)EquipmentManager.instance.currentWeapon.WeaponequipSlot) == 0)
                            //{
                                focusAbility = false;

                                // KeyUsed = 1;
                                if (focusAbility)
                                {

                                    // ���� ����� ����� ������� ����� ������ ��������

                                    //attackIndex ������� �� ������
                                    intAnim.attackIndex = 1;
                                    // Shoot out a ray ������� �����
                                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit hit;

                                    // If we hit ���� �� ������
                                    if (Physics.Raycast(ray, out hit, 100f, interactionMask))
                                    {
                                        if (hit.collider.GetComponent<Interactable>().tag == "Enemy")
                                        {
                                            OnOff = false;
                                            SetFocus(hit.collider.GetComponent<Interactable>());

                                        }

                                    }

                                }
                                else
                                {
                                    //attackIndex ������� �� ������
                                    intAnim.attackIndex = 1;
                                    GetComponent<PlayerCombat>().AttackToEnemies();
                                }
                            //}
                           

                        }

                    }

                    if (Input.GetButton("Panel2Ability"))
                    {
                        //�������� ������� ������
                        if (true)
                        {
                            //������������� ����� ���� �����: �� ������ ����� / �� ������ � ����������� �������
                            focusAbility = true;

                           //KeyUsed = 2;
                            if (focusAbility)
                            {
                                
                                
                                // ���� ����� ����� ������� ����� ������ ��������

                                    //attackIndex ������� �� ������
                                    intAnim.attackIndex = 2;
                                    // Shoot out a ray ������� �����
                                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit hit;

                                    // If we hit ���� �� ������
                                    if (Physics.Raycast(ray, out hit, 100f, interactionMask))
                                    {
                                        if (hit.collider.GetComponent<Interactable>().tag == "Enemy")
                                        {
                                            OnOff = false;
                                            SetFocus(hit.collider.GetComponent<Interactable>());

                                        }

                                    }
                                
                            }
                            else
                            {
                                //�������� ������� ������
                                //attackIndex ������� �� ������
                                intAnim.attackIndex = 1;
                                GetComponent<PlayerCombat>().AttackToEnemies();

                            }


                        }
                    }


                }
            }


        }


    }


    

    // Set our focus to a new focus ��������� ���� �������� �� ����� �����
    void SetFocus (Interactable newFocus)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

        // If our focus has changed ���� ��� ����� ���������
        if (focus != newFocus && focus != null)
		{
            // Let our previous focus know that it's no longer being focused ����� ��� ���������� ����� �����, ��� �� ������ �� ������������
            focus.OnDefocused();
		}

        // Set our focus to what we hit ��������������� �� ���, ��� �� �������
        // If it's not an interactable, simply set it to null ���� ��� �� ������������, ������ ���������� ��� ���� �������� null
        focus = newFocus;

		if (focus != null)
		{
            // Let our focus know that it's being focused �������� ������ ������, ��� �� ��������� � ������
            focus.OnFocused(transform);
		}

	}

}
