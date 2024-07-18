using UnityEngine;

/* The base item class. All items should derive from this.  ������� ����� ���������. ��� �������� ������ �������� �� �����.*/

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	new public string name = "New Item";    // �������� ��������
    public Sprite icon = null;              // ������ ��������
    public bool showInInventory = true;
    public GameObject AllThisItem;  // ����� ����� ����� ����� �� �� �����, ��������� ��� ���������� ��� ����������
    


   
    public virtual void Use ()
	{
        // ������������ �������
       
    }

    

}
