using System.Collections;
using UnityEngine;
using TMPro;

public class dialogueScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    void Awake()
    {
        textComponent.text = string.Empty;
    }

    void Start()
    {
        // Subscribe to the customer events
        customerScript customerManager = FindObjectOfType<customerScript>();
        if (customerManager != null)
        {
            customerManager.customerArrives += customerArrives;
            customerManager.customerLeaves += customerLeaves;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void customerArrives()
    {
        // Called when the customerArrives event is triggered
        gameObject.SetActive(true);
        startDialogue();
    }

    public void customerLeaves()
    {
        // Called when the customerLeaves event is triggered
        Destroy(gameObject);
    }

    void startDialogue()
    {
        index = 0;
        StartCoroutine(typeLine());
    }

    IEnumerator typeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void nextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(typeLine());
        }
    }
}
