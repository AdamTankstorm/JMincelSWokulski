using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Do obs³ugi list

public class ButtonHandler : MonoBehaviour
{
    public BreakSystem breakSystem;
    public List<Image> itemInfoPanels; // Lista wszystkich paneli
    public bool isExitButtonClicked = false;

    private void Start()
    {
        // Upewnij siê, ¿e panele s¹ przypisane w Inspectorze
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
            breakSystem.BreakTimeOver();
        }
    }

    public void BuySpecialItemButton(Image itemInfoPanel)
    {
        if (itemInfoPanel != null)
        {
            itemInfoPanel.gameObject.SetActive(false); // Wy³¹cz okreœlony panel
            CheckIfAllPanelsAreInactive(); // SprawdŸ, czy wszystkie panele s¹ wy³¹czone
        }
    }

    private void CheckIfAllPanelsAreInactive()
    {
        // SprawdŸ, czy wszystkie panele na liœcie s¹ nieaktywne
        foreach (Image panel in itemInfoPanels)
        {
            if (panel.gameObject.activeSelf)
            {
                // Jeœli znajdziesz aktywny panel, nie rób nic wiêcej
                return;
            }
        }

        // Jeœli wszystkie panele s¹ nieaktywne, wykonaj odpowiedni¹ akcjê
        Debug.Log("Wszystkie panele s¹ wy³¹czone!");
        OnAllPanelsInactive();
    }

    private void OnAllPanelsInactive()
    {
        // Tutaj mo¿esz wykonaæ logikê, gdy wszystkie panele s¹ wy³¹czone
        // Na przyk³ad zamkn¹æ sklep
        isExitButtonClicked = true;
        if (breakSystem != null)
        {
            breakSystem.BreakTimeOver();
        }
    }
}
