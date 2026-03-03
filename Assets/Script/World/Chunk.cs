using System.Collections.Generic;
using UnityEngine;

using Roguelike.Tilemaps;
using Roguelike.Noise;

public class Chunk
{
    public static readonly int k_xSize = 16;
    public static readonly int k_ySize = 16;
    public static readonly int k_zSize = 10;

    public Tilemap tilemap;    
    public Vector3Int position;

    public Chunk(GameObject parent, Vector3Int pos)
    {
        position = pos;
        tilemap = new Tilemap("Tilemap " + pos.x + " " + pos.y);
        tilemap.gameObject.transform.SetParent(parent.transform);
    }

    public void Generate()
    {
        tilemap.tiles.Clear();
        float[] noiseMap = MapGenerator.Generate(new Vector2Int(k_xSize, k_ySize), new Vector2(position.x, position.y), MapType.PERLIN_NOISE);
        for (int y = 0; y < k_ySize; ++y)
        {
            for (int x = 0; x < k_xSize; ++x)
            {
                TileType type;
                int maxz =  Mathf.RoundToInt(noiseMap[y * k_xSize + x] * 5 + 2);
                switch (maxz)
                {
                    case 2:
                        type = TileType.WATER;
                        break;
                    case 3: 
                        type = TileType.SAND;
                        break;
                    case 4:
                        type = TileType.GRASS;
                        break;
                    case 5:
                        type = TileType.ROCK;
                        break;
                    default:
                        type = TileType.DIRT;
                        break;
                }
                for (int z = maxz; z >= 0; --z)
                {
                    Tile tile;
                    Vector3Int tilePosition = new Vector3Int(x + position.x * k_xSize, y + position.y * k_ySize, z);
                    if (z == maxz) tile = Tile.GetTileFromType(type, tilePosition);
                    else tile = Tile.GetTileFromType(TileType.DIRT, tilePosition);
                    tilemap.SetTile(tilePosition, tile);
                }
            }
        }
        tilemap.Generate();
    }

}
