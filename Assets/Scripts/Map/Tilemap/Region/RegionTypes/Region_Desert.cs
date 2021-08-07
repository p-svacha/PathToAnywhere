using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Desert : Region
{
    public Region_Desert(TilemapGenerator generator, Vector2Int id) : base(generator, id)
    {
        Type = RegionType.Desert;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            Generator.SetTileType(pos, TileType.Desert);
        }
    }
}
