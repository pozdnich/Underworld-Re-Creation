using sc.terrain.vegetationspawner;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class Inventory : MonoBehaviour
{
    //�������� ������
    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    [SerializeField] public Transform transformTransform;  // �������� ������� ���������
    [SerializeField] public Transform transformItems;
    public int SizeX, SizeY; // ������ ���������
    public Cell cellPrefub; // ������� ����� ������
    public Cell[,] cells; // ���������� ����� ���������
    public ItemInCanvas draggedItem; // ������� ��������������

    public List<ItemInCanvas> initialItems; // ������ ��������� ��� ���������� ��� �������(���� ����� ����� ��������� � ��������� �������� �� Items)
    public List<ItemInCanvas> Items; // ������ ��������� �������� ���������� �����
    public ItemInCanvas[] ProfileSlot; //����� �������

    bool OneUse; // ��������� ��� ��������, ������ ��� ���� ����� �������� � ��������� ���� �� ����������

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);  // ������� ����� �������� �������������� ��� �������� � ������ ��������� ����������
    public event OnEquipmentChanged onEquipmentChanged;


    void Start()
    {
        cells = new Cell[SizeX, SizeY];
        CreateNewInventory();  

        OneUse = true;
    }

   
    // �������� ���������
    private void CreateNewInventory()
    {
        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                var newCell = Instantiate(cellPrefub, transformTransform);
                newCell.name = x + " " + y;
                newCell.x = x;
                newCell.y = y;
                newCell.isFree = true;
                newCell.inventory = this;
                newCell.CellIndex.text = x + " " + y;

                cells[x, y] = newCell;
            }
        }
       
    }
   
    //������� ������� ��� ����������� ������������ item
    private IEnumerator initialItemsMetod()
    {
        GetComponent<CanvasController>().inventoryUI.SetActive(!GetComponent<CanvasController>().inventoryUI.activeSelf);
        // ��� ���� �������
        yield return new WaitForSeconds(0.00001f);

        // �������� OnDrop
        AddInitialItems();
        GetComponent<CanvasController>().inventoryUI.SetActive(!GetComponent<CanvasController>().inventoryUI.activeSelf);
    }
    void Update()
    {
        if (OneUse)
        {

            StartCoroutine(initialItemsMetod());
            OneUse = false;
            
        }
       
    }
   

    //�������� ���� ��������� ������
    public void UpdateCellsColor()
    {
        for (int y = 0; y < SizeY; y++)
        {
            for (int x = 0; x < SizeX; x++)
            {
                if (cells[x, y].isFree)
                {
                    cells[x, y].image.color = Color.gray;
                }
                else
                {
                    cells[x, y].image.color = Color.black;
                }

            }
        }
    }
    //��������� ������ �� ����������� 
    public bool CheckCellFree(Cell cell, ItemSize size)
    {
        Vector2Int newSize = GetSize(size);
        for (int y = cell.y; y < cell.y + newSize.y; y++)
        {
            for (int x = cell.x; x < cell.x + newSize.x; x++)
            {
                if (x + 1 <= SizeX && y + 1 <= SizeY)
                {
                    if (!cells[x, y].isFree)
                    {
                        return false;
                    }
                }
                if (x + 1 > SizeX || y + 1 > SizeY)
                {

                    return false;

                }
            }
        }
        return true;
    }
    //���� ������������� ����������� ������ �����, ����� ������� ����� ��������� ��� ���������� ������ ������ 
    public void CellsOccupation(Cell cell, ItemSize size, bool isFree)
    {
        if (cell == null)
        {
            Debug.LogError("CellsOcupation: ������ (cell) ����� null");
            return;
        }

        Vector2Int newSize = GetSize(size);
        for (int y = cell.y; y < cell.y + newSize.y; y++)
        {
            for (int x = cell.x; x < cell.x + newSize.x; x++)
            {
                if (x >= SizeX || y >= SizeY)
                {
                    Debug.LogError("CellsOcupation: ������� ������� �� ��������� �������");
                    continue;
                }

                if (cells[x, y] == null)
                {
                    Debug.LogError($"CellsOcupation: cells[{x}, {y}] ����� null");
                    continue;
                }

                cells[x, y].isFree = isFree;

                if (cells[x, y].image == null)
                {
                    Debug.LogError($"CellsOcupation: cells[{x}, {y}].image ����� null");
                    continue;
                }

                cells[x, y].image.color = isFree ? Color.white : Color.black;
            }
        }
    }



    //������� ������ ��������
    public Vector2Int GetSize(ItemSize size)
    {
        Vector2Int newSize = Vector2Int.zero;
        switch (size)
        {
            case ItemSize.Small:
                return newSize = Vector2Int.one;

            case ItemSize.MediumVertical:
                return newSize = new Vector2Int(1, 2);

            case ItemSize.MediumHorizontal:
                return newSize = new Vector2Int(2, 1);

            case ItemSize.MediumSquare:
                return newSize = new Vector2Int(2, 2);

            case ItemSize.Large:
                return newSize = new Vector2Int(2, 3);

        }
        return newSize = Vector2Int.zero;
    }
    //������������� �����
    public void CellsColorize(Cell cell, ItemSize size, Color color)
    {
        Vector2Int newSize = GetSize(size);

        for (int y = cell.y; y < cell.y + newSize.y; y++)
        {
            for (int x = cell.x; x < cell.x + newSize.x; x++)
            {
                if (x + 1 <= SizeX && y + 1 <= SizeY)
                {
                    cells[x, y].image.color = color;
                }

            }
        }

    }

    // ���������� ��������� ���������
    public void AddInitialItems()
    {
        foreach (ItemInCanvas itemPrefab in initialItems)
        {
            
            // ������������ ������� �� �������
            ItemInCanvas newItem = Instantiate(itemPrefab, transformItems); // ������ ��������� ������������ ��������

            // ������� ������ ���������� ��������� ������ ��� ��������
            bool nextInitialItem = false;
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                   

                    if (CheckCellFree(cells[x, y], newItem.Size))
                    {
                        
                        newItem.SetInitialPosition(newItem, cells[x, y]); // ������������� ��������� �������
                        newItem.PrevCell = cells[x, y];
                        Items.Add(newItem);
                        CellsOccupation(cells[x, y], newItem.Size, false);
                        
                        nextInitialItem = true;

                        break;
                    }
                }
                if (nextInitialItem)
                {
                    
                    break;
                }
            }
        }
        UpdateCellsColor();
    }
    //���������� ������ �� ����������, �� ���� �������� ����� �� ��� ���������
    public void AddItem(ItemInCanvas itemPrefab)
    {
        

            // ������������ ������� �� �������
            ItemInCanvas newItem = Instantiate(itemPrefab, transformItems); // ������ ��������� ������������ ��������

            // ������� ������ ���������� ��������� ������ ��� ��������
            bool nextInitialItem = false;
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {


                    if (CheckCellFree(cells[x, y], newItem.Size))
                    {

                        newItem.SetInitialPosition(newItem, cells[x, y]); // ������������� ��������� �������
                        newItem.PrevCell = cells[x, y];
                        Items.Add(newItem);
                        CellsOccupation(cells[x, y], newItem.Size, false);

                        nextInitialItem = true;

                        break;
                    }
                }
                if (nextInitialItem)
                {

                    break;
                }
            }
        
        UpdateCellsColor();
    }
}
