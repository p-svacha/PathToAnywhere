using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerationSettings
{
    public int MinWidth;
    public int MaxWidth;
    public int MinHeight;
    public int MaxHeight;

    public BaseFeatureType WallType;
    public BaseFeatureType FloorType;
    public RoofType RoofType;
    public Color? WallColor;

    public BuildingGenerationSettings(int minWidth, int maxWidth, int minHeight, int maxHeight, BaseFeatureType wallType, BaseFeatureType floorType, RoofType roofType, Color? wallColor = null)
    {
        MinWidth = minWidth;
        MaxWidth = maxWidth;
        MinHeight = minHeight;
        MaxHeight = maxHeight;
        WallType = wallType;
        FloorType = floorType;
        RoofType = roofType;
        WallColor = wallColor;
    }
}
