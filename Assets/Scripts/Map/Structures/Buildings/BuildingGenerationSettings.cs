using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Instructions for the BuildingGenerator to what kind of building it should generate.
/// </summary>
public class BuildingGenerationSettings
{
    public Vector2Int Origin;
    public Vector2Int BoundsSwCorner;
    public Vector2Int BoundsDimensions;

    public BaseFeatureType WallType;
    public BaseFeatureType FloorType;
    public RoofType RoofType;
    public Color? WallColor;

    public BuildingGenerationSettings(Vector2Int origin,Vector2Int boundsSwCorner, Vector2Int boundsDimensions, BaseFeatureType wallType, BaseFeatureType floorType, RoofType roofType, Color? wallColor = null)
    {
        Origin = origin;
        BoundsSwCorner = boundsSwCorner;
        BoundsDimensions = boundsDimensions;
        WallType = wallType;
        FloorType = floorType;
        RoofType = roofType;
        WallColor = wallColor;
    }
}
