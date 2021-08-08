using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileSet
{
    public TileSetData Data;
    public TileType Type;

    public TileSet(TileSetData data, TileType type)
    {
        Data = data;
        Type = type;
    }
    public abstract void PlaceTile(TilemapGenerator generator, Tilemap tilemap, Vector3Int position);
}
