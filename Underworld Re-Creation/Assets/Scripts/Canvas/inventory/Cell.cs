using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Drawing;
using static UnityEditor.Progress;
public class Cell : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text CellIndex;  // текст отображающий координаты клетки
    public TMP_Text CellText;   // текст отображающий допустим количество вещей данного item
    public int x, y; // координаты клетки
    public bool isFree; // свободна ли клетка
    public Inventory inventory;  // обьект inventory для работы с ним
    public Image image; // инструмент для картинки клетки
    public GraphicRaycaster raycaster;
    private void Start()
    {
       
    }
    
    //для раскраски клетки при перемещения и захода мышки в данный момент на клетку
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventory.draggedItem)
        {
            if (isFree) 
            {
                
                inventory.CellsColorize(this,inventory.draggedItem.Size, UnityEngine.Color.green);
              
            }
        }
    }
    //для раскраски клетки при перемещения и выхода мышки в данный момент с клетки
    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventory.draggedItem)
        {
            if (isFree)
            {
                
                inventory.CellsColorize(this, inventory.draggedItem.Size, UnityEngine.Color.grey);
               
            }
        }
    }
}