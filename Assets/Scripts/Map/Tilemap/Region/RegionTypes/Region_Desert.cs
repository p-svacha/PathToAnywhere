using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Desert : Region
{
    public Region_Desert(GameModel model, Vector2Int id) : base(model, id)
    {
        Type = RegionType.Desert;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Sand);
            if (Random.value <= 0.002f) MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Wall);
            if (Random.value <= 0.004f) MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Mountain);
        }
    }
}
