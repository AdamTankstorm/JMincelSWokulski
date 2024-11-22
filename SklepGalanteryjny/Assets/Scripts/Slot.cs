using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Item item = null;
    public Inventory Inventory;
    public void AddItem(Item item)
    {
        Item itemCreated = Instantiate(item, this.transform);
        this.item = itemCreated;
        Inventory.ItemsList.Append(itemCreated);
    }

}

