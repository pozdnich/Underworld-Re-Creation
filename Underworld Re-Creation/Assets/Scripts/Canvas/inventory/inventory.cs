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
    //Удалённый доступ
    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    [SerializeField] public Transform transformTransform;  // указание облости инвенторя
    [SerializeField] public Transform transformItems;
    public int SizeX, SizeY; // Размер инвентаря
    public Cell cellPrefub; // образец одной клетки
    public Cell[,] cells; // двухмерный масив инвентаря
    public ItemInCanvas draggedItem; // элемент перетаскивания

    public List<ItemInCanvas> initialItems; // Список предметов для добавления при запуске(надо будет потом избавится и настроить загрузку от Items)
    public List<ItemInCanvas> Items; // Список предметов которыми заправляет игрок
    public ItemInCanvas[] ProfileSlot; //Слоты профиля

    bool OneUse; // требуется для загрузки, тоесть для того чтобы передать в инвентарь вещи из сохранения

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);  // делегат чтобы изменять характеристики при одевании и снятии предметов экипировки
    public event OnEquipmentChanged onEquipmentChanged;


    void Start()
    {
        cells = new Cell[SizeX, SizeY];
        CreateNewInventory();  

        OneUse = true;
    }

   
    // создание инвентаря
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
   
    //Костыль коратюн для адекватного расположения item
    private IEnumerator initialItemsMetod()
    {
        GetComponent<CanvasController>().inventoryUI.SetActive(!GetComponent<CanvasController>().inventoryUI.activeSelf);
        // Ждём одну секунду
        yield return new WaitForSeconds(0.00001f);

        // Вызываем OnDrop
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
   

    //Обновить цвет свободных клеток
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
    //Проверить ячейку на свободность 
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
    //пока перетаскиваем изначальные клетки белые, когда предмет лежит закрасить ему прилежащие клетки чёрным 
    public void CellsOccupation(Cell cell, ItemSize size, bool isFree)
    {
        if (cell == null)
        {
            Debug.LogError("CellsOcupation: ячейка (cell) равна null");
            return;
        }

        Vector2Int newSize = GetSize(size);
        for (int y = cell.y; y < cell.y + newSize.y; y++)
        {
            for (int x = cell.x; x < cell.x + newSize.x; x++)
            {
                if (x >= SizeX || y >= SizeY)
                {
                    Debug.LogError("CellsOcupation: попытка доступа за пределами массива");
                    continue;
                }

                if (cells[x, y] == null)
                {
                    Debug.LogError($"CellsOcupation: cells[{x}, {y}] равна null");
                    continue;
                }

                cells[x, y].isFree = isFree;

                if (cells[x, y].image == null)
                {
                    Debug.LogError($"CellsOcupation: cells[{x}, {y}].image равна null");
                    continue;
                }

                cells[x, y].image.color = isFree ? Color.white : Color.black;
            }
        }
    }



    //Вернуть размер предмета
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
    //Раскрашивание ячеек
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

    // Добавление начальных предметов
    public void AddInitialItems()
    {
        foreach (ItemInCanvas itemPrefab in initialItems)
        {
            
            // Инстанцируем предмет из префаба
            ItemInCanvas newItem = Instantiate(itemPrefab, transformItems); // Делаем инвентарь родительским объектом

            // Находим первую подходящую свободную ячейку для предмета
            bool nextInitialItem = false;
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                   

                    if (CheckCellFree(cells[x, y], newItem.Size))
                    {
                        
                        newItem.SetInitialPosition(newItem, cells[x, y]); // Устанавливаем начальную позицию
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
    //Добавление итемов из сохранения, но надо подумать стоит ли так оставлять
    public void AddItem(ItemInCanvas itemPrefab)
    {
        

            // Инстанцируем предмет из префаба
            ItemInCanvas newItem = Instantiate(itemPrefab, transformItems); // Делаем инвентарь родительским объектом

            // Находим первую подходящую свободную ячейку для предмета
            bool nextInitialItem = false;
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {


                    if (CheckCellFree(cells[x, y], newItem.Size))
                    {

                        newItem.SetInitialPosition(newItem, cells[x, y]); // Устанавливаем начальную позицию
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
