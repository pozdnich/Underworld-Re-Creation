using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image iconImage; // UI Image компонент для отображения иконки умения
    private Skill skill;

    public void SetSkill(Skill newSkill)
    {
        skill = newSkill;
        if (skill != null)
        {
            iconImage.sprite = skill.iconSprite;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }

    public Skill GetSkill()
    {
        return skill;
    }
}
