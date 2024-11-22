using UnityEngine;

public class ClockController : MonoBehaviour
{
    public Transform clockHand; // Wskazówka zegara
    private float currentAngle = -90f; // Startowa pozycja (270 stopni)
    private const float stepAngle = 22.5f; // Przesuniêcie o 22,5 stopnia na klienta
    private const float resetAngle = 90f; // K¹t resetu (90 stopni)
    private const float startAngle = -90f; // Pocz¹tkowy k¹t po resecie
    public float animationDuration = 0.5f; // Czas trwania animacji w sekundach

    private bool isAnimating = false;
    private bool canReset = false; // Flaga do kontrolowania resetu
    private float targetAngle;
    private float animationStartAngle;
    private float animationTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canReset)
            {
                StartReset();
            }
            else
            {
                HandleClient();
            }
        }

        if (isAnimating)
        {
            AnimateClockHand();
        }
    }

    public void HandleClient()
    {
        if (isAnimating) return; // Nie wykonuj akcji, gdy animacja trwa

        animationStartAngle = currentAngle;
        targetAngle = currentAngle + stepAngle;

        if (targetAngle >= resetAngle)
        {
            targetAngle = resetAngle; // Ustaw maksymalny k¹t na 90 stopni
            canReset = true; // Po osi¹gniêciu 90 stopni umo¿liw reset
        }

        animationTime = 0f; // Resetuj czas animacji
        isAnimating = true;
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
}
