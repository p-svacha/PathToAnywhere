using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An instance of this class provides info for a single tile.
/// </summary>
public class TileInfo
{
    public Vector2Int Position;
    public SurfaceType Type;
    public Region Region;
    public Building Building;
    public bool Passable;
    public float SpeedModifier;

    public TileInfo(Vector2Int position)
    {
        Position = position;
    }

    public void SetInfoFromTileSetData(TileSetData data)
    {
        Passable = data.Passable;
        SpeedModifier = data.SpeedModifier;
    }
}
