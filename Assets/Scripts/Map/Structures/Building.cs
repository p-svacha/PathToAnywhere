using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public Vector2Int Position;
    public int Width;
    public int Height;

    public TileType WallType;
    public TileType FloorType;

    public Building(Vector2Int position, int width, int height, TileType wallType, TileType floorType)
    {
        Position = position;
        Width = width;
        Height = height;
        WallType = wallType;
        FloorType = floorType;
    }

    public void PlaceBuilding(TilemapGenerator generator)
    {
        // Fill with walls
        for(int y = Position.y; y < Position.y + Height; y++)
        {
            for (int x = Position.x; x < Position.x + Width; x++)
            {
                generator.SetTileType(new Vector2Int(x, y), WallType);
            }
        }

        // Fill inside with floor
        for (int y = Position.y + 1; y < Position.y + Height - 1; y++)
        {
            for (int x = Position.x + 1; x < Position.x + Width - 1; x++)
            {
                generator.SetTileType(new Vector2Int(x, y), FloorType);
            }
        }

    }
}
