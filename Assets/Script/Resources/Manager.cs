using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Resources{
    public static class ResourcesManager
    {
        public static Dictionary<string, Sprite[]> LoadTilesData()
        {
            Dictionary<string, Sprite[]> ret = new Dictionary<string, Sprite[]>();
            Object[] tilesData = UnityEngine.Resources.LoadAll("ScriptableObject/Tile", typeof(TileSpriteData));
            foreach(TileSpriteData data in tilesData)
            {  
                ret.Add(data.id, data.sprites); 
            }
            return ret;
        }  
    }
}
