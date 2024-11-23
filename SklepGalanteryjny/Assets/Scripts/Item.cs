using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Item : MonoBehaviour
{

    public EventHandler clickedProduct;
    public GameObject productLabel;
    public TMP_Text nazwa;
    public TMP_Text Cena;
    public TMP_Text Opis;
    public TMP_Text Ilosc;
    public Inventory Inventory;
    public string itemName;
    public string description;
    public int itemID;
    public bool isSpecial = false;
    public float baseValue;
    public int count = 1;

    // Constructor
    private void OnEnable()
    {
        productLabel = FindObjectOfType<label>(true).gameObject;
        nazwa = FindObjectOfType<nameText>(true).GetComponent<TMP_Text>();
        Opis = FindObjectOfType<descLabel>(true).GetComponent<TMP_Text>();
        Cena = FindObjectOfType<priceText>(true).GetComponent<TMP_Text>();
        Ilosc = FindObjectOfType<ilosc>(true).GetComponent<TMP_Text>();

    }
    public Item(string name, string desc, int id, bool isSpecial, float baseValue, int count)
    {
        this.itemName = name;
        this.description = desc;
        this.itemID = id;
        this.isSpecial = isSpecial;

        this.count = 1;
    }

    public void AddItem(int numberOfItems)
    {
        this.count += numberOfItems;
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
            inventory = FindObjectOfType<Inventory>();
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
    private void OnMouseEnter()
    {

        productLabel.transform.position = FindAnyObjectByType<Camera>().WorldToScreenPoint(this.transform.position + new Vector3(2f, 1.6f));
        productLabel.gameObject.SetActive(true);
        nazwa.text = this.itemName;
        Cena.text = "Cena: " + this.baseValue.ToString() + " rubli";
        Ilosc.text = "iloœæ: " + this.count.ToString();
        Opis.text = this.description;

    }
    private void OnMouseExit()
    {
        productLabel.gameObject.SetActive(false);

    }
    void OnMouseDown()
    {
        clickedProduct?.Invoke(this, EventArgs.Empty);
    }
}