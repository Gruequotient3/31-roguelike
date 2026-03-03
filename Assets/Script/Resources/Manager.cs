using System.Collections.Generic;
using Roguelike.Tilemaps;
using UnityEngine;

namespace Roguelike.Resources{
    public static class ResourcesManager
    {
        public static Dictionary<TileType, Sprite[]> LoadTilesData()
        {
            Dictionary<TileType, Sprite[]> ret = new Dictionary<TileType, Sprite[]>();
            Object[] tilesData = UnityEngine.Resources.LoadAll("ScriptableObject/Tile", typeof(TileSpriteData));
            foreach(TileSpriteData data in tilesData)
            {  
                ret.Add(data.type, data.sprites); 
            }
            return ret;
        }  
    }
}
