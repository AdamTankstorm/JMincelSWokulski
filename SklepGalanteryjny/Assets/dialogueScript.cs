using System.Collections;
using UnityEngine;
using TMPro;

public class dialogueScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to the TextMeshPro component
    public string[] lines;               // Dialogue lines
    public float textSpeed;              // Speed of the typing effect

    private int index;

    void Awake()
    {
        if (textComponent == null)
        {
            // Find TextMeshPro child automatically if not assigned in Inspector
            textComponent = GetComponentInChildren<TextMeshProUGUI>();
        }

        textComponent.text = string.Empty;
        gameObject.SetActive(false); // Start with the dialogue box hidden
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.activeSelf)
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

    public void startDialogue()
    {
        Debug.Log("Starting dialogue typing effect");
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(typeLine());
    }

    public IEnumerator typeLine()
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
