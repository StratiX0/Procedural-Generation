using Unity.VisualScripting;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{

    public int width = 256;
    public int height = 256;

    public float halfWidth = 0f;
    public float halfHeight = 0f;

    public float scale = 20f;
    System.Random rand = new System.Random();
    public int seed = 0;
    public Vector2 offset = new Vector2(0, 0);
    public int octaves = 4;
    [Range(0f, 1f)]
    public float persistance = 0.5f;
    public float lacunarity = 2f;

    public bool animate = false;
    public float animationSpeed = 5f;

    void Start()
    {
        seed = rand.Next(999999999);
    }

    void Update()
    {
        if (animate)
        {
            offset.x += Time.deltaTime * animationSpeed;
        }

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture(width, height, scale, seed, offset, octaves, persistance, lacunarity);
    }

    public Texture2D GenerateTexture(int width, int height, float scale, int seed, Vector2 offset, int octaves, float persistance, float lacunarity)
    {
        halfWidth = width / 2f;
        halfHeight = height / 2f;

        Texture2D texture = new Texture2D(width, height);

        // Generer une Perlin Noise map pour la texture

        System.Random rand = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = rand.Next(-100000, 100000) + offset.x;
            float offsetY = rand.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
            scale = 0.001f;

        float noiseHeight = 0f;
        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                float amplitude = 1f;
                float frequency = 1f;
                noiseHeight = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (float)(x - halfWidth) /  scale * frequency + octaveOffsets[i].x;
                    float yCoord = (float)(y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float sample = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    noiseHeight += sample * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseHeight = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight);

                Color color = new Color(noiseHeight, noiseHeight, noiseHeight);
                texture.SetPixel(x, y, color);
            }
        }
        
        texture.Apply();

        return texture;
    }

    private void OnValidate()
    {
        if (width < 1)
            width = 1;
        if (height < 1)
            height = 1;
        if (seed < 0)
            seed = 0;
        if (octaves < 1)
            octaves = 1;
        if (lacunarity < 1)
            lacunarity = 1;
    }
}
