using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Ruins : Region
{
    public Region_Ruins(TilemapGenerator generator, Vector2Int id) : base(generator, id)
    {
        Type = RegionType.Ruins;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            Generator.SetTileTypeWithInfo(pos, GetRandomTileType());
        }
    }

    private TileType GetRandomTileType()
    {
        float rng = Random.value;
        if (rng < 0.5f) return TileType.Grass;
        else if (rng < 0.6f) return TileType.Desert;
        else return TileType.Mountain;
    }
}
