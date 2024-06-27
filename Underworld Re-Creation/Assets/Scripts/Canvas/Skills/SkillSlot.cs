using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image iconImage; // UI Image ��������� ��� ����������� ������ ������
    public Skill skill; // ��������� �����
    private Color colorG; // ��������� ��� ���������� ������������ ����� �����

    private void Start()
    {
        colorG = iconImage.color;
    }

    // ����� ��� ��������� ����� � �����
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
            Debug.Log("����� ������ �� ������");
            skill = null;
            iconImage.sprite = null;
            
            iconImage.color = colorG;
        }
    }
    // ����� ��� ������ ���������� �����
    public Skill GetSkill()
    {
        return skill;
    }
}
