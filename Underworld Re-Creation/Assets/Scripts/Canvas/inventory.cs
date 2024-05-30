using sc.terrain.vegetationspawner;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using UnityEngine;


public class inventory : MonoBehaviour
{
    [SerializeField] private Transform transformTransform;  // указание облости инвенторя
    public int SizeX, SizeY; // Размер инвентаря
    public Cell cellPrefub; // образец одной клетки
    public Cell[,] cells; // двухмерный масив инвентаря
    public item draggenItem; // элемент перетаскивания
    void Start()
    {
        cells = new Cell[SizeX, SizeY];
        CreateNewInventory();
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
                newCell.inventory =this;
                newCell.CellIndex.text = x + " " + y;

                cells[x, y] = newCell;
            }
        }
    }

    void Update()
    {
        
    }
    //Обновить цвет свободных клеток
    public void UpdateCellsColor()
    {
       for(int y = 0; y < SizeY;y++)
        {
            for( int x = 0;x < SizeX;x++)
            {
                if(cells[x, y].isFree)
                {
                    cells[x,y].image.color = Color.red;
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
        for(int y=cell.y; y < cell.y + newSize.y; y++)
        {
            for(int x =cell.x; x< cell.x + newSize.x; x++)
            {
                if(x+1<=SizeX && y + 1 <= SizeY)
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
    public void CellsOcupation(Cell cell, itemSize size, bool ordered)
    {
        Vector2Int newSize = GetSize(size);
        for (int y = cell.y; y < cell.y+newSize.y; y++)
        {
            for (int x = cell.x; x < cell.x+newSize.x; x++)
            {
                cells[x, y].isFree = ordered;
                if (ordered)
                {
                    cells[x, y].image.color = Color.white;
                }
                else
                {
                    cells[x, y].image.color = Color.black;
                }
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
               
            case itemSize.MediumHorisontal:
                return newSize = new Vector2Int(1, 2);
               
            case itemSize.MediumVertical:
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
}
