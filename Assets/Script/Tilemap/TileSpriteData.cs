using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Tilemap/TileData")]
public class TileSpriteData : ScriptableObject
{
    public Roguelike.Tilemaps.TileType type;
    public Sprite[] sprites;
}
