using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_Desert : Region
{
    public Region_Desert(Vector2Int id) : base(id)
    {
        Type = RegionType.Desert;
    }

    public override TileType GetTileType(int gridX, int gridY)
    {
        return TileType.Desert;
    }
}
