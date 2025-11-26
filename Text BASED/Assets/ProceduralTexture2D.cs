using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ProceduralTexture2D : MonoBehaviour
{
     public int width = 256;
    public int height = 256;
    public float scale = 10f;

    private Texture2D texture;
    private Color colorA;
    private Color colorB;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        GenerateTexture();
    }

    // 👇 This is the function your button will call
    public void GenerateTexture()
    {
        // Pick two new random colors every time
        colorA = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
        colorB = Random.ColorHSV(0f, 1f, 0.4f, 0.8f, 0.4f, 0.8f);

        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = Color.Lerp(colorA, colorB, sample);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.one * 0.5f);
        sr.sprite = sprite;
    }
}