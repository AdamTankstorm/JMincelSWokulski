using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class spawnScript : MonoBehaviour
{
    public customerScript customerReference;

    public GameObject Customer1; // Drag Customer1 prefab in the Inspector
    public GameObject Customer2; // Drag Customer2 prefab in the Inspector
    public GameObject Customer3; // Drag Customer3 prefab in the Inspector

    public Transform Spawner1; // Drag Spawner1 transform in the Inspector
    public Transform Spawner2; // Drag Spawner2 transform in the Inspector
    public Transform Spawner3; // Drag Spawner3 transform in the Inspector

    private List<Transform> availableSpawners = new List<Transform>(); // List of available spawners
    private Dictionary<Transform, GameObject> spawnedCustomers = new Dictionary<Transform, GameObject>(); // Dictionary to track which customer was spawned at which spawner

    void Start()
    {
        customerReference.customerLeaves += customerLeaves; // Subscribe to the customerLeaves event
        // Initialize the list of available spawners
        availableSpawners.Add(Spawner1);
        availableSpawners.Add(Spawner2);
        availableSpawners.Add(Spawner3);

        // Spawn customers at random positions
        SpawnCustomers();
    }

    void SpawnCustomers()
    {
        // Keep spawning customers until all spawners are used
        while (availableSpawners.Count > 0)
        {
            // Select a random spawner from the available list
            int randomIndex = UnityEngine.Random.Range(0, availableSpawners.Count); // Use UnityEngine.Random
            Transform selectedSpawner = availableSpawners[randomIndex];

            // Spawn a random customer at the selected spawner
            GameObject spawnedCustomer = SpawnCustomer(selectedSpawner);

            // Store the relationship between spawner and customer
            spawnedCustomers[selectedSpawner] = spawnedCustomer;

            // Remove the selected spawner from the available list
            availableSpawners.RemoveAt(randomIndex);
        }
    }

    GameObject SpawnCustomer(Transform spawner)
    {
        // Randomly select a customer prefab
        GameObject selectedCustomer = GetRandomCustomer();

        // Instantiate the selected customer at the spawner's position and rotation
        return Instantiate(selectedCustomer, spawner.position, spawner.rotation);
    }

    GameObject GetRandomCustomer()
    {
        // Randomly select a number between 0 and 2
        int randomIndex = UnityEngine.Random.Range(0, 3); // Use UnityEngine.Random

        // Return the corresponding customer prefab
        switch (randomIndex)
        {
            case 0:
                return Customer1;
            case 1:
                return Customer2;
            case 2:
                return Customer3;
            default:
                return Customer1; // Default case (shouldn't happen)
        }
    }

    // Function that moves the customer 20 units to the right if they spawned on Spawner1
    public void customerLeaves()
    {
        // List to track which spawners need to be removed after movement
        List<Transform> spawnersToRemove = new List<Transform>();

        // Start coroutines to move customers and only after that, we perform shifting
        StartCoroutine(MoveCustomersAndShift(spawnersToRemove));
    }

    // Coroutine to move customers and then perform the shifting and destruction
    private IEnumerator MoveCustomersAndShift(List<Transform> spawnersToRemove)
    {
        // Move all customers first, before shifting or destroying
        foreach (var entry in spawnedCustomers)
        {
            Transform originalSpawner = entry.Key;
            GameObject customer = entry.Value;

            // If the customer spawned from Spawner1, move it 20 units to the right in 2 seconds
            if (originalSpawner == Spawner1)
            {
                StartCoroutine(MoveCustomerRight(customer, 20f)); // Move 20 units for customer from Spawner1
                spawnersToRemove.Add(originalSpawner); // Mark this spawner for removal after movement
            }

            // If the customer spawned from Spawner2 or Spawner3, move it 3 units to the right in 2 seconds
            if (originalSpawner == Spawner2 || originalSpawner == Spawner3)
            {
                StartCoroutine(MoveCustomerRight(customer, 3f)); // Move 3 units for customer from Spawner2/3
            }
        }

        // Wait for all customers to finish their movement
        yield return new WaitForSeconds(2f); // Assuming all movements finish in 2 seconds

        // After all movements complete, we can safely destroy customers and reindex
        foreach (var spawner in spawnersToRemove)
        {
            if (spawnedCustomers.ContainsKey(spawner))
            {
                GameObject customerToRemove = spawnedCustomers[spawner];
                Destroy(customerToRemove); // Destroy customer at index 1
                spawnedCustomers.Remove(spawner); // Remove it from the dictionary
            }
        }

        // After destruction, shift the remaining customers down
        List<Transform> spawners = new List<Transform>(spawnedCustomers.Keys);

        // Shift the remaining customers
        if (spawners.Count > 0)
        {
            // Spawner2 becomes Spawner1, Spawner3 becomes Spawner2
            // Update the dictionary accordingly
            if (spawners.Count > 0) spawnedCustomers[Spawner1] = spawnedCustomers[spawners[0]]; // Customer at Spawner2 becomes Customer at Spawner1
            if (spawners.Count > 1) spawnedCustomers[Spawner2] = spawnedCustomers[spawners[1]]; // Customer at Spawner3 becomes Customer at Spawner2

            // Optionally, you can also reset the Spawner3 (it will now be empty).
            if (spawners.Count > 2) spawnedCustomers.Remove(spawners[2]); // Remove Spawner3 entry if there was one
        }

        // Spawn a new customer at Spawner3 (index 3)
        GameObject newCustomer = SpawnCustomer(Spawner3); // Spawn a new customer at Spawner3
        spawnedCustomers[Spawner3] = newCustomer; // Add the new customer to the dictionary
    }

    // Coroutine to move the customer by the specified amount to the right
    private IEnumerator MoveCustomerRight(GameObject customer, float distance)
    {
        Vector3 startPosition = customer.transform.position;
        Vector3 targetPosition = startPosition + Vector3.right * distance; // Move the specified distance to the right
        float duration = 2f; // 2 seconds
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            customer.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final position is exactly the target position
        customer.transform.position = targetPosition;
    }
}
