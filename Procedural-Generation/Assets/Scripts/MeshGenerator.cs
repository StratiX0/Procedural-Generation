using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    [Header("Mesh Settings")]
    public int xSize = 40;
    public int zSize = 40;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Color[] colors;
    public Gradient gradient;
    private float minTerrainHeight;
    private float maxTerrainHeight;
    public float depth = 3f;
    private float lastDepth = 3f;

    [Header("Perlin Settings")]
    public GameObject PerlinNoiseGenerator;
    public Texture2D PerlinTexture;
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public int seed = 0;
    public Vector2 offset = new Vector2(0, 0);
    public int octaves = 4;
    [Range(0f, 1f)]
    public float persistance = 0.5f;
    public float lacunarity = 2f;
    private System.Random rand = new System.Random();

    [Header("Animation Settings")]
    public bool animate = false;
    public float animationSpeed = 2f;

    [SerializeField] GameObject speedValue = null;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        lastDepth = depth;

        seed = rand.Next(999999999);

        ModifyMesh();
    }

    private void Update()
    {
        if (animate)
        {
            ModifyMesh();

            if (animate)
                offset.x += Time.deltaTime * animationSpeed;
        }
    }

    void ModifyMesh()
    {
        PerlinTexture = PerlinNoiseGenerator.GetComponent<PerlinNoiseGenerator>().GenerateTexture(width, height, width / 2f, height / 2f, scale, seed, offset, octaves, persistance, lacunarity, new Vector3((xSize * this.GetComponent<Transform>().localScale.x) / 2, 150, (zSize * this.GetComponent<Transform>().localScale.z) / 2));
        minTerrainHeight = 0f;
        maxTerrainHeight = 0f;
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float textureX = PerlinTexture.width;
                float textureY = PerlinTexture.height;
                float y = PerlinTexture.GetPixel(x, z).grayscale * depth;
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
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
        if (scale < 0)
            scale = 0;

        if (mesh != null && !animate)
        {
            ModifyMesh();
        }
    }


    // UI ----------------------------


    public void SetSizeX(string value)
    {
        xSize = int.Parse(value);
    }

    public void Animate()
    {
        animate = !animate;
    }

    public void SetSpeed(float speed)
    {
        animationSpeed = speed;
        if (speedValue != null) speedValue.GetComponent<TextMeshProUGUI>().text = animationSpeed.ToString("F2");
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null) return;

    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], 0.1f);
    //    }
    //}
}
