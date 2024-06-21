using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Skill skill; // ������, ������� ���������������
    private Image iconImage;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;

    // �������� ������ ��� ��������������
    private GameObject dragIcon;
    private RectTransform dragRectTransform;

    private void Start()
    {
        iconImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPosition = rectTransform.anchoredPosition;

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        // ������������� ������ ������
        iconImage.sprite = skill.iconSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������� �������� ������ ��� ��������������
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        Image image = dragIcon.AddComponent<Image>();
        image.sprite = iconImage.sprite;
        image.raycastTarget = false;

        dragRectTransform = dragIcon.GetComponent<RectTransform>();
        dragRectTransform.sizeDelta = rectTransform.sizeDelta;

        // ������������� ��������� ������� ��������� ������ ������������ ����
        dragRectTransform.position = eventData.position;
        dragRectTransform.pivot = new Vector2(0.5f, 0.5f);

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            dragRectTransform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        List<RaycastResult> hitResults = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerEventData, hitResults);

        SkillSlot slot = null;
        bool isOverInventoryCell = false;

        foreach (RaycastResult result in hitResults)
        {
            if (result.gameObject.CompareTag("CellSkill"))
            {
                slot = result.gameObject.GetComponent<SkillSlot>();
                isOverInventoryCell = true;
                break;
            }
        }

        if (!isOverInventoryCell)
        {
            // ������� ������ �� �������� �������
            rectTransform.anchoredPosition = startPosition;
        }
        else
        {
            if (slot != null)
            {
                slot.SetSkill(skill);
            }
        }

        // ������� �������� ������
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }

        // ������� ������ �� �������� �������
        rectTransform.anchoredPosition = startPosition;
    }
}
