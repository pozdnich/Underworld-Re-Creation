using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInCanvas : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Itemtype itemtype;
    public Equipment item;
    public Inventory inventory; // ������ inventory ��� ������ � ���
    public Canvas canvas; // canvas � ������� �������� ������ ���������
    public Cell PrevCell; // ������ � ������� ��������� ������� � ���������
    public CellEquipped PrevCellEquipped; // ������ � ������� ��������� ������� � �������
    RectTransform rectTransform; // ������ ��������������
    public CanvasGroup canvasGroup; // ��� ���������� � canvasGroup

    Vector2 positionItem; // ���������� item
    public ItemSize Size; // ���������� ��� ����������� ������� ��������� (���������� ���������� ������)

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
            Debug.LogError($"Inventory �� �������� � ���������� � �� ������ � ������������ ��������. {name}");
        }
    }

    // ��� ���������� ��� ������ ��������� ������ ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        inventory.draggedItem = this;
        inventory.UpdateCellsColor();
    }

    // ��� ���������� ��� ������� ������ ����
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if (PrevCell)
        {
            inventory.CellsOccupation(PrevCell, Size, true);
        }
    }

    // ��� ���������� ��� ���������� ������ ����
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventory.draggedItem = null;

        // ��������, ��������� �� ������� ��� ������� ��������� ��� ������� � ������ ����������
        List<RaycastResult> hitResults = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerEventData, hitResults);

        Cell targetCell = null;
        CellEquipped targetCellEquipped = null;
        bool isOverInventoryCell = false;

        foreach (RaycastResult result in hitResults)
        {
            if (result.gameObject.CompareTag("CellInventory") || result.gameObject.CompareTag("CellEquipped"))
            {
                if (result.gameObject.CompareTag("CellInventory"))
                {
                    targetCell = result.gameObject.GetComponent<Cell>();
                }
                if (result.gameObject.CompareTag("CellEquipped"))
                {
                    targetCellEquipped = result.gameObject.GetComponent<CellEquipped>();
                }

                isOverInventoryCell = true;
                break;
            }
        }

        // ���� ������� �� ������� � ������ ��������� ��� ������ ������, ���������� ��� � ���������� ������
        if (!isOverInventoryCell || (targetCell != null && !inventory.CheckCellFree(targetCell, Size)) || (targetCellEquipped != null && !targetCellEquipped.isFree))
        {
            if (PrevCell != null)
            {
                SetPosition(this, PrevCell);
            }
            else if (PrevCellEquipped != null)
            {
                SetPositionInProfile(this, PrevCellEquipped);
            }
        }
        else if (targetCell != null || targetCellEquipped != null)
        {
            if (targetCell != null)
            {
                Cell cell = PrevCell;
                SetPosition(this, targetCell);
                PrevCell = targetCell;
                PrevCellEquipped = null;
                if (!cell.isFree)
                {
                    inventory.CellsOccupation(cell, this.Size, true);
                    inventory.UpdateCellsColor();
                }
            }
            else if (targetCellEquipped != null)
            {
                CellEquipped cell = PrevCellEquipped;
                SetPositionInProfile(this, targetCellEquipped);
                PrevCellEquipped = targetCellEquipped;
                PrevCell = null;
                if (!cell.isFree)
                {
                    cell.isFree = true;
                    inventory.UpdateCellsColor();
                }
            }
        }
    }

    // ������� ������ item
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

    // ������ �������
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
        inventory.Items.Add(item);
        inventory.ProfileSlot[(int)item.item.equipSlot] = null;
        inventory.CellsOccupation(cell, item.Size, false);
        inventory.UpdateCellsColor();
    }

    // ������ ������� ��� �������� ����������
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
            Debug.LogError("Inventory �� ������ ��� SetInitialPosition");
        }
    }

    public void SetPositionInProfile(ItemInCanvas item, CellEquipped cell)
    {
        item.transform.SetParent(cell.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.SetParent(cell.GetComponentInParent<Transform>().transform);
        inventory.ProfileSlot[(int)item.item.equipSlot] = item;
        inventory.Items.Remove(item);
        inventory.UpdateCellsColor();
    }
}
public enum Itemtype { Equipment, Consumable, Resource }