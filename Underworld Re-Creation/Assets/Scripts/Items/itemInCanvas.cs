using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInCanvas : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Itemtype itemtype;
    public Equipment item;
    private Equipment originalItem; // Оригинальное состояние предмета

    public Inventory inventory; // объект inventory для работы с ним
    public Canvas canvas; // canvas с которым работает именно инвентарь
    public Cell PrevCell; // клетка в которой находится предмет в инвентаре
    public CellEquipped PrevCellEquipped; // клетка в которой находится предмет в профиле
    RectTransform rectTransform; // Прямое преобразование
    public CanvasGroup canvasGroup; // для управления в canvasGroup
    

    Vector2 positionItem; // координаты item
    public ItemSize Size; // енуминатор для определения размера предметов (количества занимаемых клеток)

    void Start()
    {
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        GetComponent<Image>().sprite = item.icon;

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        if (inventory == null)
        {
            inventory = GetComponentInParent<Inventory>();
            Debug.LogError($"Inventory не назначен в инспекторе и не найден в родительских объектах. {name}");
        }
        
        originalItem = Instantiate(item);
    }

    // что происходит при первом опускании кнопки мыши
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        inventory.draggedItem = this;
        inventory.UpdateCellsColor();
    }

    // что происходит при зажатии кнопки мыши
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if (PrevCell)
        {
            inventory.CellsOccupation(PrevCell, Size, true);
        }
    }

    // что происходит при отпускании кнопки мыши
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventory.draggedItem = null;

        // Проверка, находится ли предмет над ячейкой инвентаря или профиля
        List<RaycastResult> hitResults = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerEventData, hitResults);

        Cell targetCell = null;
        CellEquipped targetCellEquipped = null;
        bool isOverInventoryCell = false;
        bool PermissionToThrowAway = false;

        foreach (RaycastResult result in hitResults)
        {
            Debug.Log($"{result.gameObject.tag.ToString()} {hitResults.Count}");
            if (result.gameObject.CompareTag("CellInventory"))
            {
                targetCell = result.gameObject.GetComponent<Cell>();
                isOverInventoryCell = true;
                break;
            }
            if (result.gameObject.CompareTag("CellEquipped"))
            {
                targetCellEquipped = result.gameObject.GetComponent<CellEquipped>();
                isOverInventoryCell = true;
                break;
            }
            if (result.gameObject.CompareTag("AreaForThrowingItems"))
            {
                PermissionToThrowAway = true;
                break;
            }
        }

        // Проверка, что предмет находится над ячейкой и ячейка свободна
        bool isTargetCellFree = (targetCell != null && inventory.CheckCellFree(targetCell, Size));
        bool isTargetCellEquippedFree = (targetCellEquipped != null && targetCellEquipped.isFree && targetCellEquipped.TypeOfEquipment == item.equipSlot) ;

        if (!isOverInventoryCell || !(isTargetCellFree || isTargetCellEquippedFree))
        {
            Debug.Log("Не соблюдено условие, возврат на прежнее место.");
            // Возвращаем предмет в предыдущую ячейку
            if (PrevCell != null && !PermissionToThrowAway)
            {
                SetPosition(this, PrevCell);
                Debug.Log("PrevCell != null");
            }
            else if (PrevCellEquipped != null && !PermissionToThrowAway)
            {
                SetPositionInProfile(this, PrevCellEquipped);
                Debug.Log("PrevCellEquipped != null");
            }
            else 
            {
                Debug.Log("Предмет Над выбрасыванием");
                // Если предмет не над ячейкой и не был ранее в инвентаре или профиле, выбросить его
               ThrowItemAway();
            }
        }
        else
        {
            if (targetCell != null)
            {
                if (PrevCell != null)
                {
                    Debug.Log("Перемещение в инвентаре.");
                    Cell cell = PrevCell;
                    SetPosition(this, targetCell);
                    PrevCell = targetCell;

                    //if (!cell.isFree)
                    //{
                    //    inventory.CellsOccupation(cell, this.Size, true);
                    //    inventory.UpdateCellsColor();
                    //}
                }
                else
                {
                    Debug.Log("Перемещение в инвентарь из профиля.");
                    CellEquipped cell = PrevCellEquipped;
                    SetPosition(this, targetCell);
                    PrevCell = targetCell;

                    if (!cell.isFree)
                    {
                        cell.isFree = true;
                        inventory.UpdateCellsColor();
                    }

                    PrevCellEquipped = null;
                }
            }
            else if (targetCellEquipped != null)
            {
                Debug.Log("Перемещение из инвенторя в Профиль.");
                Cell cell = PrevCell;
                SetPositionInProfile(this, targetCellEquipped);
                PrevCellEquipped = targetCellEquipped;
                if (cell != null && !cell.isFree)
                {
                    inventory.CellsOccupation(cell, this.Size, true);
                    inventory.UpdateCellsColor();
                }
                PrevCell = null;
            }
        }
    }


    // Вернуть размер item
    public Vector2Int GetSize()
    {
        switch (Size)
        {
            case ItemSize.Small:
                return Vector2Int.one;
            case ItemSize.MediumVertical:
                return new Vector2Int(1, 2);
            case ItemSize.MediumHorizontal:
                return new Vector2Int(2, 1);
            case ItemSize.MediumSquare:
                return new Vector2Int(2, 2);
            case ItemSize.Large:
                return new Vector2Int(2, 3);
        }
        return Vector2Int.zero;
    }

    // Меняем Позицию
    public void SetPosition(ItemInCanvas item, Cell cell)
    {
        item.transform.SetParent(cell.transform);
        item.transform.localPosition = Vector3.zero;
        var itemSize = item.GetSize();
        var newPos = item.transform.localPosition;
        if (itemSize.x > 1)
        {
            newPos.x += itemSize.x * 10f;
        }
        if (itemSize.y > 1)
        {
            newPos.y -= itemSize.y * 10f;
        }

        item.transform.localPosition = newPos;
        item.transform.SetParent(canvas.GetComponent<Inventory>().transformItems);
        if (!inventory.Items.Contains(this))
        {
            inventory.Items.Add(item);
        }
      
        inventory.ProfileSlot[(int)item.item.equipSlot] = null;
        inventory.CellsOccupation(cell, item.Size, false);
        inventory.UpdateCellsColor();
    }

    // Меняем позицию при загрузке сохранения
    public void SetInitialPosition(ItemInCanvas item, Cell cell)
    {
        if (inventory == null)
        {
            inventory = GetComponentInParent<Inventory>();
        }
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        if (inventory != null)
        {
            item.transform.SetParent(cell.transform);
            item.transform.localPosition = Vector3.zero;
            var itemSize = item.GetSize();
            var newPos = item.transform.localPosition;
            if (itemSize.x > 1)
            {
                newPos.x += itemSize.x * 10f;
            }
            if (itemSize.y > 1)
            {
                newPos.y -= itemSize.y * 10f;
            }
            item.transform.localPosition = newPos;
            item.transform.SetParent(canvas.GetComponent<Inventory>().transformItems);
        }
        else
        {
            Debug.LogError("Inventory не найден для SetInitialPosition");
        }
    }
    // передача предмета из инвенторя в профиль игрока
    public void SetPositionInProfile(ItemInCanvas item, CellEquipped cell)
    {
        item.transform.SetParent(cell.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.SetParent(cell.GetComponentInParent<Transform>().transform);
        inventory.ProfileSlot[(int)item.item.equipSlot] = item;
        inventory.Items.Remove(item);
        inventory.UpdateCellsColor();
    }

    // Выбрасывание Предмета на землю
    public void ThrowItemAway()
    {
        // Получаем текущую позицию dropPoint
        Vector3 position = playerController.instance.dropPoint.transform.position;

        // Создаем лут-объект в мире на основе оригинального состояния предмета
        GameObject loot = Instantiate(item.AllThisItem, position, Quaternion.identity);

        // Восстанавливаем исходное состояние предмета
        item = originalItem;

        // Удаляем текущий объект из инвентаря
        Destroy(gameObject);
        inventory.UpdateCellsColor();
    }
}
public enum Itemtype { Equipment, Consumable, Resource }