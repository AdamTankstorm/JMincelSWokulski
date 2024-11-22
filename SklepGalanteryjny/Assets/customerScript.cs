using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class customerScript : MonoBehaviour
{
    // Events for customer arrival and departure
    public event Action customerArrives;
    public event Action customerLeaves;

    void Start()
    {
        Debug.Log("customerScript started: Waiting for events to trigger");
        StartCoroutine(handleCustomerEvents());
    }

    IEnumerator handleCustomerEvents()
    {
        while (true) // This creates an infinite loop for continuous customer events
        {
            // Wait 12 seconds before the next customer arrives
            yield return new WaitForSeconds(5f);
            Debug.Log("Triggering customerArrives event");
            customerArrives?.Invoke();  // Trigger the customerArrives event

            // Wait for 7 seconds after the customer arrives for them to leave
            yield return new WaitForSeconds(5f);
            Debug.Log("Triggering customerLeaves event");
            customerLeaves?.Invoke();  // Trigger the customerLeaves event
        }
    }
}
