using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class Cell : MonoBehaviour,IDropHandler,IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text CellIndex;
    public TMP_Text CellText;
    public int x, y;
    public bool isFree;
    public inventory inventory;
    public Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        var dragItem = eventData.pointerDrag.GetComponent<item>();
        if (inventory.CheckCellFree(this,dragItem.Size)) 
        {
            //dragItem.SetPosition(dragItem, this);  ???
            dragItem.transform.SetParent(transform);
            dragItem.transform.localPosition = Vector3.zero;
            var itemSize = dragItem.GetSize();
            var newPos = dragItem.transform.localPosition;
            if (itemSize.x > 1)
            {
                newPos.x += itemSize.x * 12.5f;
            }
            if (itemSize.y > 1)
            {
                newPos.y -= itemSize.y * 12.5f;
            }
            dragItem.transform.localPosition = newPos;

            dragItem.transform.SetParent(inventory.transform);
            inventory.UpdateCellsColor();
            isFree = false;
            dragItem.PrevCell = this;
            inventory.CellsOcupation(this, dragItem.Size, false);
        }
        else
        {
            dragItem.SetPosition(dragItem,dragItem.PrevCell); 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventory.draggenItem)
        {
            if (isFree) 
            {
                image.color = Color.green;
                var size = inventory.draggenItem.GetSize();
                if (size.y > 1)
                {
                    if (y + 1 < inventory.SizeY - 1)
                    {
                        inventory.cells[x, y + 1].image.color = Color.green;
                    }

                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventory.draggenItem)
        {
            if (isFree)
            {
                image.color = Color.red;
                var size = inventory.draggenItem.GetSize();
                if (size.y > 1)
                {
                    if (y + 1 < inventory.SizeY - 1)
                    {
                        inventory.cells[x, y + 1].image.color = Color.red;
                    }
                }
            }
        }
    }
}