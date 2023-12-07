using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    public ChunkCoord coords;
    Material material;
    Mesh mesh;


    public GameObject chunkObject;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    public Vector3 position;

    private bool _isActive;


    public Chunk(ChunkCoord chunkCoords, Material _material)
    {
        coords = chunkCoords;
        material = _material;
    }

    public void Init()
    {
        chunkObject = new GameObject();
        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        chunkObject.transform.SetParent(World.Instance.transform);
        chunkObject.transform.position = new Vector3(coords.x * Data.chunkWidth, 0f, coords.z * Data.chunkWidth);
        chunkObject.name = "Chunk " + coords.x + ", " + coords.z;
        position = chunkObject.transform.position;

        this.isActive = true;

        GenerateChunk();
    }

    public void GenerateChunk()
    {
        mesh = new Mesh();

        Vector3[] vertices = new Vector3[(Data.chunkWidth + 1) * (Data.chunkWidth + 1)];

        for (int i = 0, z = 0; z <= Data.chunkWidth; z++)
        {
            for (int x = 0; x <= Data.chunkWidth; x++)
            {
                float y = Noise.Get2DPerlin(0, new Vector2(x + coords.x * Data.chunkWidth, z + coords.z * Data.chunkWidth), 0, Data.scale) * Data.heightMultiplier;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        int[] triangles = new int[Data.chunkWidth * Data.chunkWidth * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < Data.chunkWidth; z++)
        {
            for (int x = 0; x < Data.chunkWidth; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + Data.chunkWidth + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + Data.chunkWidth + 1;
                triangles[tris + 5] = vert + Data.chunkWidth + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    public bool isActive
    {
        get { return _isActive; }
        set
        {

            _isActive = value;
            if (chunkObject != null)
                chunkObject.SetActive(value);
        }
    }
}

[System.Serializable]
public class ChunkCoord
{
    public int x;
    public int z;

    public ChunkCoord(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}