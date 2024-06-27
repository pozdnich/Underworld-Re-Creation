using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class CanvasController : MonoBehaviour
{


    public GameObject inventoryUI; // обьект инвентарь
    public GameObject ProfileUI; // обьект профиль
    public GameObject SkillTree; // обьект древо цмений
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")) // открытие/закрытие инвентаря
        {

            inventoryUI.SetActive(!inventoryUI.activeSelf);

        }
        if (Input.GetButtonDown("Profile"))// открытие/закрытие профиля
        {

            ProfileUI.SetActive(!ProfileUI.activeSelf);

        }
        if (Input.GetButtonDown("SkillTree"))// открытие/закрытие древа умений
        {
            if (inventoryUI.active)
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
            }
            if (ProfileUI.active)
            {
                ProfileUI.SetActive(!ProfileUI.activeSelf);
            }

            SkillTree.SetActive(!SkillTree.activeSelf);

        }
    }

}