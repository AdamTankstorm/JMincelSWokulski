using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour
{
    public BreakSystem breakSystem;
    public Transform NotEnoughFunds; // Transform dla komunikatu
    public List<Image> itemInfoPanels; // Lista wszystkich paneli
    public bool isExitButtonClicked = false;
    private float fadeDuration = 0.2f; // Czas trwania animacji

    private CanvasGroup notEnoughFundsCanvasGroup; // Do kontrolowania przezroczystoœci komunikatu

    private void Start()
    {
        if (NotEnoughFunds != null)
        {
            NotEnoughFunds.gameObject.SetActive(false); // Ukryj pocz¹tkowo
            notEnoughFundsCanvasGroup = NotEnoughFunds.GetComponent<CanvasGroup>(); // Pobierz CanvasGroup do kontrolowania przezroczystoœci
            if (notEnoughFundsCanvasGroup == null)
            {
                notEnoughFundsCanvasGroup = NotEnoughFunds.gameObject.AddComponent<CanvasGroup>(); // Jeœli nie ma CanvasGroup, dodaj go
            }
        }

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

    public void CloseWarningPopup()
    {
        StartCoroutine(FadeOutNotEnoughFunds()); // Rozpocznij p³ynne znikanie komunikatu
    }

    public void BuySpecialItemButton(Image itemInfoPanel)
    {
        if (itemInfoPanel != null)
        {
            int test = 0;
            if (test != 0)
            {
                itemInfoPanel.gameObject.SetActive(false); // Wy³¹cz okreœlony panel
                CheckIfAllPanelsAreInactive(); // SprawdŸ, czy wszystkie panele s¹ wy³¹czone
            }
            else
            {
                StartCoroutine(FadeInNotEnoughFunds()); // P³ynne pojawienie siê komunikatu
            }
        }
    }

    private IEnumerator FadeInNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            NotEnoughFunds.gameObject.SetActive(true); // Upewnij siê, ¿e komunikat jest aktywowany
            notEnoughFundsCanvasGroup.alpha = 0f; // Ustaw pocz¹tkow¹ przezroczystoœæ na 0

            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration)
            {
                notEnoughFundsCanvasGroup.alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeDuration); // Stopniowe zwiêkszanie przezroczystoœci
                yield return null; // Poczekaj do nastêpnej klatki
            }

            notEnoughFundsCanvasGroup.alpha = 1f; // Upewnij siê, ¿e na koñcu osi¹gniêto pe³n¹ przezroczystoœæ
        }
    }

    private IEnumerator FadeOutNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration)
            {
                notEnoughFundsCanvasGroup.alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration); // Stopniowe zmniejszanie przezroczystoœci
                yield return null; // Poczekaj do nastêpnej klatki
            }

            notEnoughFundsCanvasGroup.alpha = 0f; // Upewnij siê, ¿e na koñcu osi¹gniêto pe³n¹ przezroczystoœæ
            NotEnoughFunds.gameObject.SetActive(false); // Po zakoñczeniu animacji ukryj obiekt
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
