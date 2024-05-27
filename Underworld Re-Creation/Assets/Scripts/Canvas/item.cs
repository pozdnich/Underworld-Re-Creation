using sc.terrain.vegetationspawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public inventory inventory;
    public Canvas canvas;
    public Cell PrevCell;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    
    Vector2 positionItem;
    public itemSize Size;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        
        inventory.draggenItem = this;
        inventory.UpdateCellsColor();
    }

    public void OnDrag(PointerEventData eventData)
    {
        positionItem = Input.mousePosition;
        positionItem.x -= Screen.width / 2;
        positionItem.y -= Screen.height / 2;
        rectTransform.anchoredPosition = positionItem;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventory.draggenItem = null;
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
        var dragItem = eventData.pointerDrag.GetComponent<item>();
        SetPosition(dragItem, dragItem.PrevCell);
    }
    public void SetPosition(item _item,Cell cell)
    {
        _item.transform.SetParent(_item.PrevCell.transform);
        _item.transform.localPosition = Vector3.zero;
        var itemSize = _item.GetSize();
        var newPos = _item.transform.localPosition;
        if (itemSize.x > 1)
        {
            newPos.x += itemSize.x * 12.5f;
        }
        if (itemSize.y > 1)
        {
            newPos.y -= itemSize.y * 12.5f;
        }
        _item.transform.localPosition = newPos;
        _item.transform.SetParent(inventory.transform);
    }
}
