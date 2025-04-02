using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour
{
    private int vertexNumber; //Number of vertices of the terrain
    private int indexNumber; //Number of indices of the terrain
    private int triangleNumber; //Number of triangles of the mesh
    private int uvNumber; //Number of uvs (identical to the number of vertices)
    private List<Vector3> vertices;
    private int[] indices;
    private int[,] triangles;
    private Vector2[] uvs;
    public int terrainx = 250;// Number of vertices along the x axis
    public int terrainz = 250;// Number of vertices along the z axis

    private float[,] heightMap;
    public int seed = 0;

    public Color[] regionColors = { Color.blue, Color.gray, Color.green };

    [SerializeField] public static Mesh mesh;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;

    [SerializeField] public GameObject treePrefab;
    [SerializeField] private GameObject scrapMetalPrefab;

    [SerializeField] private Texture2D waterTexture;
    [SerializeField] private Texture2D groundTexture;
    [SerializeField] private Texture2D mountainTexture;

    public Texture2D[] regionTextures;

    private void Start()
    {
        vertexNumber = terrainx * terrainz; 
        triangleNumber = (terrainx - 1) * (terrainz - 1) * 2;
        indexNumber = triangleNumber * 3;
        uvNumber = vertexNumber;
        
        vertices = new List<Vector3>();
        indices = new int[indexNumber];
        triangles = new int[3,triangleNumber];
        uvs = new Vector2[uvNumber];

        meshFilter = this.GetComponent<MeshFilter>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        meshCollider = this.GetComponent<MeshCollider>();

        regionTextures = new Texture2D[3];
        regionTextures[0] = waterTexture;
        regionTextures[1] = groundTexture;
        regionTextures[2] = mountainTexture;

        System.Random r = new System.Random();
        seed = r.Next(0, 100000);

        heightMap = NoiseHeightMapGenerator.GenerateHeightMap(terrainx+1, terrainz+1, 120, seed); //create height map from which the shape of the terrain will be derived 

        this.gameObject.layer = LayerMask.NameToLayer("Environment");

        generateTerrain();
        createMesh();
        generateTrees(200);
        ResourcesManager.instance.generateResources(60, findMinHeightValues());
        GUIManager.instance.initializeManager();
    }

    private void generateTerrain()
    {
        mesh = new Mesh();

        for(int z = 0; z < terrainz; z++)
        {
            for(int x = 0; x < terrainx; x++)
            {
                vertices.Add(new Vector3(x - terrainx/2, heightMap[z, x], z - terrainz/2));
                uvs[z*terrainz + x] = new Vector2(x / (float)terrainx, z / (float)terrainz);
            }
        }


        int currentVert = 0;
        int currentTri = 0;
        
        for (int z = 0; z < terrainz - 1; z++)
        {
            for (int x = 0; x < terrainx - 1; x++)
            {
                //for every 4 vertices there are 2 triangles
                indices[currentTri] = currentVert;
                indices[currentTri + 1] = currentVert + terrainx;
                indices[currentTri + 2] = currentVert + 1;
                indices[currentTri + 3] = currentVert + 1;
                indices[currentTri + 4] = currentVert + terrainx;
                indices[currentTri + 5] = currentVert + terrainx + 1;

                currentTri += 6; // proceed to the next 2 triangles (6 indices)
                currentVert++;
            } 
            
            currentVert++;
        }

    }

    private void createMesh()
    {


        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.sharedMaterial = (Material)Resources.Load("TerrainMaterial");
        meshRenderer.sharedMaterial.mainTexture = MapTextureGenerator.GenerateTextureFromHeightMap(heightMap, terrainx, terrainz, regionColors, regionTextures);
        

        Debug.Log(mesh.vertices);
    }


    private void generateTrees(int numOfTrees)
    {
        List<Vector3> possiblePositions = findMaxHeightValues();
        System.Random random = new System.Random();

        for(int i = 0; i < numOfTrees; i++)
        {
            int randIndex = random.Next(0, possiblePositions.Count - 1);
            Instantiate(treePrefab, possiblePositions[randIndex], Quaternion.Euler(0f, ((float)random.Next(0, 360)), 0f));
        }

    }


    /*private void generateResources(int numOfResources)
    {
        List<Vector3> possiblePositions = findMinHeightValues();
        System.Random random = new System.Random();

        for (int i = 0; i < numOfResources; i++)
        {
            int randIndex = random.Next(0, possiblePositions.Count - 1);
            Instantiate(scrapMetalPrefab, possiblePositions[randIndex], Quaternion.identity);
        }

    }*/

    private List<Vector3> findMaxHeightValues()
    {
        List<Vector3> maxValues = new List<Vector3>();

        foreach(Vector3 vec in vertices)
        {
            if(vec.y > 5f) //On elevated terrain which must be over 5
                maxValues.Add(vec);
        }

        return maxValues;
    }


    private List<Vector3> findMinHeightValues()
    {
        List<Vector3> minValues = new List<Vector3>();

        foreach (Vector3 vec in vertices)
        {
            if (vec.y < 0.065f) //On low terrain which must be udner 0.065
                minValues.Add(vec);
        }

        return minValues;
    }


    /* void CalcNoise()
     {
         // For each pixel in the texture...
         float y = 0.0F;

         while (y < noiseTex.height)
         {
             float x = 0.0F;
             while (x < noiseTex.width)
             {
                 float xCoord = xOrg + x / noiseTex.width * scale;
                 float yCoord = yOrg + y / noiseTex.height * scale;
                 float sample = Mathf.PerlinNoise(xCoord, yCoord);
                 pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                 x++;
             }
             y++;
         }

         // Copy the pixel data to the texture and load it into the GPU.
         noiseTex.SetPixels(pix);
         noiseTex.Apply();
     }*/
}
