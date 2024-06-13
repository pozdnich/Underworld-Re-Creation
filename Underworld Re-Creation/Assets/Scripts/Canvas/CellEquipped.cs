using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CellEquipped : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text CellText; // текст отображающий допустимое количество вещей данного item
    public EquipmentSlot TypeOfEquipment; // тип экипировки для этой ячейки
    public bool isFree; // свободна ли клетка
    public Inventory inventory; // объект inventory для работы с ним
    public Image image; // инструмент для картинки клетки
    public GraphicRaycaster raycaster;

    private void Start()
    {
        isFree = true; // Изначально ячейка свободна
    }

    // Используется для размещения объекта в новую клетку, либо для возврата на предыдущую позицию
    public void OnDrop(PointerEventData eventData)
    {
        var dragItem = eventData.pointerDrag.GetComponent<ItemInCanvas>();
        if (dragItem == null) return;

        // Проверяем, находится ли предмет над ячейкой инвентаря
        List<RaycastResult> hitResults = new List<RaycastResult>();
        raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        raycaster.Raycast(eventData, hitResults);

        bool isOverInventoryCell = false;
        foreach (RaycastResult result in hitResults)
        {
            if (result.gameObject.CompareTag("CellEquipped"))
            {
                isOverInventoryCell = true;
                break;
            }
        }

        // Проверка соответствия типа предмета типу ячейки экипировки
        if (isFree && isOverInventoryCell && TypeOfEquipment == dragItem.item.equipSlot)
        {
            dragItem.SetPositionInProfile(dragItem, this);
            dragItem.PrevCell = null;
            dragItem.PrevCellEquipped = this;
            isFree = false;
        }
        else
        {
            dragItem.SetPosition(dragItem, dragItem.PrevCell);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ваш код для обработки события входа мыши в ячейку
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Ваш код для обработки события выхода мыши из ячейки
    }
}
