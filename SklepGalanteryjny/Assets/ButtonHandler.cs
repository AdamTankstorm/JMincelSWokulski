using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Do obs�ugi list

public class ButtonHandler : MonoBehaviour
{
    public event Action lateDay;
    public BreakSystem breakSystem;
    public Transform NotEnoughFunds; // Transform dla komunikatu
    public List<Image> itemInfoPanels; // Lista wszystkich paneli
    public bool isExitButtonClicked = false;
    private float fadeDuration = 0.2f; // Czas trwania animacji

    private CanvasGroup notEnoughFundsCanvasGroup; // Do kontrolowania przezroczysto�ci komunikatu

    private void Start()
    {
        if (NotEnoughFunds != null)
        {
            NotEnoughFunds.gameObject.SetActive(false); // Ukryj pocz�tkowo
            notEnoughFundsCanvasGroup = NotEnoughFunds.GetComponent<CanvasGroup>(); // Pobierz CanvasGroup do kontrolowania przezroczysto�ci
            if (notEnoughFundsCanvasGroup == null)
            {
                notEnoughFundsCanvasGroup = NotEnoughFunds.gameObject.AddComponent<CanvasGroup>(); // Je�li nie ma CanvasGroup, dodaj go
            }
        }

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

    public void CloseWarningPopup()
    {
        StartCoroutine(FadeOutNotEnoughFunds()); // Rozpocznij p�ynne znikanie komunikatu
    }

    public void BuySpecialItemButton(Image itemInfoPanel)
    {
        if (itemInfoPanel != null)
        {
            int test = 0;
            if (test != 0)
            {
                itemInfoPanel.gameObject.SetActive(false); // Wy��cz okre�lony panel
                CheckIfAllPanelsAreInactive(); // Sprawd�, czy wszystkie panele s� wy��czone
            }
            else
            {
                StartCoroutine(FadeInNotEnoughFunds()); // P�ynne pojawienie si� komunikatu
            }
        }
    }

    private IEnumerator FadeInNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            NotEnoughFunds.gameObject.SetActive(true); // Upewnij si�, �e komunikat jest aktywowany
            notEnoughFundsCanvasGroup.alpha = 0f; // Ustaw pocz�tkow� przezroczysto�� na 0

            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration)
            {
                notEnoughFundsCanvasGroup.alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeDuration); // Stopniowe zwi�kszanie przezroczysto�ci
                yield return null; // Poczekaj do nast�pnej klatki
            }

            notEnoughFundsCanvasGroup.alpha = 1f; // Upewnij si�, �e na ko�cu osi�gni�to pe�n� przezroczysto��
        }
    }

    private IEnumerator FadeOutNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration)
            {
                notEnoughFundsCanvasGroup.alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration); // Stopniowe zmniejszanie przezroczysto�ci
                yield return null; // Poczekaj do nast�pnej klatki
            }

            notEnoughFundsCanvasGroup.alpha = 0f; // Upewnij si�, �e na ko�cu osi�gni�to pe�n� przezroczysto��
            NotEnoughFunds.gameObject.SetActive(false); // Po zako�czeniu animacji ukryj obiekt
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
