using UnityEngine;

public class ClockController : MonoBehaviour
{
    public Transform clockHand; // Wskaz�wka zegara
    private float currentAngle = -90f; // Startowa pozycja (270 stopni)
    private const float stepAngle = 22.5f; // Przesuni�cie o 22,5 stopnia na klienta
    private const float resetAngle = 90f; // K�t resetu (90 stopni)
    private const float startAngle = -90f; // Pocz�tkowy k�t po resecie
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
            targetAngle = resetAngle; // Ustaw maksymalny k�t na 90 stopni
            canReset = true; // Po osi�gni�ciu 90 stopni umo�liw reset
        }

        animationTime = 0f; // Resetuj czas animacji
        isAnimating = true;
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
