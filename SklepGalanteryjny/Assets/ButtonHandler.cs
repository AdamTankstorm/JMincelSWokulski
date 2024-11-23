using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour
{
    public BreakSystem breakSystem;
    public Image MorningShop;
    public Transform NotEnoughFunds; // Transform dla komunikatu
    public List<Image> itemInfoPanels; // Lista wszystkich paneli
    public bool isExitButtonClicked = false;
    public Image background; // T³o, które ma p³ynnie przechodziæ
    private float fadeDuration = 1f, fadeDuration1 = 0.2f; // Czas trwania animacji

    private CanvasGroup notEnoughFundsTransform; // Do kontrolowania przezroczystoœci komunikatu

    private void Start()
    {
        // Inicjalizacja komunikatu "NotEnoughFunds"
        if (NotEnoughFunds != null)
        {
            NotEnoughFunds.gameObject.SetActive(false); // Ukryj pocz¹tkowo
            notEnoughFundsTransform = NotEnoughFunds.GetComponent<CanvasGroup>(); // Pobierz CanvasGroup do kontrolowania przezroczystoœci
            if (notEnoughFundsTransform == null)
            {
                notEnoughFundsTransform = NotEnoughFunds.gameObject.AddComponent<CanvasGroup>(); // Jeœli nie ma CanvasGroup, dodaj go
            }
        }

        // Upewnij siê, ¿e panele s¹ przypisane w Inspectorze
        if (itemInfoPanels == null || itemInfoPanels.Count == 0)
        {
            Debug.LogWarning("Brak przypisanych paneli w ButtonHandler!");
        }

        // Ukryj t³o na starcie, jeœli jest przypisane
        if (background != null)
        {
            background.gameObject.SetActive(false);
            Color bgColor = background.color;
            background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // Ustaw przezroczystoœæ na 0
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

    public void StartTheDay()
    {
        if (background != null)
        {
            StartCoroutine(FadeInBackground()); // Rozpocznij p³ynne pojawianie siê t³a
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

    private IEnumerator FadeInBackground()
    {
        if (background != null)
        {
            background.gameObject.SetActive(true);
            float startTime = Time.time;
            Color bgColor = background.color;

            while (Time.time < startTime + fadeDuration)
            {
                float t = (Time.time - startTime) / fadeDuration;
                background.color = new Color(bgColor.r, bgColor.g, bgColor.b, Mathf.Lerp(0f, 1f, t)); // Stopniowe zwiêkszanie alfa
                yield return null;
            }

            background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 1f); // Ustaw pe³n¹ widocznoœæ

            MorningShop.gameObject.SetActive(false);
            StartCoroutine(FadeOutBackground()); // Rozpocznij p³ynne pojawianie siê t³a
        }
    }

    private IEnumerator FadeOutBackground()
    {
        if (background != null)
        {

            float startTime = Time.time;
            Color bgColor = background.color;

            while (Time.time < startTime + fadeDuration)
            {
                float t = (Time.time - startTime) / fadeDuration;
                background.color = new Color(bgColor.r, bgColor.g, bgColor.b, Mathf.Lerp(1f, 0f, t)); // Stopniowe zmniejszanie alfa
                yield return null;
            }

            background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // Ustaw pe³n¹ przezroczystoœæ
            background.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeInNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            NotEnoughFunds.gameObject.SetActive(true); // Upewnij siê, ¿e komunikat jest aktywowany
            notEnoughFundsTransform.alpha = 0f; // Ustaw pocz¹tkow¹ przezroczystoœæ na 0

            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration1)
            {
                notEnoughFundsTransform.alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeDuration1); // Stopniowe zwiêkszanie przezroczystoœci
                yield return null; // Poczekaj do nastêpnej klatki
            }

            notEnoughFundsTransform.alpha = 1f; // Upewnij siê, ¿e na koñcu osi¹gniêto pe³n¹ przezroczystoœæ
        }
    }

    private IEnumerator FadeOutNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration1)
            {
                notEnoughFundsTransform.alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration1); // Stopniowe zmniejszanie przezroczystoœci
                yield return null; // Poczekaj do nastêpnej klatki
            }

            notEnoughFundsTransform.alpha = 0f; // Upewnij siê, ¿e na koñcu osi¹gniêto pe³n¹ przezroczystoœæ
            NotEnoughFunds.gameObject.SetActive(false); // Po zakoñczeniu animacji ukryj obiekt
        }
    }

    private void CheckIfAllPanelsAreInactive()
    {
        foreach (Image panel in itemInfoPanels)
        {
            if (panel.gameObject.activeSelf)
            {
                return; // Jeœli znajdziesz aktywny panel, zakoñcz metodê
            }
        }

        Debug.Log("Wszystkie panele s¹ wy³¹czone!");
        OnAllPanelsInactive();
    }

    private void OnAllPanelsInactive()
    {
        isExitButtonClicked = true;
        if (breakSystem != null)
        {
            breakSystem.BreakTimeOver();
        }
    }
}
