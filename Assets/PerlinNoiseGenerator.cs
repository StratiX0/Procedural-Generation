using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{

    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public bool animate = false;
    public float animationSpeed = 5f;

    void Start()
    {
        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = GenerateTexture();

        offsetX = Random.Range(0f, 999999f);
        offsetY = Random.Range(0f, 999999f);
    }

    void Update()
    {
        if (animate)
        {
            offsetX += Time.deltaTime * animationSpeed;
        }

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        // Generer une Perlin Noise map pour la texture

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
