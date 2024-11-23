using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class managerScript : MonoBehaviour
{
    public customerScript customerManager;
    public dialogueScript dialogueManager;
    public ClockController clockManager;
    public ButtonHandler buttonManager;
    public spawnScript spawnManager;

    // To store the dialogue sets from the CSV file
    private List<DialogueSet> dialogueSets;

    void Start()
    {
        if (customerManager != null)
        {
            Debug.Log("managerScript subscribed to customerScript events");
            customerManager.customerArrives += customerArrives;
            customerManager.customerLeaves += customerLeaves;
        }
        else
        {
            Debug.LogError("customerScript or dialogueManager not found in the scene!");
        }

        if (clockManager != null)
        {
            clockManager.midDay += midDay;
        }

        if (clockManager != null)
        {
            clockManager.endDay += endDay;
        }

        if (clockManager != null)
        {
            clockManager.startDay += startDay;
        }

        if (buttonManager != null)
        {
            buttonManager.lateDay += lateDay;
        }

        // Load the dialogue sets from the CSV file
        LoadDialogueSets();

        Debug.Log("Customer Arrived: Dialogue Started");

        if (dialogueSets == null || dialogueSets.Count == 0)
        {
            Debug.LogError("No dialogue sets available to show!");
            return;
        }

        // Select a random dialogue set from the preloaded list
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

        // Read all lines from the CSV file
        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file does not contain enough data!");
            return;
        }

        // Process each line, skipping the header
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

            // Parse the line while respecting commas within quotes
            string[] columns = ParseCsvLine(line);

            if (columns.Length == 4) // Ensure there are four columns (id, linesA, linesB, item)
            {
                int id;
                if (int.TryParse(columns[0].Trim(), out id)) // Parse the id safely
                {
                    // Only add sets with IDs between 1 and 40
                    if (id >= 1 && id <= 40)
                    {
                        string linesA = columns[1].Trim().Trim('"'); // Get the first dialogue line (linesA)
                        string linesB = columns[2].Trim().Trim('"'); // Get the second dialogue line (linesB)
                        string item = columns[3].Trim().Trim('"'); // Get the item

                        // Add the dialogue set to the list
                        dialogueSets.Add(new DialogueSet(id, linesA, linesB, item));
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Skipping malformed line: {line}");
            }
        }

        // Log the total number of records loaded
        Debug.Log($"Loaded {dialogueSets.Count} dialogue sets with IDs from 1 to 40.");
    }

    private string[] ParseCsvLine(string line)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        string currentField = "";

        foreach (char c in line)
        {
            if (c == '"' && inQuotes)
            {
                // End of quoted section
                inQuotes = false;
            }
            else if (c == '"')
            {
                // Start of quoted section
                inQuotes = true;
            }
            else if (c == ',' && !inQuotes)
            {
                // End of field
                fields.Add(currentField);
                currentField = "";
            }
            else
            {
                // Append to the current field
                currentField += c;
            }
        }

        // Add the last field
        fields.Add(currentField);

        return fields.ToArray();
    }

    public void customerArrives()
    {
        Debug.Log("Customer Arrived: Dialogue Started");

        if (dialogueSets == null || dialogueSets.Count == 0)
        {
            Debug.LogError("No dialogue sets available to show!");
            return;
        }

        // Select a random dialogue set from the preloaded list
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
        clockManager.HandleClient();
    }

    public void midDay()
    {
        StopCoroutine(customerManager.customerChange());
        customerManager.gameObject.SetActive(false);
        
        spawnManager.gameObject.SetActive(false);
    }

    public void lateDay()
    {
        customerManager.gameObject.SetActive(true);
        StartCoroutine(customerManager.customerChange());

        spawnManager.gameObject.SetActive(true);
    }

    public void endDay()
    {
        StopCoroutine(customerManager.customerChange());
        customerManager.gameObject.SetActive(false);
    }

    public void startDay()
    {
        customerManager.gameObject.SetActive(true);
        StartCoroutine(customerManager.customerChange());
    }
}

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
