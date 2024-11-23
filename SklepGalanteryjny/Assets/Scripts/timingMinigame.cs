using UnityEngine;
using UnityEngine.UI;

public class TimingMinigame : MonoBehaviour
{
    public RectTransform movingLine;
    public RectTransform targetArea;
    public float speed = 100f;
    private bool movingUp = true;

    void Update()
    {
        MoveLine();
    }

    void MoveLine()
    {
        if (movingLine == null)
            return;

        float move = speed * Time.deltaTime;
        if (movingUp)
        {
            movingLine.anchoredPosition += new Vector2(0, move);
            if (movingLine.anchoredPosition.y >= 200) // Upper limit
                movingUp = false;
        }
        else
        {
            movingLine.anchoredPosition -= new Vector2(0, move);
            if (movingLine.anchoredPosition.y <= -200) // Lower limit
                movingUp = true;
        }
    }

    public void CheckHit()
    {
        if (movingLine == null || targetArea == null)
            return;

        float linePosY = movingLine.anchoredPosition.y;
        float targetMinY = targetArea.anchoredPosition.y - (targetArea.rect.height / 2);
        float targetMaxY = targetArea.anchoredPosition.y + (targetArea.rect.height / 2);

        if (linePosY >= targetMinY && linePosY <= targetMaxY)
        {
            Debug.Log("Hit!");
        }
        else
        {
            Debug.Log("Missed!");
        }
    }
}
