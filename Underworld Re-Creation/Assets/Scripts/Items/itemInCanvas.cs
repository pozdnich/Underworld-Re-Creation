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
    private Equipment originalItem; // ������������ ��������� ��������

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
        
        originalItem = Instantiate(item);
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

        // ��������, ��������� �� ������� ��� ������� ��������� ��� �������
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

        // ��������, ��� ������� ��������� ��� ������� � ������ ��������
        bool isTargetCellFree = (targetCell != null && inventory.CheckCellFree(targetCell, Size));
        bool isTargetCellEquippedFree = (targetCellEquipped != null && targetCellEquipped.isFree && targetCellEquipped.TypeOfEquipment == item.equipSlot) ;

        if (!isOverInventoryCell || !(isTargetCellFree || isTargetCellEquippedFree))
        {
            Debug.Log("�� ��������� �������, ������� �� ������� �����.");
            // ���������� ������� � ���������� ������
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
                Debug.Log("������� ��� �������������");
                // ���� ������� �� ��� ������� � �� ��� ����� � ��������� ��� �������, ��������� ���
               ThrowItemAway();
            }
        }
        else
        {
            if (targetCell != null)
            {
                if (PrevCell != null)
                {
                    Debug.Log("����������� � ���������.");
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
                    Debug.Log("����������� � ��������� �� �������.");
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
                Debug.Log("����������� �� ��������� � �������.");
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
        if (!inventory.Items.Contains(this))
        {
            inventory.Items.Add(item);
        }
      
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
    // �������� �������� �� ��������� � ������� ������
    public void SetPositionInProfile(ItemInCanvas item, CellEquipped cell)
    {
        item.transform.SetParent(cell.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.SetParent(cell.GetComponentInParent<Transform>().transform);
        inventory.ProfileSlot[(int)item.item.equipSlot] = item;
        inventory.Items.Remove(item);
        inventory.UpdateCellsColor();
    }

    // ������������ �������� �� �����
    public void ThrowItemAway()
    {
        // �������� ������� ������� dropPoint
        Vector3 position = playerController.instance.dropPoint.transform.position;

        // ������� ���-������ � ���� �� ������ ������������� ��������� ��������
        GameObject loot = Instantiate(item.AllThisItem, position, Quaternion.identity);

        // ��������������� �������� ��������� ��������
        item = originalItem;

        // ������� ������� ������ �� ���������
        Destroy(gameObject);
        inventory.UpdateCellsColor();
    }
}
public enum Itemtype { Equipment, Consumable, Resource }