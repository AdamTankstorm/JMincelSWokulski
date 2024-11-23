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
    public GameObject shopManager;
    public haggling hagglingManager;

    private List<DialogueSet> dialogueSets;
    private List<DialogueSet> unpleasantDialogueSets;
    private List<DialogueSet> pleasantDialogueSets;

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

        if (hagglingManager != null)
        {
            hagglingManager.win += win;
            hagglingManager.draw += draw;
            hagglingManager.loss += loss;
        }

        LoadDialogueSets();
        Debug.Log("Customer Arrived: Dialogue Started");

        if (dialogueSets == null || dialogueSets.Count == 0)
        {
            Debug.LogError("No dialogue sets available to show!");
            return;
        }

        DialogueSet selectedSet = dialogueSets[Random.Range(0, dialogueSets.Count)];

        if (selectedSet == null || string.IsNullOrEmpty(selectedSet.LinesA) || string.IsNullOrEmpty(selectedSet.LinesB))
        {
            Debug.LogError("Selected dialogue set has no lines to display.");
            return;
        }

        dialogueManager.gameObject.SetActive(true);
        dialogueManager.lines = new string[] { selectedSet.LinesA, selectedSet.LinesB };

        dialogueManager.startDialogue();
    }

    private void LoadDialogueSets()
    {
        dialogueSets = new List<DialogueSet>();
        unpleasantDialogueSets = new List<DialogueSet>();
        pleasantDialogueSets = new List<DialogueSet>();

        string filePath = Path.Combine(Application.dataPath, "dialogueLines.csv");

        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV file not found at path: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length <= 1)
        {
            Debug.LogError("CSV file does not contain enough data!");
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] columns = ParseCsvLine(line);

            if (columns.Length == 4)
            {
                int id;
                if (int.TryParse(columns[0].Trim(), out id))
                {
                    if (id >= 1 && id <= 40)
                    {
                        string linesA = columns[1].Trim().Trim('"');
                        string linesB = columns[2].Trim().Trim('"');
                        string item = columns[3].Trim().Trim('"');

                        dialogueSets.Add(new DialogueSet(id, linesA, linesB, item));
                    }
                    else if (id >= 41 && id <= 60)
                    {
                        string linesA = columns[1].Trim().Trim('"');
                        string linesB = columns[2].Trim().Trim('"');
                        string item = columns[3].Trim().Trim('"');

                        unpleasantDialogueSets.Add(new DialogueSet(id, linesA, linesB, item));
                        pleasantDialogueSets.Add(new DialogueSet(id, linesA, linesB, item));
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Skipping malformed line: {line}");
            }
        }

        Debug.Log($"Loaded {dialogueSets.Count} dialogue sets with IDs from 1 to 40.");
        Debug.Log($"Loaded {unpleasantDialogueSets.Count} unpleasant dialogue sets with IDs from 41 to 60.");
        Debug.Log($"Loaded {pleasantDialogueSets.Count} pleasant dialogue sets with IDs from 41 to 60.");
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
                inQuotes = false;
            }
            else if (c == '"')
            {
                inQuotes = true;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(currentField);
                currentField = "";
            }
            else
            {
                currentField += c;
            }
        }

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

        DialogueSet selectedSet = dialogueSets[Random.Range(0, dialogueSets.Count)];

        if (selectedSet == null || string.IsNullOrEmpty(selectedSet.LinesA) || string.IsNullOrEmpty(selectedSet.LinesB))
        {
            Debug.LogError("Selected dialogue set has no lines to display.");
            return;
        }

        dialogueManager.gameObject.SetActive(true);
        dialogueManager.lines = new string[] { selectedSet.LinesA, selectedSet.LinesB };

        dialogueManager.startDialogue();

        hagglingManager.gameObject.SetActive(true);
    }

    public void customerLeaves()
    {
        clockManager.HandleClient();
    }

    public void win()
    {
        unpleasantGoodbye();
    }

    public void draw()
    {
        unpleasantGoodbye();
    }

    public void loss()
    {
        pleasantGoodbye();
    }

    public void pleasantGoodbye()
    {
        Debug.Log("Triggering pleasant goodbye.");
        if (pleasantDialogueSets != null && pleasantDialogueSets.Count > 0)
        {
            DialogueSet selectedSet = pleasantDialogueSets[Random.Range(0, pleasantDialogueSets.Count)];

            if (selectedSet == null || string.IsNullOrEmpty(selectedSet.LinesA))
            {
                Debug.LogError("Selected pleasant dialogue set has no lines to display.");
                return;
            }

            Debug.Log($"Selected pleasant dialogue: {selectedSet.LinesA}");

            dialogueManager.gameObject.SetActive(true);
            dialogueManager.lines = new string[] { selectedSet.LinesA };  // Only using LinesA for pleasant goodbye

            dialogueManager.startDialogue();
        }
        else
        {
            Debug.LogError("No pleasant goodbye dialogue sets available.");
        }

        StartCoroutine(customerManager.customerChange());
    }

    public void unpleasantGoodbye()
    {
        Debug.Log("Triggering unpleasant goodbye.");
        if (unpleasantDialogueSets != null && unpleasantDialogueSets.Count > 0)
        {
            DialogueSet selectedSet = unpleasantDialogueSets[Random.Range(0, unpleasantDialogueSets.Count)];

            if (selectedSet == null || string.IsNullOrEmpty(selectedSet.LinesB))
            {
                Debug.LogError("Selected unpleasant dialogue set has no lines to display.");
                return;
            }

            Debug.Log($"Selected unpleasant dialogue: {selectedSet.LinesB}");  // Using LinesB for unpleasant goodbye

            dialogueManager.gameObject.SetActive(true);
            dialogueManager.lines = new string[] { selectedSet.LinesB };  // Only using LinesB for unpleasant goodbye

            dialogueManager.startDialogue();
        }
        else
        {
            Debug.LogError("No unpleasant goodbye dialogue sets available.");
        }

        StartCoroutine(customerManager.customerChange());
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
        shopManager.SetActive(true);

        customerManager.gameObject.SetActive(true);
        StartCoroutine(customerManager.customerChange());
    }
}

[System.Serializable]
public class DialogueSet
{
    public int id;
    public string LinesA;
    public string LinesB;
    public string Item;

    public DialogueSet(int id, string linesA, string linesB, string item)
    {
        this.id = id;
        this.LinesA = linesA;
        this.LinesB = linesB;
        this.Item = item;
    }
}
