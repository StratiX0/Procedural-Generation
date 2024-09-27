using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    Texture2D texture;
    Renderer rendererComponent;

    void Start()
    {
        rendererComponent = GetComponent<Renderer>();
    }

    void Update()
    {
        rendererComponent.material.mainTexture = texture;
    }

    public Texture2D GenerateTexture(int width, int height, float halfWidth, float halfHeight, float scale, int seed, Vector2 offset, int octaves, float persistance, float lacunarity)
    {
        texture = new Texture2D(width, height);

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
                    float xCoord = (float)(x - halfWidth) / scale * frequency + octaveOffsets[i].x;
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
}
