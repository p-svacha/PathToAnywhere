using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// An instance of this class provides info for a single tile.
/// </summary>
public class TileInfo
{
    public Vector2Int Position;

    public BaseSurfaceType BaseSurfaceType;
    public BaseFeatureType BaseFeatureType;

    public Color? BaseSurfaceColor;
    public Color? BaseFeautureColor;

    public Character Character;
    public Region Region;
    public Building Building;

    public bool Blocked;

    public TileInfo(Vector2Int position)
    {
        Position = position;
    }

    public bool IsPassable(TilemapGenerator generator)
    {
        if (BaseFeatureType != BaseFeatureType.None && !generator.BaseFeatureTilesets[BaseFeatureType].Data.Passable) return false;
        if (Blocked) return false;
        if (BaseSurfaceType != BaseSurfaceType.None && !generator.BaseSurfaceTilesets[BaseSurfaceType].Data.Passable) return false;
        if (Character != null) return false;
        return true;
    }

    public float GetSpeedModifier(TilemapGenerator generator)
    {
        List<float> modifiers = new List<float>();
        if (BaseSurfaceType != BaseSurfaceType.None) modifiers.Add(generator.BaseSurfaceTilesets[BaseSurfaceType].Data.SpeedModifier);
        if (BaseFeatureType != BaseFeatureType.None) modifiers.Add(generator.BaseFeatureTilesets[BaseFeatureType].Data.SpeedModifier);

        if (modifiers.Count == 0) return 1f;
        else return modifiers.Min();
    }
}
