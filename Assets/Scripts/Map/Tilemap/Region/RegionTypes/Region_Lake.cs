using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_Lake : Region
{
    public Region_Lake(TilemapGenerator generator, Vector2Int id) : base(generator, id)
    {
        Type = RegionType.Lake;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            Generator.SetTileTypeWithInfo(pos, SurfaceType.Water);
        }
    }
}
