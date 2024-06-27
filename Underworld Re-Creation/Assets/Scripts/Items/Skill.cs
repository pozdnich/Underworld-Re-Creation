using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill Tree/Skill")]
public class Skill : ScriptableObject
{
    public string name; // название умения
    public Sprite iconSprite; // иконка умения
    public int MinLevel; // минимальный уровень для применения
    public SpecificEquipment[] NecessaryEquipment; // перечесление какие тыпы экиперовки требуются для использования
    public float SkillNumber; // номер скила, требуется для выбора анимации


}
