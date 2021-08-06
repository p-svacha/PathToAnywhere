using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Region
{
    public Vector2Int Id;
    public RegionType Type;

    public Region(Vector2Int id)
    {
        Id = id;
    }

    public abstract TileType GetTileType(int gridX, int gridY);
}
