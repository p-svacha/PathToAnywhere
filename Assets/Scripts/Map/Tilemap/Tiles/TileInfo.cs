using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An instance of this class provides info for a single tile.
/// </summary>
public class TileInfo
{
    public Vector2Int Position;

    public BaseSurfaceType BaseSurfaceType;
    public BaseFeatureType BaseFeatureType;

    public Region Region;
    public Building Building;

    public bool Passable;
    public float SpeedModifier;

    public TileInfo(Vector2Int position)
    {
        Position = position;
        Passable = true;
        SpeedModifier = 1f;
    }

    public void SetInfoFromTileSetData(TileSetData data)
    {
        if (!data.Passable) Passable = false;
        if (data.SpeedModifier < SpeedModifier) SpeedModifier = data.SpeedModifier;
    }
}
