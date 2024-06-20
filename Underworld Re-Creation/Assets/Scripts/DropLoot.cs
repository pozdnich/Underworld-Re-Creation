using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject LootStart;
    public List<GameObject> Loot;
    // Start is called before the first frame update
    void Start()
    {

        Vector3 poz = LootStart.GetComponent<Transform>().position;
        int l = UnityEngine.Random.Range(0, Loot.Count - 1);
        Loot[l].GetComponentInChildren<ItemInCanvas>().item.AllThisItem = Loot[l]; // присвоили его образ его партаметру AllThisItem чтобы можно было его спокойно выкидывать из инвенторя и обратно подымать в инвентарь
        
        Loot[l].GetComponentInChildren<ItemInCanvas>().item.AllThisItem.GetComponentInChildren<ItemInCanvas>().item.ArmorPowerEquipment = 200; // пример рамндомной настройки предмета
        
        GameObject loot = Instantiate(Loot[l].GetComponentInChildren<ItemInCanvas>().item.AllThisItem, poz, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
