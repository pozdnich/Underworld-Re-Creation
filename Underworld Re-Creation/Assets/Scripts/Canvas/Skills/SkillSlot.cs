using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image iconImage; // UI Image компонент для отображения иконки умения
    public Skill skill; // параметры скила
    private Color colorG; // требуется для сохранения изначального цвета скила

    private void Start()
    {
        colorG = iconImage.color;
    }

    // метод для изменения скила в слоте
    public void SetSkill(Skill newSkill,bool Add)
    {
        skill = newSkill;
        if (Add)
        {
            iconImage.sprite = skill.iconSprite;
            
            iconImage.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            Debug.Log("Стёрта абилка из ячейки");
            skill = null;
            iconImage.sprite = null;
            
            iconImage.color = colorG;
        }
    }
    // метод для вызова параметров скила
    public Skill GetSkill()
    {
        return skill;
    }
}
