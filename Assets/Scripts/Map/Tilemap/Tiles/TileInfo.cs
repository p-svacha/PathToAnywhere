using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An instance of this class provides info for a single tile.
/// </summary>
public class TileInfo
{
    public TileType Type;
    public Region Region;
    public Building Building;
    public bool Passable;
    public float SpeedModifier;

    public void SetInfoFromTileSetData(TileSetData data)
    {
        Passable = data.Passable;
        SpeedModifier = data.SpeedModifier;
    }
}
