using System;
using System.Collections;
using UnityEngine;

public class customerScript : MonoBehaviour
{
    // Declare events
    public event Action customerArrives;
    public event Action customerLeaves;

    // Start is called before the first frame update
    void Start()
    {
        // Trigger the customerArrives event
        StartCoroutine(handleCustomerEvents());
    }

    IEnumerator handleCustomerEvents()
    {
        // Invoke the customerArrives event
        customerArrives?.Invoke();
        Debug.Log("Customer arrived!");

        // Wait for 10 seconds
        yield return new WaitForSeconds(10f);

        // Invoke the customerLeaves event
        customerLeaves?.Invoke();
        Debug.Log("Customer left!");
    }
}
