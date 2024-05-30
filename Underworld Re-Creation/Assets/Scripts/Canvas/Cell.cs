using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Drawing;
using static UnityEditor.Progress;
public class Cell : MonoBehaviour,IDropHandler,IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text CellIndex;  // текст отображающий координаты клетки
    public TMP_Text CellText;   // текст отображающий допустим количество вещей данного item
    public int x, y; // координаты клетки
    public bool isFree; // свободна ли клетка
    public inventory inventory;  // обьект inventory для работы с ним
    public Image image; // инструмент для картинки клетки
    public GraphicRaycaster raycaster;
    private void Start()
    {
        image = GetComponent<Image>(); // присваеваем компонент для управления цветом
    }
    //используется для размещения обьекта в новую клетку, либо для возврата на предыдущую позицию
    public void OnDrop(PointerEventData eventData)
    {
        var dragItem = eventData.pointerDrag.GetComponent<item>();
       
        if (dragItem == null)
        {
            return;
        }

        // Проверяем, находится ли предмет над ячейкой инвентаря
        List<RaycastResult> hitResults = new List<RaycastResult>();
        raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>(); ;
        raycaster.Raycast(eventData, hitResults);

        bool isOverInventoryCell = false;
        foreach (RaycastResult result in hitResults)
        {
            if (result.gameObject.CompareTag("CellInventory"))
            {
                isOverInventoryCell = true;
                break;
            }
        }

        if (inventory.CheckCellFree(this,dragItem.Size) && isOverInventoryCell) 
        {
            dragItem.SetPosition(dragItem, this);  
            dragItem.PrevCell = this;
            
        }
        else
        {
          
            dragItem.SetPosition(dragItem,dragItem.PrevCell); 
        }
    }
    //для раскраски клетки при перемещения и захода мышки в данный момент на клетку
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventory.draggenItem)
        {
            if (isFree) 
            {
                
                inventory.CellsColorize(this,inventory.draggenItem.Size, UnityEngine.Color.green);
              
            }
        }
    }
    //для раскраски клетки при перемещения и выхода мышки в данный момент с клетки
    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventory.draggenItem)
        {
            if (isFree)
            {
                
                inventory.CellsColorize(this, inventory.draggenItem.Size, UnityEngine.Color.red);
               
            }
        }
    }
}