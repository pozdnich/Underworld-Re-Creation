using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Базовый класс для всех характеристик: здоровья, брони, урона и т. д. */

[System.Serializable]
public class Stat {

	public int baseValue;   // Начальное значение

    // Сохранение списка всех модификаторов этой характеристики.
    public List<int> modifiers = new List<int>();

    // Сложение всех модификаторов вместе и возврат результата
    public int GetValue ()
	{
		int finalValue = baseValue;
		modifiers.ForEach(x => finalValue += x);
		return finalValue;
	}

	// Добавление модификатора в список
	public void AddModifier (int modifier)
	{
		if (modifier != 0)
			modifiers.Add(modifier);
	}

	// Удаление модификатора из списка
	public void RemoveModifier (int modifier)
	{
		if (modifier != 0)
			modifiers.Remove(modifier);
	}

}
