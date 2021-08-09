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
            if(Random.value <= 0.002f) Generator.SetTileTypeWithInfo(pos, TileType.Water);
            if(Random.value <= 0.004f) Generator.SetTileTypeWithInfo(pos, TileType.Mountain);
            else Generator.SetTileTypeWithInfo(pos, TileType.Desert);
        }
    }
}
