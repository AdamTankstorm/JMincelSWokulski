using UnityEngine;
using UnityEngine.UI;

public class BreakSystem : MonoBehaviour
{
    public ButtonHandler buttonHandler;
    public Image backgroundImage; // T�o
    public Image clockBackground; // T�o zegara
    public Image clockHand;       // Wskaz�wka zegara
    public Image specialItemShopPanel;
    private float fadeTime = 0f;
    public float fadeDuration = 1f;
    private bool isFadingOut = false; // Flaga: �ciemnianie
    private bool isFadingIn = false;  // Flaga: rozja�nianie

    private void Start()
    {
        // Ustaw pocz�tkow� przezroczysto�� t�a i zegara
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(false);
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // T�o jest pocz�tkowo niewidoczne
        }

        if (clockBackground != null && clockHand != null && specialItemShopPanel != null)
        {
            clockBackground.gameObject.SetActive(true); // Zegar jest pocz�tkowo widoczny
            clockHand.gameObject.SetActive(true);
            specialItemShopPanel.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isFadingOut || isFadingIn)
        {
            fadeTime += Time.deltaTime;

            float t = Mathf.Clamp01(fadeTime / fadeDuration);
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(true);
                Color bgColor = backgroundImage.color;
                float alpha = isFadingOut
                    ? Mathf.Lerp(0f, 1f, t) // �ciemnianie
                    : Mathf.Lerp(1f, 0f, t); // Rozja�nianie
                backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, alpha);
            }

            if (fadeTime >= fadeDuration)
            {
                fadeTime = 0f; // Resetuj czas
                if (isFadingIn)
                {
                    isFadingIn = false;
                    backgroundImage.gameObject.SetActive(false);
                }

                if (isFadingOut && !buttonHandler.isExitButtonClicked)
                {
                    isFadingOut = false;
                    if (clockBackground != null && clockHand != null && specialItemShopPanel != null)
                    {
                        clockBackground.gameObject.SetActive(false); // Wy��cz zegar po �ciemnieniu
                        clockHand.gameObject.SetActive(false);
                        specialItemShopPanel.gameObject.SetActive(true);
                    }
                    StartFadeIn(); // Automatycznie rozpocznij rozja�nianie
                }
                else if (isFadingOut && buttonHandler.isExitButtonClicked)
                {
                    isFadingOut = false;
                    if (clockBackground != null && clockHand != null && specialItemShopPanel != null)
                    {
                        clockBackground.gameObject.SetActive(true); // Wy��cz zegar po �ciemnieniu
                        clockHand.gameObject.SetActive(true);
                        specialItemShopPanel.gameObject.SetActive(false);
                    }
                    StartFadeIn(); // Automatycznie rozpocznij rozja�nianie
                }


            }
        }
    }

    public void BreakTime()
    {
        // Rozpocznij animacj� �ciemniania t�a
        Invoke(nameof(StartFadeOut), 2f); // Wywo�aj animacj� �ciemniania po 2 sekundach
    }
    public void BreakTimeOver()
    {
        // Rozpocznij animacj� �ciemniania t�a
        Invoke(nameof(StartFadeOut), 0f); // Wywo�aj animacj� �ciemniania po 2 sekundach
    }

    private void StartFadeOut()
    {
        fadeTime = 0f; // Zresetuj czas
        isFadingOut = true; // Rozpocznij animacj� �ciemniania
    }

    private void StartFadeIn()
    {
        fadeTime = 0f; // Zresetuj czas
        isFadingIn = true; // Rozpocznij animacj� rozja�niania
    }
}
