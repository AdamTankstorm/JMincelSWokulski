using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
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
    public bool isItemInInventory(string name)
    {
        if((ItemsList.FindAll(item => item.name == name).Count)>0)
        {
            return true;
        }

        else { return false; }

    }
    public bool isItemInInventory(int id)
    {
        if ((ItemsList.FindAll(item => item.itemID == id).Count) > 0)
        {
            return true;
        }
        else { return false; }
    }
}
