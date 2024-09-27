using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Color[] colors;

    public int xSize = 40;
    public int zSize = 40;

    public float depth = 3f;
    private float lastDepth = 3f;

    public float animationSpeed = 2f;
    public bool animate = false;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    public GameObject PerlinNoiseGenerator;
    public Texture2D PerlinTexture;


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

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        PerlinTexture = PerlinNoiseGenerator.GetComponent<PerlinNoiseGenerator>().GenerateTexture(width, height, scale, seed, offset, octaves, persistance, lacunarity);

        lastDepth = depth;

        CreateShape();
        UpdateMesh();
    }

    private void Update()
    {
        if (animate || lastDepth != depth)
        {
            ModifyMesh();

            if (lastDepth != depth)
                lastDepth = depth;

            if (animate)
                offset.x += Time.deltaTime * animationSpeed;
        }
    }

    void ModifyMesh()
    {
        PerlinTexture = PerlinNoiseGenerator.GetComponent<PerlinNoiseGenerator>().GenerateTexture(width, height, scale, seed, offset, octaves, persistance, lacunarity);
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
                //float y = CalculateHeight(x, z);
                vertices[i] = new Vector3(x, y, z);

                if (depth == lastDepth)
                {
                    if (y > maxTerrainHeight)
                        maxTerrainHeight = y;
                    if (y < minTerrainHeight)
                        minTerrainHeight = y;
                }

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

        //// Si image
        //uvs = new Vector2[vertices.Length];

        //for (int i = 0, z = 0; z <= zSize; z++)
        //{
        //    for (int x = 0; x <= xSize; x++)
        //    {
        //        uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
        //        i++;
        //    }
        //}

    }

    //float CalculateHeight(int x, int y)
    //{
    //    float xCoord = (float)x / xSize * scale + offsetX;
    //    float yCoord = (float)y / zSize * scale + offsetY;

    //    return Mathf.PerlinNoise(xCoord, yCoord) * depth;
    //}

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        //// Si image
        //mesh.uv = uvs;

        mesh.RecalculateNormals();
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
