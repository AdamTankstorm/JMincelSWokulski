using Unity.VisualScripting;
using UnityEngine;

public class Item:MonoBehaviour
{
    public string itemName;
    public string description;
    public int itemID;
    public Sprite icon;
    public bool isSpecial =false;
    public float baseValue;
    public GameObject Wardrobe;
    public GameObject Place;
    public int inventorySlot;
    public int count = 1;
    // Constructor
    public Item(string name, string desc, int id, Sprite ico, bool isSpecial, float baseValue, int count, GameObject wardrobe, GameObject place)
    {
        this.itemName = name;
        this.description = desc;
        this.itemID = id;
        this.icon = ico;
        this.isSpecial = isSpecial;
        Wardrobe = wardrobe;
        Place = place;
        this.count= 1;
    }
    
    public void AddItem(int numberOfItems)
    {
        this.count+=numberOfItems;
    }
    public void AddItem()
    {
        this.count++;
    }
    public void RemoveItem(int numberOfItems)
    {
        this.count -= numberOfItems;
        if(this.count == 0)
        {
            
        }
    }
    public void RemoveItem()
    {

    }

}
