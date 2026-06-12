using System.Collections.Generic;
using UnityEngine;

using Roguelike.Utils;
using Roguelike.Tilemap.NProp;

public enum WorldType
{
    HUB,
    WORLD,
};

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator singleton = null;

    private Dictionary<Vector3Int, Chunk> _chunks;

    public int renderDistance;
    public Transform center;
    public BiomeCollection biomeCollection;
    public WorldType type;

    void Awake()
    {
        singleton = this;
        _chunks = new Dictionary<Vector3Int, Chunk>();
    }

    void Start()
    {
        switch (type)
        {
            case WorldType.HUB:
                BiomeValueGenerator.singleton.SetSeed(GameManager.singleton.hubSeed);
                InitializeHub();
                break;
            case WorldType.WORLD:
                BiomeValueGenerator.singleton.SetSeed(GameManager.singleton.worldSeed);
                InitializeWorld();
                break;

        }
    }

    void Update()
    {
        if (type == WorldType.WORLD) UpdateChunk();
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
                Vector3Int[] offsets =
                {
                    new Vector3Int( 1,  0,  0) + chunkPos,
                    new Vector3Int(-1,  0,  0) + chunkPos,
                    new Vector3Int( 0,  1,  0) + chunkPos,
                    new Vector3Int( 0, -1,  0) + chunkPos,

                };
                if (GetChunk(chunkPos) != null) continue;
                Chunk newChunk = new Chunk(gameObject, chunkPos);
                newChunk.InitializeWorldData(biomeCollection, GameManager.singleton.worldSeed);
                newChunk.Generate();
                for (int k = 0; k < 4; ++k)
                {
                    if (GetChunk(offsets[k]) == null) continue;
                    Chunk neighbord = GetChunk(offsets[k]);
                    neighbord.UpdateBorderedTile(newChunk.tilemap, newChunk.position);
                    newChunk.UpdateBorderedTile(neighbord.tilemap, neighbord.position);
                }
                _chunks.Add(chunkPos, newChunk);
            }
        }
        // Update current chunk
        // Remove non visible chunk
        List<Vector3Int> toRemove = new List<Vector3Int>();
        foreach(var (chunkPos, _) in _chunks)
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
            UnityEngine.Object.Destroy(_chunks[chunkPos].tilemap.gameObject);
            _chunks.Remove(chunkPos);
        }
    }

    public void InitializeWorld()
    {
        Vector3Int pos = Coordinate.WorldToChunk(center.position);
        // Generate new Chunk
        for (int i = -renderDistance; i < renderDistance+1; ++i)
        {
            for (int j = -renderDistance; j < renderDistance+1; ++j)
            {
                Vector3Int chunkPos = new Vector3Int(i + pos.x, j + pos.y, 0);
                Vector3Int[] offsets =
                {
                    new Vector3Int( 1,  0,  0) + chunkPos,
                    new Vector3Int(-1,  0,  0) + chunkPos,
                    new Vector3Int( 0,  1,  0) + chunkPos,
                    new Vector3Int( 0, -1,  0) + chunkPos,

                };
                if (GetChunk(chunkPos) != null) continue;
                Chunk newChunk = new Chunk(gameObject, chunkPos);
                newChunk.InitializeWorldData(biomeCollection, GameManager.singleton.worldSeed);
                newChunk.Generate();
                for (int k = 0; k < 4; ++k)
                {
                    if (GetChunk(offsets[k]) == null) continue;
                    Chunk neighbord = GetChunk(offsets[k]);
                    neighbord.UpdateBorderedTile(newChunk.tilemap, newChunk.position);
                    newChunk.UpdateBorderedTile(neighbord.tilemap, neighbord.position);
                }
                _chunks.Add(chunkPos, newChunk);
            }
        } 
    }

    public void InitializeHub()
    {
        for (int i = -1; i <= 1; ++i)
        {
            for (int j = -1; j <= 1; ++j)
            {
                Vector3Int chunkPos = new Vector3Int(i, j, 0);
                Vector3Int[] offsets =
                {
                    new Vector3Int( 1,  0,  0) + chunkPos,
                    new Vector3Int(-1,  0,  0) + chunkPos,
                    new Vector3Int( 0,  1,  0) + chunkPos,
                    new Vector3Int( 0, -1,  0) + chunkPos,

                };
                Chunk newChunk = new Chunk(gameObject, chunkPos);
                newChunk.InitializeWorldData(biomeCollection, GameManager.singleton.hubSeed);
                newChunk.Generate();

                for (int k = 0; k < 4; ++k)
                {
                    if (GetChunk(offsets[k]) == null) continue;
                    Chunk neighbord = GetChunk(offsets[k]);
                    neighbord.UpdateBorderedTile(newChunk.tilemap, newChunk.position);
                    newChunk.UpdateBorderedTile(neighbord.tilemap, neighbord.position);
                }
                _chunks.Add(chunkPos, newChunk);
            }
        }
        Chunk chunk =  _chunks[new Vector3Int(0, 0, 0)]; 
        Vector3Int altarPos = new Vector3Int(Chunk.k_xSize / 2, Chunk.k_ySize / 2, 0);
        for (int i = 1; i < Chunk.k_zSize; ++i)
        {
            altarPos.z = i;
            if (!chunk.tilemap.ContainObject(altarPos) || chunk.tilemap.GetProp(altarPos) != null)
            {
                chunk.tilemap.SetProp(altarPos, Prop.GetPropFromType(PropType.ALTAR, altarPos));
                break;
            }
        }
    }

    public Chunk GetChunk(Vector3Int position)
    {
        if (!_chunks.ContainsKey(position)) return null;
        return _chunks[position];
    }

}
