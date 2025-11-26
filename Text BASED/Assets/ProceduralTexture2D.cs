using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class ProceduralTexture2D : MonoBehaviour
{
   
    // Texture set up
    public int width = 256;
    public int height = 256;
    public float scale = 10f;

    //UI Sliders
    public Slider sandSlider;
    public Slider grassSlider;
    public Slider rockSlider;

    //Terrain Colors
    public Color waterColor = new Color(0.1f, 0.2f, 0.8f, 1f); 
    public Color sandColor = new Color(0.9f, 0.9f, 0.6f, 1f);  
    public Color grassColor = new Color(0.2f, 0.6f, 0.1f, 1f); 
    public Color rockColor = new Color(0.5f, 0.5f, 0.5f, 1f);  

    private Texture2D texture;
    private SpriteRenderer sr;
    
   
    private float offsetX;
    private float offsetY;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        // Generate the initial island shape and texture
        NewIsland(); 

      
        if (sandSlider != null)
            sandSlider.onValueChanged.AddListener(delegate { GenerateTexture(); });
        if (grassSlider != null)
            grassSlider.onValueChanged.AddListener(delegate { GenerateTexture(); });
        if (rockSlider != null)
            rockSlider.onValueChanged.AddListener(delegate { GenerateTexture(); });
    }

    //This function is called to create a new button
    public void NewIsland()
    {
        // 1. Generate new random offsets for a different spot on the noise map
        offsetX = Random.Range(0f, 100f);
        offsetY = Random.Range(0f, 100f);

        // 2. Call the main generation function to draw the new map
        GenerateTexture();
    }

    //Function is called by the sliders to update terrain levels
    public void GenerateTexture()
    {
        // 1. Read the current threshold values from the Sliders
        float sandLevel = sandSlider != null ? sandSlider.value : 0.4f;
        float grassLevel = grassSlider != null ? grassSlider.value : 0.6f;
        float rockLevel = rockSlider != null ? rockSlider.value : 0.75f;
        
       
        if (texture == null)
        {
            texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
               
                float xCoord = (float)x / width * scale + offsetX;
                float yCoord = (float)y / height * scale + offsetY;
                
                // Perlin noise sample
                float heightSample = Mathf.PerlinNoise(xCoord, yCoord);
                
                //Apply Falloff 
                float dist_x = (float)x / width - 0.5f;
                float dist_y = (float)y / height - 0.5f;
                float dist_sq = dist_x * dist_x + dist_y * dist_y;
                float falloff = Mathf.Pow(dist_sq * 4f, 2.5f); 
                heightSample = Mathf.Max(0f, heightSample - falloff);
                
                //
                Color pixelColor = GetTerrainColor(heightSample, sandLevel, grassLevel, rockLevel);
                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.one * 0.5f, 100, 0, SpriteMeshType.FullRect);
        sr.sprite = sprite;
    }

    //Function to determine color based on height sample and dynamic levels
    private Color GetTerrainColor(float height, float sandLevel, float grassLevel, float rockLevel)
    {
        
        if (height < sandLevel)
        {
            return waterColor; 
        }
        else if (height < grassLevel)
        {
            return sandColor;
        }
        else if (height < rockLevel)
        {
            return grassColor;
        }
        else
        {
            return rockColor;
        }
    }
}