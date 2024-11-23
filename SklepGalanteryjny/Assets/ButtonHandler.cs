using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Do obs�ugi list
using TMPro;
public class ButtonHandler : MonoBehaviour
{
    public event Action lateDay;
    public BreakSystem breakSystem;
    public Image MorningShop;
    public Transform NotEnoughFunds; // Transform dla komunikatu
    public List<Image> itemInfoPanels; // Lista wszystkich paneli
    public bool isExitButtonClicked = false;
    public Image background; // T�o, kt�re ma p�ynnie przechodzi�
    private float fadeDuration = 1f, fadeDuration1 = 0.2f; // Czas trwania animacji
    public Inventory inventory;
    private CanvasGroup notEnoughFundsTransform; // Do kontrolowania przezroczysto�ci komunikatu
    public TMP_Text moneyText;
    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        moneyText.text = "Ilość rubli: "+inventory.money.ToString();

        // Inicjalizacja komunikatu "NotEnoughFunds"
        if (NotEnoughFunds != null)
        {
            
            NotEnoughFunds.gameObject.SetActive(false); // Ukryj pocz�tkowo
            notEnoughFundsTransform = NotEnoughFunds.GetComponent<CanvasGroup>(); // Pobierz CanvasGroup do kontrolowania przezroczysto�ci
            if (notEnoughFundsTransform == null)
            {
                notEnoughFundsTransform = NotEnoughFunds.gameObject.AddComponent<CanvasGroup>(); // Je�li nie ma CanvasGroup, dodaj go
            }
        }

        // Upewnij si�, �e panele s� przypisane w Inspectorze
        if (itemInfoPanels == null || itemInfoPanels.Count == 0)
        {
            Debug.LogWarning("Brak przypisanych paneli w ButtonHandler!");
        }

        // Ukryj t�o na starcie, je�li jest przypisane
        if (background != null)
        {
            background.gameObject.SetActive(false);
            Color bgColor = background.color;
            background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // Ustaw przezroczysto�� na 0
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

    public void StartTheDay()
    {
        if (background != null)
        {
            StartCoroutine(FadeInBackground()); // Rozpocznij p�ynne pojawianie si� t�a
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
                background.color = new Color(bgColor.r, bgColor.g, bgColor.b, Mathf.Lerp(0f, 1f, t)); // Stopniowe zwi�kszanie alfa
                yield return null;
            }

            background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 1f); // Ustaw pe�n� widoczno��

            MorningShop.gameObject.SetActive(false);
            StartCoroutine(FadeOutBackground()); // Rozpocznij p�ynne pojawianie si� t�a
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

            background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // Ustaw pe�n� przezroczysto��
            background.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeInNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            NotEnoughFunds.gameObject.SetActive(true); // Upewnij si�, �e komunikat jest aktywowany
            notEnoughFundsTransform.alpha = 0f; // Ustaw pocz�tkow� przezroczysto�� na 0

            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration1)
            {
                notEnoughFundsTransform.alpha = Mathf.Lerp(0f, 1f, (Time.time - startTime) / fadeDuration1); // Stopniowe zwi�kszanie przezroczysto�ci
                yield return null; // Poczekaj do nast�pnej klatki
            }

            notEnoughFundsTransform.alpha = 1f; // Upewnij si�, �e na ko�cu osi�gni�to pe�n� przezroczysto��
        }
    }

    private IEnumerator FadeOutNotEnoughFunds()
    {
        if (NotEnoughFunds != null)
        {
            float startTime = Time.time;

            while (Time.time < startTime + fadeDuration1)
            {
                notEnoughFundsTransform.alpha = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeDuration1); // Stopniowe zmniejszanie przezroczysto�ci
                yield return null; // Poczekaj do nast�pnej klatki
            }

            notEnoughFundsTransform.alpha = 0f; // Upewnij si�, �e na ko�cu osi�gni�to pe�n� przezroczysto��
            NotEnoughFunds.gameObject.SetActive(false); // Po zako�czeniu animacji ukryj obiekt
        }
    }

    private void CheckIfAllPanelsAreInactive()
    {
        foreach (Image panel in itemInfoPanels)
        {
            if (panel.gameObject.activeSelf)
            {
                return; // Je�li znajdziesz aktywny panel, zako�cz metod�
            }
        }

        Debug.Log("Wszystkie panele s� wy��czone!");
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
