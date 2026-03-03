using System;
using System.Collections.Generic;
using UnityEngine;

using Roguelike.Utils;

namespace Roguelike.Tilemaps{
    public class Tilemap
    {
        public GameObject gameObject = null;
        public Dictionary<Vector3Int, Tile> tiles;

        public Tilemap(string name = "Tilemap")
        {
            gameObject = new GameObject(name);
            tiles = new Dictionary<Vector3Int, Tile>();
        }


        public Tile GetTile(Vector3Int position)
        {
            if (!tiles.ContainsKey(position)) return null;
            return tiles[position];
        }

        public void SetTile(Vector3Int position, TileData tileData)
        {
            tiles[position] = new Tile(position, tileData);
        }

        public void Generate()
        {
            string name = "Tilemap";
            if (gameObject) {
                name = gameObject.name;
                UnityEngine.Object.Destroy(gameObject);
            }
            gameObject = new GameObject(name);
            foreach(var (_, tile) in tiles)
            {
                tile.Generate();
                tile.gameObject.transform.SetParent(gameObject.transform);
            }            
        }
    }
}


