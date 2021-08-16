using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Regions_Mountains : Region
{
    public Regions_Mountains(TilemapGenerator generator, Vector2Int id) : base(generator, id)
    {
        Type = RegionType.Mountain;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            Generator.SetBaseFeatureType(pos, BaseFeatureType.Mountain);
        }
    }
}
