using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemInCanvasCon : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public Itemtype itemtype; //��� �������� (����������, ��������� ��� �� ������)

    public Consumable item; // �������� ��������, �� ���� �������� ��� �� ������ � ����������� �� ���� ��������
    private Consumable originalItem; // ������������ ��������� ��������, ���� ��� ���������

    public Inventory inventory; // ������ inventory ��� ������ � ���
    public Canvas canvas; // canvas � ������� �������� ������ ���������
    public Cell PrevCell; // ������ � ������� ��������� ������� � ���������
    
    RectTransform rectTransform; // ������ ��������������
    public CanvasGroup canvasGroup; // ��� ���������� � canvasGroup


    Vector2 positionItem; // ���������� item
    public ItemSize Size; // ���������� ��� ����������� ������� ��������� (���������� ���������� ������)

    private bool isDragging = false; // ���� ��� ������������ ��������������

    private float lastClickTime;
    private const float doubleClickThreshold = 1f; // �������� ��� �������� �������

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



    // ��� ���������� ��� ������ ��������� ������ ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        inventory.draggedItemCon = this;
        inventory.UpdateCellsColor();

    }
    // ���� ��� �������������� �������� ����� ������� ��������� ��� ������� � ����������� �� ���� ��� ����� �������
    private void OnDisable()
    {
        if (isDragging)
        {
            // �������� � ���������� �������� ��� PrevCell
            if (PrevCell != null)
            {
                inventory.draggedItemCon = null;
                ThrowItemAway();
                inventory.UpdateCellsColor();
            }

           
        }

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
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventory.draggedItemCon = null;

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
            if (result.gameObject.CompareTag("AreaForThrowingItems"))
            {
                PermissionToThrowAway = true;
                break;
            }
        }

        // ��������, ��� ������� ��������� ��� ������� � ������ ��������
        bool isTargetCellFree = (targetCell != null && inventory.CheckCellFree(targetCell, Size));
       

        if (!isOverInventoryCell || !(isTargetCellFree))
        {
            Debug.Log("�� ��������� �������, ������� �� ������� �����.");
            // ���������� ������� � ���������� ������
            if (PrevCell != null && !PermissionToThrowAway)
            {
                SetPosition(this, PrevCell);

            }
            else
            {

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

    // ������ ������� ��� �������� ����������
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
            Debug.LogError("Inventory �� ������ ��� SetInitialPosition");
        }
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
