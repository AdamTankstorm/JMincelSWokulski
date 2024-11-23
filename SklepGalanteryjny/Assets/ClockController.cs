using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{
    public event Action midDay;
    public event Action endDay;
    public event Action startDay;

    public BreakSystem breakSystem;

    public Transform clockHand; // Wskaz�wka zegara
    public RectTransform clockUI; // RectTransform zegara (dla animacji pozycji i skalowania)
    public Image backgroundImage; // Obiekt UI Image dla t�a
    public Sprite targetBackgroundSprite; // Obrazek t�a (Source Image)
    public Vector3 targetPosition = Vector3.zero; // Pozycja �rodka ekranu
    public Vector3 targetScale = Vector3.one * 4f; // Docelowa skala (powi�kszenie)
    public float moveDuration = 1f; // Czas trwania animacji przesuwania i skalowania
    public float waitBeforeMove = 2f; // Czas oczekiwania przed przesuni�ciem
    public float fadeDuration = 1f; // Czas trwania animacji "pojawniania si�" t�a

    private float currentAngle = -180f; // Startowa pozycja (270 stopni)
    private const float stepAngle = 22.5f; // Przesuni�cie o 22,5 stopnia na klienta
    private const float resetAngle = 0f; // K�t resetu (90 stopni)
    private const float startAngle = -180f; // Pocz�tkowy k�t po resecie
    public float animationDuration = 0.5f; // Czas trwania animacji wskaz�wki

    private bool isAnimating = false;
    private bool canReset = false; // Flaga do kontrolowania resetu
    private bool isMovingClock = false; // Flaga przesuwania zegara
    private bool isReturningClock = false; // Flaga przesuwania zegara
    private bool resetAfterMove = false; // Flaga do uruchomienia resetu po zako�czeniu animacji ruchu

    private float targetAngle;
    private float animationStartAngle;
    private float animationTime;

    private Vector3 originalPosition; // Pocz�tkowa pozycja zegara
    private Vector3 originalScale; // Pocz�tkowa skala zegara
    private float moveTime;
    private float fadeTime = 0f; // Czas trwania animacji pojawiania si� t�a

    void Start()
    {
        // Zapisz oryginaln� pozycj� i skal� zegara
        originalPosition = clockUI.anchoredPosition;
        originalScale = clockUI.localScale;

        // Ustaw pocz�tkow� przezroczysto�� t�a na pe�n� przezroczysto�� (przezroczyste)
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // T�o jest pocz�tkowo niewidoczne
        }

        // Ustaw pocz�tkowy obrazek t�a
        if (backgroundImage != null && targetBackgroundSprite != null)
        {
            backgroundImage.sprite = targetBackgroundSprite; // Ustaw t�o na obrazek
        }
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleClient();
        }*/

        if (isAnimating)
        {
            AnimateClockHand();
        }

        if (isMovingClock)
        {
            MoveClockToCenter();
        }

        if (isReturningClock)
        {
            ReturnClock();
        }

        if (fadeTime > 0f)
        {
            FadeBackground();
        }

        // Je�li k�t osi�gnie 90 stopni, rozpocznij animacj� przesuwania
        if (currentAngle >= resetAngle && !isMovingClock && !resetAfterMove)
        {
            endDay?.Invoke();
            Invoke(nameof(StartClockMove), waitBeforeMove); // Czekaj 2 sekundy, zanim zaczniesz przesuwa� zegar
            resetAfterMove = true; // Zabezpieczenie przed wielokrotnym wywo�aniem tej samej animacji
        }
    }

    public void HandleClient()
    {
        if (isAnimating) return; // Nie wykonuj akcji, gdy animacja trwa

        animationStartAngle = currentAngle;
        targetAngle = currentAngle + stepAngle;

        animationTime = 0f; // Resetuj czas animacji
        isAnimating = true;

        if(targetAngle == -90f)
        {
            midDay?.Invoke();
            
            breakSystem.BreakTime();
        }

        if (targetAngle >= resetAngle)
        {
            targetAngle = resetAngle; // Ustaw maksymalny k�t na 90 stopni
            canReset = true; // Po osi�gni�ciu 90 stopni umo�liw reset
        }
    }

    private void StartClockMove()
    {
        isMovingClock = true;
        moveTime = 0f; // Resetuj czas ruchu
        fadeTime = 0f; // Resetuj czas animacji pojawiania si� t�a
    }

    private void StartClockReturn()
    {
        isReturningClock = true;
        moveTime = 0f; // Resetuj czas ruchu
        fadeTime = 0f; // Resetuj czas animacji pojawiania si� t�a
    }

    private void MoveClockToCenter()
    {
        moveTime += Time.deltaTime;
        float t = moveTime / moveDuration; // Normalizowany czas (0 do 1)

        if (t >= 1f)
        {
            t = 1f;
            isMovingClock = false;

            // Rozpocznij animacj� resetu po zako�czeniu przesuwania zegara na �rodek
            StartReset();
            Invoke(nameof(StartClockReturn), waitBeforeMove);
        }

        // Interpolacja pozycji i skali z efektem ease-in-out
        clockUI.anchoredPosition = Vector3.Lerp(originalPosition, targetPosition, Mathf.SmoothStep(0f, 1f, t));
        clockUI.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.SmoothStep(0f, 1f, t));

        // Rozpocznij animacj� t�a (pojawianie si� obrazu)
        fadeTime += Time.deltaTime;
    }

    private void ReturnClock()
    {
        moveTime += Time.deltaTime;
        float t = moveTime / moveDuration; // Normalizowany czas (0 do 1)

        if (t >= 1f)
        {
            t = 1f;
            isReturningClock = false;

            // Zresetuj zmienne po zako�czeniu animacji powrotu
            ResetAll();
        }

        // Interpolacja pozycji i skali z efektem ease-in-out
        clockUI.anchoredPosition = Vector3.Lerp(targetPosition, originalPosition, Mathf.SmoothStep(0f, 1f, t));
        clockUI.localScale = Vector3.Lerp(targetScale, originalScale, Mathf.SmoothStep(0f, 1f, t));
    }

    private void FadeBackground()
    {
        float t = fadeTime / fadeDuration;

        if (t >= 1f)
        {
            t = 1f;
        }

        // Lerpowanie warto�ci alfa t�a (od 0 do 1) - pojawianie si� obrazu w tle
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, Mathf.Lerp(0f, 1f, t)); // Zmiana przezroczysto�ci obrazu
        }
    }

    private void StartReset()
    {
        if (isAnimating) return; // Nie wykonuj akcji, gdy animacja trwa

        animationStartAngle = currentAngle;
        targetAngle = 360f + startAngle; // Pe�ny obr�t do pocz�tkowej warto�ci
        animationTime = 0f;
        isAnimating = true;
        canReset = false; // Wy��cz mo�liwo�� resetu po rozpocz�ciu
    }

    private void AnimateClockHand()
    {
        animationTime += Time.deltaTime;
        float t = animationTime / animationDuration; // Normalizowany czas (0 do 1)

        if (t >= 1f)
        {
            t = 1f;
            isAnimating = false;

            // Je�li osi�gn�li�my pe�ny obr�t, zresetuj k�t
            if (targetAngle >= 360f + startAngle)
            {
                currentAngle = startAngle;
                startDay?.Invoke();
            }
            else
            {
                currentAngle = targetAngle;
            }
        }
        else
        {
            // SmoothStep dla efektu ease-in-out
            currentAngle = Mathf.Lerp(animationStartAngle, targetAngle, Mathf.SmoothStep(0f, 1f, t));
        }

        clockHand.eulerAngles = new Vector3(0, 0, -currentAngle);
    }

    // Funkcja do resetowania wszystkiego
    private void ResetAll()
    {
        currentAngle = startAngle;
        fadeTime = 0f;
        resetAfterMove = false;
        isAnimating = false;
        isMovingClock = false;
        isReturningClock = false;
        clockUI.anchoredPosition = originalPosition;
        clockUI.localScale = originalScale;

        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f); // T�o niewidoczne
        }
    }
}
