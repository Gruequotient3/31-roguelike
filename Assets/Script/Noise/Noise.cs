using UnityEngine;

namespace Roguelike.Noise
{
    public enum MapType
    {
        PERLIN_NOISE,
    };

    public static class Noise
    {
        public static float PerlinNoise(Vector2 v)
        {
            return Mathf.PerlinNoise(v.x, v.y);
        }
    };

    public static class MapGenerator
    {
        private static void GeneratePerlinNoise(Vector2Int size, Vector2 offset, ref float[] noisemap)
        {
            for (int y = 0; y < size.y; ++y)
            {
                for (int x = 0; x < size.x; ++x)
                {
                    float dx = offset.x + (float)x / (float)size.x;
                    float dy = offset.y + (float)y / (float)size.y;
                    noisemap[y * size.x + x] = Noise.PerlinNoise(new Vector2(dx, dy));
                }
            }
            
        }

        public static float[] Generate(Vector2Int size, Vector2 offset, MapType type)
        {
           float[] noisemap = new float[size.x * size.y];
           switch (type)
            {
                case MapType.PERLIN_NOISE:
                    GeneratePerlinNoise(size, offset, ref noisemap);
                    break;
                default:
                    break;
            }
            return noisemap; 
        }
    };
}