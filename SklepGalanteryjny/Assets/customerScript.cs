using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customerScript : MonoBehaviour
{
    public event Action customerArrives;
    public event Action customerLeaves;
    // Customer class
    private class Customer
    {
        public int Number { get; set; }
        public string Item { get; set; }

        public Customer(int number, string item)
        {
            Number = number;
            Item = item;
        }

        public override string ToString()
        {
            return $"Customer {Number}: {Item}";
        }
    }

    private List<Customer> customers = new List<Customer>();
    private System.Random random = new System.Random();

    void Start()
    {
        // Initializing with 3 customers
        for (int i = 1; i <= 3; i++)
        {
            customers.Add(new Customer(i, GetRandomItem()));
        }

        // Start the loop
        //StartCoroutine(customerChange());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(customerChange());
        }
    }

    public IEnumerator customerChange()
    {
        if (customers.Count > 0)
        {
            // Remove the first customer
            Debug.Log($"Removing: {customers[0]}");
            customers.RemoveAt(0);

            customerLeaves?.Invoke();

            // Wait for 2 seconds
            yield return new WaitForSeconds(3f);

            // Decrease the numbers of remaining customers
            for (int i = 0; i < customers.Count; i++)
            {
                customers[i].Number = i + 1;
            }

            // Add a new customer with number 3
            if (customers.Count < 3)
            {
                Customer newCustomer = new Customer(3, GetRandomItem());
                customers.Add(newCustomer);
                Debug.Log($"Added: {newCustomer}");
            }

            customerArrives?.Invoke();

            // Log current customers
            LogCurrentCustomers();
            
        }
    }

    private string GetRandomItem()
    {
        // Possible items for the customer
        string[] items = { "len", "jedwab", "cukier", "cynamon", "mydło" };
        return items[random.Next(items.Length)];
    }

    private void LogCurrentCustomers()
    {
        Debug.Log("Current Customers:");
        foreach (var customer in customers)
        {
            Debug.Log(customer);
        }
    }
}
