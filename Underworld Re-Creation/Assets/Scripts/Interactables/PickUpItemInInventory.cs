using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInInventory : MonoBehaviour
{
    

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
                Inventory.instance.AddItem(GetComponentInChildren<ItemInCanvas>());
                // ����� ������� � ���������������
                Debug.Log("����� ������� � ��������� �������!");
                Destroy(gameObject);
            }
        }
    }
}
