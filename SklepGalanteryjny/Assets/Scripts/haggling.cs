using UnityEngine;
using UnityEngine.UI;
public class haggling : MonoBehaviour
{
    public Slider slider;
    public Image gradientImage;
    private Texture2D gradientTexture;
    private Camera camera;
    public int greenStart;
    
    void OnEnable()
    {
        CreateGradient();
        camera= FindAnyObjectByType(typeof(Camera)) as Camera;
    }

    private void Update()
    {
        slider.value = Mathf.PingPong(Time.time / Time.fixedDeltaTime, 100);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (slider.value >= greenStart && slider.value < greenStart + 10) { 

            }
            else if ((slider.value >= greenStart - 20 && slider.value < greenStart) || (slider.value >= greenStart + 10 && slider.value < greenStart + 20 + 10)) // Red bar surrounding green
            {

            }
            else
            {

            }
        }
    }
    void CreateGradient()
    {
        int totalLength = 100;
        int greenLength = 10; // Thin green bar
        int redLength = 20;   // Red bar on both sides

        // Random start position for the green bar, ensuring there's space for red on both sides
        greenStart = Random.Range(redLength, totalLength - greenLength - redLength);

        gradientTexture = new Texture2D(totalLength, 1);
        gradientTexture.wrapMode = TextureWrapMode.Clamp;

        Color[] colors = new Color[totalLength];

        for (int i = 0; i < totalLength; i++)
        {
            if (i >= greenStart && i < greenStart + greenLength) // Green bar
            {
                colors[i] = Color.green;
            }
            else if ((i >= greenStart - redLength && i < greenStart) || (i >= greenStart + greenLength && i < greenStart + greenLength + redLength)) // Red bar surrounding green
            {
                colors[i] = Color.red;
            }
            else // Rest filled with yellow
            {
                colors[i] = Color.yellow;
            }
        }

        gradientTexture.SetPixels(colors);
        gradientTexture.Apply();

        gradientImage.sprite = Sprite.Create(gradientTexture, new Rect(0, 0, gradientTexture.width, gradientTexture.height), new Vector2(0.5f, 0.5f));
    }
}
