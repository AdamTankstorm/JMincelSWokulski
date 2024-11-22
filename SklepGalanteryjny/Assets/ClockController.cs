using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{

    public BreakSystem breakSystem;

    public Transform clockHand; // Wskazówka zegara
    public RectTransform clockUI; // RectTransform zegara (dla animacji pozycji i skalowania)
    public Image backgroundImage; // Obiekt UI Image dla t³a
    public Sprite targetBackgroundSprite; // Obrazek t³a (Source Image)
    public Vector3 targetPosition = Vector3.zero; // Pozycja œrodka ekranu
    public Vector3 targetScale = Vector3.one * 3f; // Docelowa skala (powiêkszenie)
    public float moveDuration = 1f; // Czas trwania animacji przesuwania i skalowania
    public float waitBeforeMove = 2f; // Czas oczekiwania przed przesuniêciem
    public float fadeDuration = 1f; // Czas trwania animacji "pojawniania siê" t³a

    private float currentAngle = -90f; // Startowa pozycja (270 stopni)
    private const float stepAngle = 22.5f; // Przesuniêcie o 22,5 stopnia na klienta
    private const float resetAngle = 90f; // K¹t resetu (90 stopni)
    private const float startAngle = -90f; // Pocz¹tkowy k¹t po resecie
    public float animationDuration = 0.5f; // Czas trwania animacji wskazówki

    private bool isAnimating = false;
    private bool canReset = false; // Flaga do kontrolowania resetu
    private bool isMovingClock = false; // Flaga przesuwania zegara
    private bool isReturningClock = false; // Flaga przesuwania zegara
    private bool resetAfterMove = false; // Flaga do uruchomienia resetu po zakoñczeniu animacji ruchu

    private float targetAngle;
    private float animationStartAngle;
    private float animationTime;

    private Vector3 originalPosition; // Pocz¹tkowa pozycja zegara
    private Vector3 originalScale; // Pocz¹tkowa skala zegara
    private float moveTime;
    private float fadeTime = 0f; // Czas trwania animacji pojawiania siê t³a

    void Start()
    {
        // Zapisz oryginaln¹ pozycjê i skalê zegara
        originalPosition = clockUI.anchoredPosition;
        originalScale = clockUI.localScale;

        // Ustaw pocz¹tkow¹ przezroczystoœæ t³a na pe³n¹ przezroczystoœæ (przezroczyste)
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f); // T³o jest pocz¹tkowo niewidoczne
        }

        // Ustaw pocz¹tkowy obrazek t³a
        if (backgroundImage != null && targetBackgroundSprite != null)
        {
            backgroundImage.sprite = targetBackgroundSprite; // Ustaw t³o na obrazek
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleClient();
        }

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

        // Jeœli k¹t osi¹gnie 90 stopni, rozpocznij animacjê przesuwania
        if (currentAngle >= resetAngle && !isMovingClock && !resetAfterMove)
        {
            Invoke(nameof(StartClockMove), waitBeforeMove); // Czekaj 2 sekundy, zanim zaczniesz przesuwaæ zegar
            resetAfterMove = true; // Zabezpieczenie przed wielokrotnym wywo³aniem tej samej animacji
        }
    }

    public void HandleClient()
    {
        if (isAnimating) return; // Nie wykonuj akcji, gdy animacja trwa

        animationStartAngle = currentAngle;
        targetAngle = currentAngle + stepAngle;

        animationTime = 0f; // Resetuj czas animacji
        isAnimating = true;

        if(targetAngle == 0f)
        {
            breakSystem.BreakTime();
        }

        if (targetAngle >= resetAngle)
        {
            targetAngle = resetAngle; // Ustaw maksymalny k¹t na 90 stopni
            canReset = true; // Po osi¹gniêciu 90 stopni umo¿liw reset
        }
    }

    private void StartClockMove()
    {
        isMovingClock = true;
        moveTime = 0f; // Resetuj czas ruchu
        fadeTime = 0f; // Resetuj czas animacji pojawiania siê t³a
    }

    private void StartClockReturn()
    {
        isReturningClock = true;
        moveTime = 0f; // Resetuj czas ruchu
        fadeTime = 0f; // Resetuj czas animacji pojawiania siê t³a
    }

    private void MoveClockToCenter()
    {
        moveTime += Time.deltaTime;
        float t = moveTime / moveDuration; // Normalizowany czas (0 do 1)

        if (t >= 1f)
        {
            t = 1f;
            isMovingClock = false;

            // Rozpocznij animacjê resetu po zakoñczeniu przesuwania zegara na œrodek
            StartReset();
            Invoke(nameof(StartClockReturn), waitBeforeMove);
        }

        // Interpolacja pozycji i skali z efektem ease-in-out
        clockUI.anchoredPosition = Vector3.Lerp(originalPosition, targetPosition, Mathf.SmoothStep(0f, 1f, t));
        clockUI.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.SmoothStep(0f, 1f, t));

        // Rozpocznij animacjê t³a (pojawianie siê obrazu)
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

            // Zresetuj zmienne po zakoñczeniu animacji powrotu
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

        // Lerpowanie wartoœci alfa t³a (od 0 do 1) - pojawianie siê obrazu w tle
        if (backgroundImage != null)
        {
            Color bgColor = backgroundImage.color;
            backgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, Mathf.Lerp(0f, 1f, t)); // Zmiana przezroczystoœci obrazu
        }
    }

    private void StartReset()
    {
        if (isAnimating) return; // Nie wykonuj akcji, gdy animacja trwa

        animationStartAngle = currentAngle;
        targetAngle = 360f + startAngle; // Pe³ny obrót do pocz¹tkowej wartoœci
        animationTime = 0f;
        isAnimating = true;
        canReset = false; // Wy³¹cz mo¿liwoœæ resetu po rozpoczêciu
    }

    private void AnimateClockHand()
    {
        animationTime += Time.deltaTime;
        float t = animationTime / animationDuration; // Normalizowany czas (0 do 1)

        if (t >= 1f)
        {
            t = 1f;
            isAnimating = false;

            // Jeœli osi¹gnêliœmy pe³ny obrót, zresetuj k¹t
            if (targetAngle >= 360f + startAngle)
            {
                currentAngle = startAngle;
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
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f); // T³o niewidoczne
        }
    }
}
