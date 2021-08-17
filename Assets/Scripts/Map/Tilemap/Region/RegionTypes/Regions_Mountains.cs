using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Regions_Mountains : Region
{
    public Regions_Mountains(GameModel model, Vector2Int id) : base(model, id)
    {
        Type = RegionType.Mountain;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Mountain);
        }
    }
}
