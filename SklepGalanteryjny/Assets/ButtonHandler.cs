using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Do obs�ugi list

public class ButtonHandler : MonoBehaviour
{
    public event Action lateDay;
    public BreakSystem breakSystem;
    public List<Image> itemInfoPanels; // Lista wszystkich paneli
    public bool isExitButtonClicked = false;

    private void Start()
    {
        // Upewnij si�, �e panele s� przypisane w Inspectorze
        if (itemInfoPanels == null || itemInfoPanels.Count == 0)
        {
            Debug.LogWarning("Brak przypisanych paneli w ButtonHandler!");
        }
    }

    public void ExitSpecialItemShopButton()
    {
        isExitButtonClicked = true;
        if (breakSystem != null)
        {
            lateDay?.Invoke();
            breakSystem.BreakTimeOver();
        }
    }

    public void BuySpecialItemButton(Image itemInfoPanel)
    {
        if (itemInfoPanel != null)
        {
            itemInfoPanel.gameObject.SetActive(false); // Wy��cz okre�lony panel
            CheckIfAllPanelsAreInactive(); // Sprawd�, czy wszystkie panele s� wy��czone
        }
    }

    private void CheckIfAllPanelsAreInactive()
    {
        // Sprawd�, czy wszystkie panele na li�cie s� nieaktywne
        foreach (Image panel in itemInfoPanels)
        {
            if (panel.gameObject.activeSelf)
            {
                // Je�li znajdziesz aktywny panel, nie r�b nic wi�cej
                return;
            }
        }

        // Je�li wszystkie panele s� nieaktywne, wykonaj odpowiedni� akcj�
        Debug.Log("Wszystkie panele s� wy��czone!");
        OnAllPanelsInactive();
    }

    private void OnAllPanelsInactive()
    {
        // Tutaj mo�esz wykona� logik�, gdy wszystkie panele s� wy��czone
        // Na przyk�ad zamkn�� sklep
        isExitButtonClicked = true;
        if (breakSystem != null)
        {
            breakSystem.BreakTimeOver();
        }
    }
}
