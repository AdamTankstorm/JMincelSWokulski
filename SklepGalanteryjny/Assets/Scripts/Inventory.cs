using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public class Inventory : MonoBehaviour
{
    public float money = 200f;
    public EventHandler onItemsChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Item> ItemsList;
    public Wardrobe[] wardrobes;
    public Slot[] AvaibleSlots = null;
    public void addWardrabe(Wardrobe wardrobe)
    {
        wardrobe.IsLocked = false;
        wardrobes.Append(wardrobe);
    }

    public List<Slot> getAvaibleSlots()
    {
        List<Slot> slots = new List<Slot>();
        foreach (Wardrobe wardrobe in wardrobes) {
            
            if(wardrobe.getAllAvaibleSlot()!= null)
            {
                slots.AddRange(wardrobe.getAllAvaibleSlot());
            }
            
        }
        if ((slots.Count) > 0)
        {

            return slots;


        }
        else { return null; }

        
    }
    // Update is called once per frame
    public int isItemInInventory(string name)
    {
        if((ItemsList.FindAll(item => item.name == name).Count)>0)
        {
            return ItemsList.FindAll(item => item.name == name).FirstOrDefault().count;
        }

        else { return 0; }

    }
    public int isItemInInventory(int id)
    {
        if ((ItemsList.FindAll(item => item.itemID == id).Count) > 0)
        {
           return ItemsList.FindAll(item => item.itemID == id).FirstOrDefault().count;
        }
        else { return 0; }
    }
    public Item getItemByString(string name)
    {
        if ((ItemsList.FindAll(item => item.name == name).Count) > 0)
        {
            return ItemsList.FirstOrDefault(item => item.itemName == name);
        }
        return null;
    }
    public void itemsChanged()
    {
        onItemsChanged?.Invoke(this, EventArgs.Empty);
    }
}
