using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_Ruins : Region
{
    public Region_Ruins(Vector2Int id) : base(id)
    {
        Type = RegionType.Ruins;
    }

    public override TileType GetTileType(int gridX, int gridY)
    {
        return GetRandomTileType();
    }

    private TileType GetRandomTileType()
    {
        float rng = Random.value;
        if (rng < 0.5f) return TileType.Grass;
        else if (rng < 0.6f) return TileType.Desert;
        else return TileType.Mountain;
    }
}
