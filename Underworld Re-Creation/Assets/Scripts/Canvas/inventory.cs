using sc.terrain.vegetationspawner;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using UnityEngine;


public class inventory : MonoBehaviour
{
    [SerializeField] private Transform transformTransform;
    public int SizeX, SizeY;
    public Cell cellPrefub;
    public Cell[,] cells;
    public item draggenItem;
    void Start()
    {
        cells = new Cell[SizeX, SizeY];
        CreateNewInventory();
    }

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
            }
        }
    }

    public bool CheckCellFree(Cell cell, itemSize size)
    {
        Vector2Int newSize = GetSize(size);
        for(int y=cell.y; y < cell.y + newSize.y; y++)
        {
            for(int x =cell.x; x< cell.x + newSize.x; x++)
            {
                if(x+1<=SizeX && y + 1 <= SizeY)
                {
                    if (!cells[x,y].isFree)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void CellsOcupation(Cell cell, itemSize size, bool ordered)
    {
        Vector2Int newSize = GetSize(size);
        for (int y = cell.y; y < cell.y+newSize.y; y++)
        {
            for (int x = cell.x; x < cell.x+newSize.x; x++)
            {
                cells[x, y].isFree = ordered;
                if(ordered)
                {
                    cells[x, y].image.color = Color.green;
                }
                else
                {
                    cells[x, y].image.color = Color.red;
                }
            }
        }
       
    }
    public Vector2Int GetSize(itemSize size)
    {
        Vector2Int newSize = Vector2Int.zero;
        switch (size)
        {
            case itemSize.Smal:
                 newSize = Vector2Int.one;
                break;
            case itemSize.MediumHorisontal:
                 newSize = new Vector2Int(1, 2);
                break;
            case itemSize.MediumVertical:
                 newSize = new Vector2Int(2, 1);
                break;
            case itemSize.MediumSquare:
                 newSize = new Vector2Int(2, 2);
                break;
            case itemSize.Large:
                 newSize = new Vector2Int(2, 3);
                break;
        }
        return newSize = Vector2Int.zero;
    }
}
