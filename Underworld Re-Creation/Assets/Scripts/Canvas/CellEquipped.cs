using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CellEquipped : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ваш код для обработки события входа мыши в ячейку
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Ваш код для обработки события выхода мыши из ячейки
    }
}
