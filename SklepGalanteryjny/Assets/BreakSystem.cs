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
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // T�o jest pocz�tkowo niewidoczne
        }
    }

    void Update()
    {
        // Zaktualizuj fadeTime w metodzie Update, aby animacja dzia�a�a p�ynnie
        if (fadeTime > 0f)
        {
            fadeTime += Time.deltaTime;
            if (fadeTime > fadeDuration)
            {
                fadeTime = fadeDuration; // Upewnij si�, �e fadeTime nie przekroczy fadeDuration
            }
        }
    }

    public void BreakTime()
    {
        // Uruchom animacj� �ciemniania po 2 sekundach
        Invoke(nameof(StartFade), 2f); // Wywo�aj metod� StartFade po 2 sekundach
    }

    private void StartFade()
    {
        fadeTime = 0f; // Resetuj fadeTime przy rozpocz�ciu animacji

        // Uruchom korutyn� do p�ynnej animacji �ciemniania t�a
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
                backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, Mathf.Lerp(0f, 1f, t)); // Zmiana przezroczysto�ci t�a
            }
            fadeTime += Time.deltaTime; // Dodaj czas w ka�dej klatce
            yield return null; // Poczekaj do nast�pnej klatki
        }

        // Upewnij si�, �e t�o b�dzie mia�o pe�n� przezroczysto�� po zako�czeniu animacji
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 1f);
        }
    }
}
