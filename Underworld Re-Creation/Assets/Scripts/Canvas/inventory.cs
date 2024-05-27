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
}
