using UnityEngine;
using UnityEngine.UI;
public class BreakSystem : MonoBehaviour
{
    public Image backgroundImage;
    private float fadeTime = 0f;
    public float fadeDuration = 1f;

    private void Start()
    {
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // T³o jest pocz¹tkowo niewidoczne
        }
    }

    void Update()
    {
        // Zaktualizuj fadeTime w metodzie Update, aby animacja dzia³a³a p³ynnie
        if (fadeTime > 0f)
        {
            fadeTime += Time.deltaTime;
            if (fadeTime > fadeDuration)
            {
                fadeTime = fadeDuration; // Upewnij siê, ¿e fadeTime nie przekroczy fadeDuration
            }
        }
    }

    public void BreakTime()
    {
        // Uruchom animacjê œciemniania po 2 sekundach
        Invoke(nameof(StartFade), 2f); // Wywo³aj metodê StartFade po 2 sekundach
    }

    private void StartFade()
    {
        fadeTime = 0f; // Resetuj fadeTime przy rozpoczêciu animacji

        // Uruchom korutynê do p³ynnej animacji œciemniania t³a
        StartCoroutine(FadeBackground());
    }

    private System.Collections.IEnumerator FadeBackground()
    {
        while (fadeTime < fadeDuration)
        {
            float t = fadeTime / fadeDuration;
            if (backgroundImage != null)
            {
                Color bgColor = backgroundImage.color;
                backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, Mathf.Lerp(0f, 1f, t)); // Zmiana przezroczystoœci t³a
            }
            fadeTime += Time.deltaTime; // Dodaj czas w ka¿dej klatce
            yield return null; // Poczekaj do nastêpnej klatki
        }

        // Upewnij siê, ¿e t³o bêdzie mia³o pe³n¹ przezroczystoœæ po zakoñczeniu animacji
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 1f);
        }
    }
}
