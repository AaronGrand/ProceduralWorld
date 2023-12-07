using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    private static World _instance;
    public static World Instance { get { return _instance; } }

    [SerializeField] private int viewDistanceInChunks = 8;
    [SerializeField] private Transform player;


    [SerializeField] private Material material;
    Chunk[,] chunks = new Chunk[Data.WorldSizeInChunks, Data.WorldSizeInChunks];

    //Chunk Update
    ChunkCoord playerLastChunkCoord;
    ChunkCoord playerChunkCoord;

    List<ChunkCoord> chunksToCreate = new List<ChunkCoord>();

    List<ChunkCoord> activeChunks = new List<ChunkCoord>();


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }


    private void Start()
    {
        //PopulateWorld(Data.seed);

        CheckViewDistance();
    }

    private void Update()
    {
        playerChunkCoord = GetChunkCoordFromVector3(player.position);

        if (!playerChunkCoord.Equals(playerLastChunkCoord))
            CheckViewDistance();

        if (chunksToCreate.Count > 0)
            CreateChunk();
    }

    private void PopulateWorld(int seed)
    {
        for (int x = (Data.WorldSizeInChunks / 2) - viewDistanceInChunks; x < (Data.WorldSizeInChunks / 2) + viewDistanceInChunks; x++)
        {
            for (int z = (Data.WorldSizeInChunks / 2) - viewDistanceInChunks; z < (Data.WorldSizeInChunks / 2) + viewDistanceInChunks; z++)
            {
                ChunkCoord newChunk = new ChunkCoord(x, z);
                chunks[x, z] = new Chunk(newChunk, material);
                chunksToCreate.Add(newChunk);
            }
        }    
    }

    private void CheckViewDistance()
    {

        ChunkCoord coord = GetChunkCoordFromVector3(player.position);

        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);
        activeChunks.Clear();

        foreach (ChunkCoord c in previouslyActiveChunks)
            chunks[c.x, c.z].isActive = false;

        for (int x = coord.x - viewDistanceInChunks; x < coord.x + viewDistanceInChunks; x++)
        {
            for (int z = coord.z - viewDistanceInChunks; z < coord.z + viewDistanceInChunks; z++)
            {

                ChunkCoord thisChunkCoord = new ChunkCoord(x, z);

                if (IsChunkInWorld(thisChunkCoord))
                {
                    if (chunks[x, z] == null)
                    {
                        chunks[x, z] = new Chunk(thisChunkCoord, material);
                        chunksToCreate.Add(thisChunkCoord);
                    }
                    else if (!chunks[x, z].isActive)
                    {
                        chunks[x, z].isActive = true;
                    }
                    activeChunks.Add(thisChunkCoord);
                }

                
                for (int i = 0; i < previouslyActiveChunks.Count; i++)
                {
                    if (previouslyActiveChunks[i].Equals(thisChunkCoord))
                        previouslyActiveChunks.RemoveAt(i);
                }
            }
        }
        

    }

    private void CreateChunk()
    {
        ChunkCoord c = chunksToCreate[0];
        chunksToCreate.RemoveAt(0);
        chunks[c.x, c.z].Init();
    }

    bool IsChunkInWorld(ChunkCoord coord)
    {
        if (coord.x > 0 && coord.x < Data.WorldSizeInChunks - 1 && coord.z > 0 && coord.z < Data.WorldSizeInChunks - 1)
            return true;
        else
            return false;
    }

    private ChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / Data.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / Data.chunkWidth);
        return new ChunkCoord(x, z);
    }

}
