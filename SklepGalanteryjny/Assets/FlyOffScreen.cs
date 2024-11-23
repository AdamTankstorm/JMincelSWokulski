using UnityEngine;

public class FlyOffScreen : MonoBehaviour
{
    public Vector2 targetPosition; // Pozycja koñcowa w przestrzeni lokalnej (UI)
    public float moveDuration = 1f; // Czas trwania animacji
    private Vector2 startPosition;
    private float elapsedTime = 0f;
    private bool isAnimating = false;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Pobierz RectTransform obiektu
        startPosition = rectTransform.anchoredPosition; // Pozycja pocz¹tkowa w przestrzeni UI
    }

    void Update()
    {
        if (isAnimating)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // Interpolacja pozycji z efektem ease-in-out
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0f, 1f, t));

            // Zakoñcz animacjê, gdy osi¹gnie cel
            if (t >= 1f)
            {
                isAnimating = false;
                rectTransform.gameObject.SetActive(false);
            }
        }
    }

    public void StartFlyOff()
    {
        isAnimating = true;
        elapsedTime = 0f; // Zresetuj czas
    }
}
