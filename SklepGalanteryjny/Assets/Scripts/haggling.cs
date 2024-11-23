using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class haggling : MonoBehaviour
{
    public Slider slider;
    public Image gradientImage;
    private Texture2D gradientTexture;
    private Camera camera;
    public int greenStart;
    private int tries = 0, red = 0, green = 0, yellow = 0;

    public event Action win;
    public event Action draw;
    public event Action loss;
    
    void OnEnable()
    {
        CreateGradient();
        camera= FindAnyObjectByType(typeof(Camera)) as Camera;
    }

    private void Update()
    {
        if(tries == 2)
        {
            if(red >= 1)
            {
                loss?.Invoke();
            }
            else if(yellow == 2)
            {
                draw?.Invoke();
            }
            else if(green >= 1)
            {
                win?.Invoke();
            }

            tries = 0;
            red = 0;
            green = 0;
            yellow = 0;
            gameObject.SetActive(false);
        }

        slider.value = Mathf.PingPong(Time.time / Time.fixedDeltaTime, 100);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (slider.value >= greenStart && slider.value < greenStart + 10)
            { 
                tries++;
                red++;
            }
            else if ((slider.value >= greenStart - 20 && slider.value < greenStart) || (slider.value >= greenStart + 10 && slider.value < greenStart + 20 + 10)) // Red bar surrounding green
            {
                tries++;
                green++;
            }
            else
            {
                tries++;
                yellow++;
            }
        }
    }
    void CreateGradient()
    {
        int totalLength = 100;
        int greenLength = 10; // Thin green bar
        int redLength = 20;   // Red bar on both sides

        // Random start position for the green bar, ensuring there's space for red on both sides
        greenStart = UnityEngine.Random.Range(redLength, totalLength - greenLength - redLength);

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
