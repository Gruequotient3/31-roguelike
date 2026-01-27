using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Tilemap/TileData")]
public class TileSpriteData : ScriptableObject
{
    public string name;
    public Sprite[] sprites;
}
