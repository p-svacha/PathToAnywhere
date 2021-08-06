using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regions_Mountains : Region
{
    public Regions_Mountains(Vector2Int id) : base(id)
    {
        Type = RegionType.Mountain;
    }

    public override TileType GetTileType(int gridX, int gridY)
    {
        return TileType.Mountain;
    }
}
