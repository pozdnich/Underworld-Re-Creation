using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemInCanvasCon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public Itemtype itemtype; //тип предмета (Экиперовка, Расходник или же Ресурс)

    public Consumable item; // свойства предмета, но надо подумать как их менять в зависимости от типа предмета
    private Consumable originalItem; // Оригинальное состояние предмета, если это экиперовк

    public Inventory inventory; // объект inventory для работы с ним
    public Canvas canvas; // canvas с которым работает именно инвентарь
    public Cell PrevCell; // клетка в которой находится предмет в инвентаре
    
    RectTransform rectTransform; // Прямое преобразование
    public CanvasGroup canvasGroup; // для управления в canvasGroup


    Vector2 positionItem; // координаты item
    public ItemSize Size; // енуминатор для определения размера предметов (количества занимаемых клеток)

    private bool isDragging = false; // Флаг для отслеживания перетаскивания

    private float lastClickTime;
    private const float doubleClickThreshold = 1f; // Интервал для двойного нажатия

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

        }

        originalItem = Instantiate(item);
    }



    // что происходит при первом опускании кнопки мыши
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        inventory.draggedItemCon = this;
        inventory.UpdateCellsColor();

    }
    // Если при перетаскивании предмета игрок закроет инвентарь или профиль в зависимости от того где лежал предмет
    private void OnDisable()
    {
        if (isDragging)
        {
            // Проверка и выполнение действия для PrevCell
            if (PrevCell != null)
            {
                inventory.draggedItemCon = null;
                ThrowItemAway();
                inventory.UpdateCellsColor();
            }

           
        }

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
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventory.draggedItemCon = null;

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
            if (result.gameObject.CompareTag("AreaForThrowingItems"))
            {
                PermissionToThrowAway = true;
                break;
            }
        }

        // Проверка, что предмет находится над ячейкой и ячейка свободна
        bool isTargetCellFree = (targetCell != null && inventory.CheckCellFree(targetCell, Size));
       

        if (!isOverInventoryCell || !(isTargetCellFree))
        {
            Debug.Log("Не соблюдено условие, возврат на прежнее место.");
            // Возвращаем предмет в предыдущую ячейку
            if (PrevCell != null && !PermissionToThrowAway)
            {
                SetPosition(this, PrevCell);

            }
            else
            {

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

                }
               
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
    public void SetPosition(itemInCanvasCon item, Cell cell)
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
        if (!inventory.ItemsCon.Contains(item))
        {
            inventory.ItemsCon.Add(item);
        }

        inventory.CellsOccupation(cell, item.Size, false);
        inventory.UpdateCellsColor();
    }

    // Меняем позицию при загрузке сохранения
    public void SetInitialPosition(itemInCanvasCon item, Cell cell)
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
   

    // Выбрасывание Предмета на землю
    public void ThrowItemAway()
    {
        // Получаем текущую позицию dropPoint
        Vector3 position = playerController.instance.dropPoint.transform.position;

        // Создаем лут-объект в мире на основе оригинального состояния предмета
        GameObject loot = Instantiate(item.AllThisItem, position, Quaternion.identity);

        // Восстанавливаем исходное состояние предмета
        item = originalItem;
        if (PrevCell != null)
        {
            inventory.ItemsCon.Remove(this);
            Destroy(gameObject);
            inventory.UpdateCellsColor();


        }
       

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            item.Use();
            PrevCell.isFree = true;
            inventory.ItemsCon.Remove(this);
            Destroy(gameObject);
            inventory.UpdateCellsColor();
        }
        else
        {
            lastClickTime = Time.time;
        }
    }
}
