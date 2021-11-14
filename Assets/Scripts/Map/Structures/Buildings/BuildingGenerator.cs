using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class BuildingGenerator
{
    /// <summary>
    /// Generates the metadata for a building without yet placing it on the map
    /// </summary>
    public static Building GenerateBuilding(TilemapGenerator generator, BuildingGenerationSettings settings)
    {
        Building building = new Building(settings.Origin, settings.WallType, settings.FloorType, settings.RoofType);

        List<Vector2Int> roofTiles = new List<Vector2Int>();

        // Floor
        for (int y = 1; y < settings.BoundsDimensions.y - 1; y++)
        {
            for(int x = 1; x < settings.BoundsDimensions.x - 1; x++)
            {
                Vector2Int relativePos = new Vector2Int(x, y);
                building.BaseFeatureTypes.Add(settings.BoundsSwCorner + relativePos, settings.FloorType);
                building.InsideTiles.Add(settings.BoundsSwCorner + relativePos);
            }
        }

        // Walls
        for (int y = 0; y < settings.BoundsDimensions.y; y++)
        {
            for (int x = 0; x < settings.BoundsDimensions.x; x++)
            {
                Vector2Int relativePos = new Vector2Int(x, y);
                Vector2Int absolutePos = settings.BoundsSwCorner + relativePos;

                building.BuildingTiles.Add(absolutePos, building);
                roofTiles.Add(absolutePos);
                

                if (!building.BaseFeatureTypes.ContainsKey(absolutePos))
                {
                    if(absolutePos == settings.Origin)
                    {
                        building.BaseFeatureTypes.Add(absolutePos, settings.FloorType);
                    }
                    else
                    {
                        building.BaseFeatureTypes.Add(absolutePos, settings.WallType);
                        building.BaseFeatureColor.Add(absolutePos, settings.WallColor);
                    }
                }
            }
        }

        // Roof
        foreach(Vector2Int roofPos in roofTiles)
        {
            TileBase roofTile = generator.RoofTilesets[settings.RoofType].GetTileAt(roofPos, roofTiles);

            building.RoofTiles.Add(roofPos, roofTile);
        }

        return building;
    }
}
