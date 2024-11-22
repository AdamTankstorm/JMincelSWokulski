using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO; // For reading from file system

public class managerScript : MonoBehaviour
{
    public customerScript customerManager;
    public dialogueScript dialogueManager;

    // To store the dialogue sets from the CSV file
    private List<DialogueSet> dialogueSets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (customerManager != null && dialogueManager != null)
        {
            Debug.Log("managerScript subscribed to customerScript events");
            customerManager.customerArrives += customerArrives;
            customerManager.customerLeaves += customerLeaves;
        }
        else
        {
            Debug.LogError("customerScript or dialogueManager not found in the scene!");
        }

        // Load the dialogue sets from the CSV file
        LoadDialogueSets();
    }

    // Load the dialogue sets from the CSV file
    private void LoadDialogueSets()
    {
        dialogueSets = new List<DialogueSet>();

        // Get the path of the CSV file in the Assets folder
        string filePath = Path.Combine(Application.dataPath, "dialogueLines.csv");

        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV file not found at path: " + filePath);
            return;
        }

        // Read the CSV file as text
        string fileContents = File.ReadAllText(filePath);

        // Split the CSV by new line to process each line
        string[] lines = fileContents.Split(new char[] { '\n' });

        // Process each line to extract the id, linesA, linesB, and item
        foreach (string line in lines.Skip(1)) // Skip header line
        {
            string[] columns = line.Split(',');

            if (columns.Length == 4) // Ensure there are four columns (id, linesA, linesB, item)
            {
                int id = int.Parse(columns[0].Trim()); // Get the id
                string linesA = columns[1].Trim().Trim('"'); // Get the first dialogue line (linesA)
                string linesB = columns[2].Trim().Trim('"'); // Get the second dialogue line (linesB)
                string item = columns[3].Trim().Trim('"'); // Get the item

                // Add the new dialogue set to the list
                dialogueSets.Add(new DialogueSet(id, linesA, linesB, item));
            }
        }

        // Log the count of dialogue sets loaded
        Debug.Log($"Loaded {dialogueSets.Count} dialogue sets from CSV.");
    }

    public void customerArrives()
    {
        Debug.Log("Customer Arrived: Dialogue Started");

        if (dialogueSets == null || dialogueSets.Count == 0)
        {
            Debug.LogError("No dialogue sets available to show!");
            return;
        }

        // Select a random dialogue set, if available
        DialogueSet selectedSet = dialogueSets[Random.Range(0, dialogueSets.Count)];

        if (selectedSet == null || string.IsNullOrEmpty(selectedSet.LinesA) || string.IsNullOrEmpty(selectedSet.LinesB))
        {
            Debug.LogError("Selected dialogue set has no lines to display.");
            return;
        }

        // Set the lines in the dialogueManager
        dialogueManager.gameObject.SetActive(true); // Show the dialogue box
        dialogueManager.lines = new string[] { selectedSet.LinesA, selectedSet.LinesB };

        dialogueManager.startDialogue();
    }

    public void customerLeaves()
    {
        Debug.Log("Customer Left: Dialogue Stopped");
        dialogueManager.gameObject.SetActive(false); // Hide the dialogue box
    }
}

// Class to store dialogue sets (id, linesA, linesB, and item)
[System.Serializable]
public class DialogueSet
{
    public int id;
    public string LinesA; // First dialogue line
    public string LinesB; // Second dialogue line
    public string Item;

    public DialogueSet(int id, string linesA, string linesB, string item)
    {
        this.id = id;
        this.LinesA = linesA;
        this.LinesB = linesB;
        this.Item = item;
    }
}
