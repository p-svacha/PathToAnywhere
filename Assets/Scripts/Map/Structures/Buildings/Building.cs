using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : Structure
{
    public TileType WallType;
    public TileType FloorType;

    public Building(Vector2Int origin, TileType wallType, TileType floorType) : base(origin)
    {
        WallType = wallType;
        FloorType = floorType;
    }

    public bool IntersectsWith(Building other)
    {
        return TileTypes.Keys.Intersect(other.TileTypes.Keys).Count() > 0;
    }

    public bool IsFullyWithinRegion(Region region)
    {
        foreach(Vector2Int tile in TileTypes.Keys)
        {
            if (!region.TilePositions.Contains(tile)) return false;
        }
        return true;
    }

}
