using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/* Controls the player. Here we chose our "focus" and where to move. Управляет игроком. Здесь мы выбрали наш «фокус» и куда двигаться. */

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;

	public Interactable focus;  // Our current focus: Item, Enemy etc. Наш текущий фокус: предмет, враг и т. д.

    public LayerMask movementMask;      // The ground Земля
    public LayerMask interactionMask;   // Everything we can interact with Все, с чем мы можем взаимодействовать

    PlayerMotor motor;      // Reference to our motor Ссылка на наш мотор
    Camera cam;             // Reference to our camera ссылка на нашу камеру

    public PlayerAnim intAnim;

    public bool Stan = false;
    public bool silence = true; // переменная проверки действия негативного эфекта
    public bool OnOff = true; // переменная которая недомускает запус следущей абилки если не закончена придедущая
  //  public int KeyUsed=0;

    public Collider Area;
    public List<Enemy> AbilityArea;
    public bool focusAbility = false;

    public GameObject EnemyHP;
    public GameObject EnemyHPImage;
    public GameObject EnemyHPText;

    // Get references Получить рекомендации
    void Start ()
	{
        AbilityArea =new List<Enemy>();
        intAnim = GetComponentInChildren<PlayerAnim>();
        motor = GetComponent<PlayerMotor>();
		cam = Camera.main;
	}

    // Update is called once per frame Обновление вызывается один раз за кадр
    void Update () {

		if (EventSystem.current.IsPointerOverGameObject())
			return;
        if (Stan)
        {

            GetComponent<CharacterCombat>().Stan();

        }
        else
        {
           

            //If we press left mouse Если мы нажмем левую кнопку мыши
            if (Input.GetMouseButton(0))
            {
                // Shoot out a ray Стреляй лучом
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we hit Если мы ударим
                if (Physics.Raycast(ray, out hit, movementMask))
                {
                    motor.MoveToPoint(hit.point);

                    SetFocus(null);
                }
            }

            if (OnOff)
            {


                // If we press right mouse Если мы нажмем правую кнопку мыши
                if (Input.GetMouseButton(1))
                {

                   // KeyUsed = 0;
                    //это дефолтная атака, так что attackIndex всегда 0
                    intAnim.attackIndex = 0;
                    // Shoot out a ray Стреляй лучом
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // If we hit Если мы ударим
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
                        //Проверка наличия Абилки
                        if (true)
                        {
                            //if(((int)EquipmentManager.instance.currentWeapon.WeaponequipSlot) == 0)
                            //{
                                focusAbility = false;

                                // KeyUsed = 1;
                                if (focusAbility)
                                {

                                    // Сюда можно будет вбивать номер нужной анимации

                                    //attackIndex зависит от абилки
                                    intAnim.attackIndex = 1;
                                    // Shoot out a ray Стреляй лучом
                                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit hit;

                                    // If we hit Если мы ударим
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
                                    //attackIndex зависит от абилки
                                    intAnim.attackIndex = 1;
                                    GetComponent<PlayerCombat>().AttackToEnemies();
                                }
                            //}
                           

                        }

                    }

                    if (Input.GetButton("Panel2Ability"))
                    {
                        //Проверка наличия Абилки
                        if (true)
                        {
                            //определяеться выбор типа атаки: На одного врага / На врагов в определённой облости
                            focusAbility = true;

                           //KeyUsed = 2;
                            if (focusAbility)
                            {
                                
                                
                                // Сюда можно будет вбивать номер нужной анимации

                                    //attackIndex зависит от абилки
                                    intAnim.attackIndex = 2;
                                    // Shoot out a ray Стреляй лучом
                                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit hit;

                                    // If we hit Если мы ударим
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
                                //Проверка наличия Абилки
                                //attackIndex зависит от абилки
                                intAnim.attackIndex = 1;
                                GetComponent<PlayerCombat>().AttackToEnemies();

                            }


                        }
                    }


                }
            }


        }


    }


    

    // Set our focus to a new focus Направьте наше внимание на новый фокус
    void SetFocus (Interactable newFocus)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

        // If our focus has changed Если наш фокус изменился
        if (focus != newFocus && focus != null)
		{
            // Let our previous focus know that it's no longer being focused Пусть наш предыдущий фокус знает, что он больше не сфокусирован
            focus.OnDefocused();
		}

        // Set our focus to what we hit Сосредоточьтесь на том, что мы ударили
        // If it's not an interactable, simply set it to null Если это не интерактивно, просто установите для него значение null
        focus = newFocus;

		if (focus != null)
		{
            // Let our focus know that it's being focused Сообщаем нашему фокусу, что он находится в фокусе
            focus.OnFocused(transform);
		}

	}

}
