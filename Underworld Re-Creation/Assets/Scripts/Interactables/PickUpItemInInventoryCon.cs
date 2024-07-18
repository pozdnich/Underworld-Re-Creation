using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInInventoryCon : MonoBehaviour
{
    public void Start()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("����� ������������� � ����������!");
        // ���������, ������������� �� ��������� ������ � ���������
        if (other.CompareTag("Player"))
        {
            Debug.Log("����� ������������� � ���������!");
            playerController player = other.GetComponent<playerController>();
            if (player != null && player.Focus == gameObject)
            {
                Debug.Log("������� ��� ���� � �����");
                Inventory.instance.AddItemCon(GetComponentInChildren<itemInCanvasCon>());

               
                Destroy(gameObject);
            }
        }
    }
}
