using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    private static World _instance;
    public static World Instance { get { return _instance; } }



    [SerializeField] private Material material;
    Chunk[,] chunks = new Chunk[Data.WorldSizeInChunks, Data.WorldSizeInChunks];


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }


    private void Start()
    {
        PopulateWorld(Data.seed);
    }

    private void PopulateWorld(int seed)
    {
        for (int x = 0; x < Data.WorldSizeInChunks; x++)
        {
            for (int z = 0; z < Data.WorldSizeInChunks; z++)
            {
                Debug.Log(x + " " + z);
                ChunkCoord newChunk = new ChunkCoord(x, z);
                chunks[x, z] = new Chunk(newChunk, material);
                chunks[x, z].Init();
            }
        }
    }

}
