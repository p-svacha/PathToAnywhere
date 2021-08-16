using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileSet
{
    public TileSetData Data;

    public TileSet() { }

    public TileSet(TileSetData data)
    {
        Data = data;
    }
    public abstract void PlaceTile(TilemapGenerator generator, Tilemap tilemap, Vector3Int position);
}
