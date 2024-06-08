using sc.terrain.vegetationspawner;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class itemInCanvas : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler/*, IDropHandler*/
{


    public Item item;
    public inventory inventory; // ������ inventory ��� ������ � ���
    public Canvas canvas; // canvas � ������� �������� ������ ���������
    public Cell PrevCell; // ������ � ������� ��������� �������
    RectTransform rectTransform; // ������ �������������� ?
    public CanvasGroup canvasGroup; // ��� ���������� � canvasGroup ?

    Vector2 positionItem; // ���������� item
    public itemSize Size; // ���������� ��� ����������� ������� ��������� (���������� ���������� �����)

    public Transform nowTransform;
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
            inventory = GetComponentInParent<inventory>();
            Debug.LogError($"inventory �� �������� � ���������� � �� ������ � ������������ ��������.{name}" );
        }
    }

    //��� ���������� ��� ������ ��������� ������ ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
        
        inventory.draggenItem = this;
        inventory.UpdateCellsColor();
    }
    //��� ���������� ��� ������� ������ ����
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
    //��� ���������� ��� ���������� ������ ����
    public void OnEndDrag(PointerEventData eventData)
    {
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventory.draggenItem = null;



        // ��������, ��������� �� ������� ��� ������� ���������
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

        // ���� ������� �� ������� � ������ ��������� ��� ������ ������, ���������� ��� � ���������� ������
        if (!isOverInventoryCell || (targetCell != null && !inventory.CheckCellFree(targetCell, Size)))
        {
            SetPosition(this, PrevCell);
           
        }
        else if (targetCell != null)
        {
            Cell cell = PrevCell;
            SetPosition(this, targetCell);
            PrevCell = targetCell;
            if (!cell.isFree)
            {
               
                inventory.CellsOcupation(cell, this.Size, true);
                inventory.UpdateCellsColor();
                
            }
        }
    }
    //������� ������ item
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

    // ����� ��� ����������� ��� �������� � �������
    public void OnDrop(PointerEventData eventData)
    {
        //var dragItem = eventData.pointerDrag.GetComponent<item>();
        //Debug.Log("��������");
        //SetPosition(dragItem, dragItem.PrevCell);
    }
    //������ �������
    public void SetPosition(itemInCanvas _item,Cell cell)
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
        
        _item.transform.SetParent(canvas.GetComponent<inventory>().transformItems);
       
        inventory.CellsOcupation(cell, _item.Size, false);
        inventory.UpdateCellsColor();
        
    }
    //������ ������� ��� �������� ���������� ��������(� ��� ������� � ��� ����� ��������� � ������� SetPosition ��� ������������ ����)
    public void SetInitialPosition(itemInCanvas _item, Cell cell)
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
           

            _item.transform.SetParent(canvas.GetComponent<inventory>().transformItems);
           
            
        }
        else
        {
            Debug.LogError("inventory �� ������ ��� SetInitialPosition");
        }
    }


}