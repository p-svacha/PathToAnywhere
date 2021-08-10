using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileSet
{
    public TileSetData Data;
    public SurfaceType Type;

    public TileSet() { }

    public TileSet(TileSetData data, SurfaceType type)
    {
        Data = data;
        Type = type;
    }
    public abstract void PlaceTile(TilemapGenerator generator, Tilemap tilemap, Vector3Int position);
}
