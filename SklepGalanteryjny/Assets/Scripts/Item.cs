using System;
using Unity.VisualScripting;
using UnityEngine;

public class Item:MonoBehaviour
{
    
    public Inventory Inventory;
    public string itemName;
    public string description;
    public int itemID;
    public bool isSpecial =false;
    public float baseValue;
    public int count = 1;
    // Constructor
    public Item(string name, string desc, int id, bool isSpecial, float baseValue, int count)
    {
        this.itemName = name;
        this.description = desc;
        this.itemID = id;
        this.isSpecial = isSpecial;

        this.count= 1;
    }
    
    public void AddItem(int numberOfItems)
    {
        this.count+=numberOfItems;
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.itemsChanged();
        

    }
    public void AddItem()
    {
        this.count++;
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.itemsChanged();
    }
    public void RemoveItem(int numberOfItems)
    {
        this.count -= numberOfItems;
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.itemsChanged();
        if (this.count == 0)
        {
            inventory= FindObjectOfType<Inventory>();
            inventory.ItemsList.Remove(this);

            Destroy(this.gameObject);
        }
    }
    public void RemoveItem()
    {
        this.count--;
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.itemsChanged();
        if (this.count == 0)
        {
            inventory = FindObjectOfType<Inventory>();
            inventory.ItemsList.Remove(this);
            Destroy(this.gameObject);
        }
    }

}
