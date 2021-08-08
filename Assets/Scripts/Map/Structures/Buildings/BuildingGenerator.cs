using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildingGenerator
{

    public static Building GenerateBuilding(Vector2Int origin, TileType wallType, TileType floorType)
    {
        Building building = new Building(origin, wallType, floorType);

        int width = Random.Range(4, 10);
        int height = Random.Range(4, 10);

        for (int y = 1; y < height - 1; y++)
        {
            for(int x = 1; x < width - 1; x++)
            {
                Vector2Int relativePos = new Vector2Int(x, y);
                building.TileTypes.Add(origin + relativePos, floorType);
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int relativePos = new Vector2Int(x, y);
                Vector2Int absolutePos = origin + relativePos;
                if (!building.TileTypes.ContainsKey(absolutePos))
                    building.TileTypes.Add(absolutePos, wallType);
            }
        }

        return building;
    }
}
