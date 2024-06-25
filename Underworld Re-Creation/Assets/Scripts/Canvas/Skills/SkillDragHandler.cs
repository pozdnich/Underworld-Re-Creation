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
        iconImage.color = new Color(1f, 1f, 1f, 1f); // ������������� ������ ������� � ��������������
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������� �������� ������ ��� ��������������
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        Image image = dragIcon.AddComponent<Image>();
        image.sprite = iconImage.sprite;
        image.color = iconImage.color; // �������� ��������� �����
        image.raycastTarget = false;

        dragRectTransform = dragIcon.GetComponent<RectTransform>();
        dragRectTransform.sizeDelta = rectTransform.sizeDelta;

        // ������������� ��������� ������� ��������� ������ ������������ ����
        dragRectTransform.position = eventData.position;
        dragRectTransform.pivot = new Vector2(0.5f, 0.5f);

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
        canvasGroup.blocksRaycasts = true;

        List<RaycastResult> hitResults = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerEventData, hitResults);

        SkillSlot slot = null;
        bool isOverSkillCell = false;

        // �������� �� ��������� � ������ � �������
        foreach (RaycastResult result in hitResults)
        {
            if (result.gameObject.CompareTag("CellSkill"))
            {
                slot = result.gameObject.GetComponent<SkillSlot>();
                isOverSkillCell = true;
                break;
            }
        }

        if (!isOverSkillCell)
        {
            // ������� ������ �� �������� �������, ���� �� ��� �������
            rectTransform.anchoredPosition = startPosition;
        }
        else
        {
            if (slot != null)
            {
                // �������� ������� ����� � ��� �� �������, ���� �� ����������
                foreach (SkillSlot one in playerController.instance.skillSlots)
                {
                    if (one.skill != null && skill != null && one.skill.name == skill.name)
                    {
                       
                        one.SetSkill(skill, false);
                        break;
                    }
                }
                // ��������� ������ ������ � ����
                slot.SetSkill(skill,true);
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
