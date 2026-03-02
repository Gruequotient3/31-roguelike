using System;
using System.Collections.Generic;
using UnityEngine;

using Roguelike.Utils;

namespace Roguelike.Tilemaps{
    [Serializable]
    public struct TileData
    {
        public Sprite sprite;
        public bool seleted;
    }

    public class Tile
    {
        public GameObject gameObject = null;

        private Vector3Int _position;
        private TileData _tileData;

        public Tile(Vector3Int position, TileData tileData){
            _position = position;
            _tileData = tileData;
        }

        public void Generate()
        {
            if (gameObject) UnityEngine.Object.Destroy(gameObject);
            gameObject = new GameObject("Tile " + _position.x + " " + _position.y + " " + _position.z);
            SpriteRenderer sp = gameObject.AddComponent<SpriteRenderer>();
            sp.sprite = _tileData.sprite;

            Vector3 pos = Coordinate.IsoToWorld(_position);

            sp.sortingOrder = -(_position.x + _position.y) + _position.z * 5;
            gameObject.transform.position = new Vector3(pos.x, pos.y, 0.0f);
        }

        public Sprite GetSprite(){ return _tileData.sprite; }
        public void SetSprite(Sprite sprite)
        {
            _tileData.sprite = sprite;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

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


