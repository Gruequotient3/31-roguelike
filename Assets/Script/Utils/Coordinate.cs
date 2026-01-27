using UnityEngine;

namespace Roguelike.Utils
{
    
    public static class Coordinate
    {
        public static readonly float tileWidth = 1f;
        public static readonly float tileHeight = 0.5f;

        public static readonly float elevationScale = 0.25f;

        public static Vector3Int WorldToIso(Vector3 worldPos)
        {
            int x = Mathf.FloorToInt((worldPos.y / tileHeight) + (worldPos.x / tileWidth) - worldPos.z * elevationScale); 
            int y = Mathf.FloorToInt((worldPos.y / tileHeight) - (worldPos.x / tileWidth) - worldPos.z * elevationScale);
            return new Vector3Int(x, y, (int)worldPos.z);
        }

        public static Vector3Int WorldToChunk(Vector3 worldPos)
        {
            Vector3Int iso = WorldToIso(worldPos);
            return new Vector3Int(Mathf.FloorToInt((float)iso.x / (float)Chunk.k_xSize), 
                                  Mathf.FloorToInt((float)iso.y / (float)Chunk.k_ySize),
                                  0);
        }

        public static Vector3 IsoToWorld(Vector3Int iso)
        {
            float x = (iso.x - iso.y) * (tileWidth * 0.5f);  
            float y = (iso.x + iso.y) * (tileHeight * 0.5f) + iso.z * elevationScale;
            return new Vector3(x, y, iso.z);
        }
    }
}


    
