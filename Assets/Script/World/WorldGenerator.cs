using System.Collections.Generic;
using UnityEngine;

using Roguelike.Utils;

public class WorldGenerator : MonoBehaviour
{
    public Dictionary<Vector3Int, Chunk> chunks;

    public int renderDistance;
    public Transform center;

    void Awake()
    {
        chunks = new Dictionary<Vector3Int, Chunk>();
    }


    void Update()
    {
        UpdateChunk();
    }

    public void UpdateChunk()
    {
        Vector3Int pos = Coordinate.WorldToChunk(center.position);
        // Generate new Chunk
        for (int i = -renderDistance; i < renderDistance+1; ++i)
        {
            for (int j = -renderDistance; j < renderDistance+1; ++j)
            {
                Vector3Int chunkPos = new Vector3Int(i + pos.x, j + pos.y, 0);
                if (chunks.ContainsKey(chunkPos)) continue;
                Chunk newChunk = new Chunk(gameObject, chunkPos);
                newChunk.Generate();
                chunks.Add(chunkPos, newChunk);
            }
        }
        // Update current chunk
        // Remove non visible chunk
        List<Vector3Int> toRemove = new List<Vector3Int>();
        foreach(var (chunkPos, _) in chunks)
        {
            Vector3Int diff = pos - chunkPos;
            if (diff.x > renderDistance || diff.x < -renderDistance 
                || diff.y > renderDistance || diff.y < -renderDistance)
            {
                toRemove.Add(chunkPos);
            }
        }

        foreach(var chunkPos in toRemove)
        {
            UnityEngine.Object.Destroy(chunks[chunkPos].tilemap.gameObject);
            chunks.Remove(chunkPos);
        }
    }


}
