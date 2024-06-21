using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill Tree/Skill")]
public class Skill : ScriptableObject
{
    public string name;
    public Sprite iconSprite;
    public int MinLevel;
    public SpecificEquipment[] NecessaryEquipment;

}
