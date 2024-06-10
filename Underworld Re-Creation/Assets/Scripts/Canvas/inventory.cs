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


public class inventory : MonoBehaviour
{
    #region Singleton

    public static inventory instance;

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
    public itemInCanvas draggenItem; // элемент перетаскивания

    public List<itemInCanvas> initialItems; // Список предметов для добавления при запуске(надо будет потом избавится и настроить загрузку от Items)
    public List<itemInCanvas> Items; // Список предметов которыми заправляет игрок
    
    bool OneUse; // требуется для загрузки, тоесть для того чтобы передать в инвентарь вещи из сохранения
  
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
    public bool CheckCellFree(Cell cell, itemSize size)
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
    public void CellsOcupation(Cell cell, itemSize size, bool isFree)
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
    public Vector2Int GetSize(itemSize size)
    {
        Vector2Int newSize = Vector2Int.zero;
        switch (size)
        {
            case itemSize.Smal:
                return newSize = Vector2Int.one;

            case itemSize.MediumVertical:
                return newSize = new Vector2Int(1, 2);

            case itemSize.MediumHorisontal:
                return newSize = new Vector2Int(2, 1);

            case itemSize.MediumSquare:
                return newSize = new Vector2Int(2, 2);

            case itemSize.Large:
                return newSize = new Vector2Int(2, 3);

        }
        return newSize = Vector2Int.zero;
    }
    //Раскрашивание ячеек
    public void CellsColorize(Cell cell, itemSize size, Color color)
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
        foreach (itemInCanvas itemPrefab in initialItems)
        {
            
            // Инстанцируем предмет из префаба
            itemInCanvas newItem = Instantiate(itemPrefab, transformItems); // Делаем инвентарь родительским объектом

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
                        CellsOcupation(cells[x, y], newItem.Size, false);
                        
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

    public void AddItem(itemInCanvas itemPrefab)
    {
        

            // Инстанцируем предмет из префаба
            itemInCanvas newItem = Instantiate(itemPrefab, transformItems); // Делаем инвентарь родительским объектом

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
                        CellsOcupation(cells[x, y], newItem.Size, false);

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
