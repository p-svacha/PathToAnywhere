using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_Lake : Region
{
    public Region_Lake(GameModel model, Vector2Int id) : base(model, id)
    {
        Type = RegionType.Lake;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Sand);
            MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Water);
        }
    }
}
