using UnityEngine;
using UnityEngine.UI;

public class BreakSystem : MonoBehaviour
{
    public ButtonHandler buttonHandler;
    public Image backgroundImage; // T³o
    public Image clockBackground; // T³o zegara
    public Image clockHand;       // Wskazówka zegara
    public Image specialItemShopPanel;
    private float fadeTime = 0f;
    public float fadeDuration = 1f;
    private bool isFadingOut = false; // Flaga: œciemnianie
    private bool isFadingIn = false;  // Flaga: rozjaœnianie

    private void Start()
    {
        // Ustaw pocz¹tkow¹ przezroczystoœæ t³a i zegara
        if (backgroundImage != null)
        {
            backgroundImage.gameObject.SetActive(false);
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // T³o jest pocz¹tkowo niewidoczne
        }

        if (clockBackground != null && clockHand != null && specialItemShopPanel != null)
        {
            clockBackground.gameObject.SetActive(true); // Zegar jest pocz¹tkowo widoczny
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
                    ? Mathf.Lerp(0f, 1f, t) // Œciemnianie
                    : Mathf.Lerp(1f, 0f, t); // Rozjaœnianie
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
                        clockBackground.gameObject.SetActive(false); // Wy³¹cz zegar po œciemnieniu
                        clockHand.gameObject.SetActive(false);
                        specialItemShopPanel.gameObject.SetActive(true);
                    }
                    StartFadeIn(); // Automatycznie rozpocznij rozjaœnianie
                }
                else if (isFadingOut && buttonHandler.isExitButtonClicked)
                {
                    isFadingOut = false;
                    if (clockBackground != null && clockHand != null && specialItemShopPanel != null)
                    {
                        clockBackground.gameObject.SetActive(true); // Wy³¹cz zegar po œciemnieniu
                        clockHand.gameObject.SetActive(true);
                        specialItemShopPanel.gameObject.SetActive(false);
                    }
                    StartFadeIn(); // Automatycznie rozpocznij rozjaœnianie
                }


            }
        }
    }

    public void BreakTime()
    {
        // Rozpocznij animacjê œciemniania t³a
        Invoke(nameof(StartFadeOut), 2f); // Wywo³aj animacjê œciemniania po 2 sekundach
    }
    public void BreakTimeOver()
    {
        // Rozpocznij animacjê œciemniania t³a
        Invoke(nameof(StartFadeOut), 0f); // Wywo³aj animacjê œciemniania po 2 sekundach
    }

    private void StartFadeOut()
    {
        fadeTime = 0f; // Zresetuj czas
        isFadingOut = true; // Rozpocznij animacjê œciemniania
    }

    private void StartFadeIn()
    {
        fadeTime = 0f; // Zresetuj czas
        isFadingIn = true; // Rozpocznij animacjê rozjaœniania
    }
}
