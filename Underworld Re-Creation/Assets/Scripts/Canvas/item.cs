using sc.terrain.vegetationspawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler/*, IDropHandler*/
{



    public inventory inventory; // обьект inventory для работы с ним
    public Canvas canvas; // canvas с которым работает именно инвентарь
    public Cell PrevCell; // клетка в которой находится предмет
    RectTransform rectTransform; // Прямое преобразование ?
    CanvasGroup canvasGroup; // для управления в canvasGroup ?

    Vector2 positionItem; // координаты item
    public itemSize Size; // енуминатор для определения размера предметов (количества занимаемых леток)

    public Transform nowTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        if (inventory == null)
        {
            inventory = GetComponentInParent<inventory>();
            Debug.LogError($"inventory не назначен в инспекторе и не найден в родительских объектах.{name}" );
        }
    }

    //что происходит при первом опускании кнопки мыши
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        
        inventory.draggenItem = this;
        inventory.UpdateCellsColor();
    }
    //что происходит при зажатии кнопки мыши
    public void OnDrag(PointerEventData eventData)
    {
        //positionItem = Input.mousePosition;
        //positionItem.x -= Screen.width / 2;
        //positionItem.y -= Screen.height / 2;
        
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if (PrevCell)
        {
           
            inventory.CellsOcupation(PrevCell, Size, true);
        }
       
    }
    //что происходит при отпускании кнопки мыши
    public void OnEndDrag(PointerEventData eventData)
    {
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventory.draggenItem = null;



        // Проверка, находится ли предмет над ячейкой инвентаря
        List<RaycastResult> hitResults = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerEventData, hitResults);

        Cell targetCell = null;
        bool isOverInventoryCell = false;

        foreach (RaycastResult result in hitResults)
        {
            if (result.gameObject.CompareTag("CellInventory"))
            {
                targetCell = result.gameObject.GetComponent<Cell>();
                isOverInventoryCell = true;
                break;
            }
        }

        // Если предмет не помещен в ячейку инвентаря или ячейка занята, возвращаем его в предыдущую ячейку
        if (!isOverInventoryCell || (targetCell != null && !inventory.CheckCellFree(targetCell, Size)))
        {
            SetPosition(this, PrevCell);
        }
        else if (targetCell != null)
        {
            SetPosition(this, targetCell);
            PrevCell = targetCell;
        }
    }

    public Vector2Int GetSize()
    {
       
        Vector2Int size;
        switch (Size)
        {
            case itemSize.Smal:
                return size = Vector2Int.one;
            case itemSize.MediumHorisontal:
                return size = new Vector2Int(1,2);
            case itemSize.MediumVertical:
                return size = new Vector2Int(2, 1);
            case itemSize.MediumSquare:
                return size = new Vector2Int(2, 2);
            case itemSize.Large:
                return size = new Vector2Int(2, 3);
        }
        return size = Vector2Int.zero;
    }


    public void OnDrop(PointerEventData eventData)
    {
        //var dragItem = eventData.pointerDrag.GetComponent<item>();
        //Debug.Log("Действие");
        //SetPosition(dragItem, dragItem.PrevCell);
    }
    public void SetPosition(item _item,Cell cell)
    {
        Debug.Log($"_item.transform.localPosition x={_item.transform.localPosition.x} y={_item.transform.localPosition.y} ");
        _item.transform.SetParent(cell.transform);
        _item.transform.localPosition = Vector3.zero;
        var itemSize = _item.GetSize();
        var newPos = _item.transform.localPosition;
        if (itemSize.x > 1)
        {
            newPos.x += itemSize.x * 10f;
        }
        if (itemSize.y > 1)
        {
            newPos.y -= itemSize.y * 10f;
        }
        Debug.Log($"_item.transform.localPosition x={_item.transform.localPosition.x} y={_item.transform.localPosition.y} ");
        _item.transform.localPosition = newPos;
        Debug.Log($"_item.transform.localPosition x={_item.transform.localPosition.x} y={_item.transform.localPosition.y} ,newPos  x={newPos.x} y={newPos.y}");
        _item.transform.SetParent(canvas.GetComponent<inventory>().transformItems);
        Debug.Log($"_item.transform.localPosition x={_item.transform.localPosition.x} y={_item.transform.localPosition.y} ,newPos  x={newPos.x} y={newPos.y}");
        inventory.CellsOcupation(cell, _item.Size, false);
        inventory.UpdateCellsColor();
    }

    public void SetInitialPosition(item _item, Cell cell)
    {
        if (inventory == null)
        {
            inventory = GetComponentInParent<inventory>();
        }
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        if (inventory != null)
        {
            _item.transform.SetParent(cell.transform); 
            _item.transform.localPosition = Vector3.zero;
            var itemSize = _item.GetSize();
            var newPos = _item.transform.localPosition;
            if (itemSize.x > 1)
            {
                newPos.x += itemSize.x * 10f;
            }
            if (itemSize.y > 1)
            {
                newPos.y -= itemSize.y * 10f;
            }
            _item.transform.localPosition = newPos;
            Debug.Log($"_item.transform.localPosition x={_item.transform.localPosition.x} y={_item.transform.localPosition.y} ,newPos  x={newPos.x} y={newPos.y}");
           
            _item.transform.SetParent(canvas.GetComponent<inventory>().transformItems,false);
            Debug.Log($"_item.transform.localPosition x={_item.transform.localPosition.x} y={_item.transform.localPosition.y} ,newPos  x={newPos.x} y={newPos.y}");

        }
        else
        {
            Debug.LogError("inventory не найден для SetInitialPosition");
        }
    }


}
