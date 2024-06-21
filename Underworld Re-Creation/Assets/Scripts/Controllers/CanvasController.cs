using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject ProfileUI;
    public GameObject SkillTree;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {

            inventoryUI.SetActive(!inventoryUI.activeSelf);
            
        }
        if (Input.GetButtonDown("Profile"))
        {

            ProfileUI.SetActive(!ProfileUI.activeSelf);

        }
        if (Input.GetButtonDown("SkillTree"))
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
