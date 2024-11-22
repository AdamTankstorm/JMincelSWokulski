using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class ListOfItems : MonoBehaviour
{
    public VisualTreeAsset listViewAsset;
    public Inventory inventory;

    private void OnEnable()
    {
        inventory.onItemsChanged += HandleEvent;
    }

    private void HandleEvent(object sender, EventArgs e)
    {
        
    }

    void Refresh()
    {

    }
   
   
}


