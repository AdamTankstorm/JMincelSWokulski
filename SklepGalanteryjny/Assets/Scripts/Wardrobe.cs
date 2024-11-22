using System.Linq;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    public Slot[] Slots;
    public bool IsLocked = false;
    public Inventory Inventory;
    public Slot[] getAllAvaibleSlot()
    {
        Slot[] slotsAvaible = null;
        foreach(Slot slot in Slots)
        {
            if(slot.item != null)
            {
                slotsAvaible.Append(slot);
            }
        }

        return slotsAvaible;
    }

}
