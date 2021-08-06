using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_Grassland : Region
{
    private int BuildingRadius;

    public Region_Grassland(Vector2Int id) : base(id)
    {
        Type = RegionType.Grassland;
        BuildingRadius = Random.Range(1, 5);
    }

    public override TileType GetTileType(int gridX, int gridY)
    {
        if ((gridX == Id.x - BuildingRadius && gridY >= Id.y - BuildingRadius && gridY <= Id.y + BuildingRadius) ||
            (gridX == Id.x + BuildingRadius && gridY >= Id.y - BuildingRadius && gridY <= Id.y + BuildingRadius) ||
            (gridY == Id.y - BuildingRadius && gridX >= Id.x - BuildingRadius && gridX <= Id.x + BuildingRadius) ||
            (gridY == Id.y + BuildingRadius && gridX >= Id.x - BuildingRadius && gridX <= Id.x + BuildingRadius)) return TileType.Wall;
        else if (gridX > Id.x - BuildingRadius && gridX < Id.x + BuildingRadius && gridY > Id.y - BuildingRadius && gridY < Id.y + BuildingRadius) return TileType.WoodFloor;
        else return TileType.Grass;
    }
}
