using System.Collections.Generic;
using UnityEngine;

using Roguelike.Resources;

public class WorldGenerator : MonoBehaviour
{
    private Dictionary<string, Sprite[]> _tileSprites;
    public Dictionary<Vector3Int, Chunk> chunks;

    void Awake()
    {
        _tileSprites = ResourcesManager.LoadTilesData();
        chunks = new Dictionary<Vector3Int, Chunk>();
        for (int  i = -2; i < 3; ++i)
        {
            for (int j = -2; j < 3; ++j)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                Chunk newChunk = new Chunk(gameObject, pos);
                newChunk.Generate(_tileSprites);
                chunks.Add(pos, newChunk);
            }
        }
    }
}
