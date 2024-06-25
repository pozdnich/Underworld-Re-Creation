using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Skill skill; // Умение, которое перетаскивается
    private Image iconImage;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;

    // Дубликат иконки для перетаскивания
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

        // Устанавливаем иконку умения
        iconImage.sprite = skill.iconSprite;
        iconImage.color = new Color(1f, 1f, 1f, 1f); // Устанавливаем полную яркость и непрозрачность
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Создаем дубликат иконки для перетаскивания
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        Image image = dragIcon.AddComponent<Image>();
        image.sprite = iconImage.sprite;
        image.color = iconImage.color; // Копируем настройки цвета
        image.raycastTarget = false;

        dragRectTransform = dragIcon.GetComponent<RectTransform>();
        dragRectTransform.sizeDelta = rectTransform.sizeDelta;

        // Устанавливаем начальную позицию дубликата иконки относительно мыши
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

        // Проверка на попадание в ячейку с навыком
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
            // Вернуть иконку на исходную позицию, если не над ячейкой
            rectTransform.anchoredPosition = startPosition;
        }
        else
        {
            if (slot != null)
            {
                // Удаление старого слота с тем же навыком, если он существует
                foreach (SkillSlot one in playerController.instance.skillSlots)
                {
                    if (one.skill != null && skill != null && one.skill.name == skill.name)
                    {
                       
                        one.SetSkill(skill, false);
                        break;
                    }
                }
                // Установка нового навыка в слот
                slot.SetSkill(skill,true);
            }
        }

        // Удаляем дубликат иконки
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }

        // Вернуть иконку на исходную позицию
        rectTransform.anchoredPosition = startPosition;
    }
}
