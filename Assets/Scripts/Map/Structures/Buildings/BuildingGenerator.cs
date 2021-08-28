using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class BuildingGenerator
{
    /// <summary>
    /// Generates the metadata for a building without yet placing it on the map
    /// </summary>
    public static Building GenerateBuilding(TilemapGenerator generator, Vector2Int origin, BuildingGenerationSettings settings)
    {
        Building building = new Building(origin, settings.WallType, settings.FloorType, settings.RoofType);

        List<Vector2Int> roofTiles = new List<Vector2Int>();

        int width = Random.Range(settings.MinWidth, settings.MaxWidth + 1);
        int height = Random.Range(settings.MinHeight, settings.MaxHeight + 1);

        // Floor
        for (int y = 1; y < height - 1; y++)
        {
            for(int x = 1; x < width - 1; x++)
            {
                Vector2Int relativePos = new Vector2Int(x, y);
                building.BaseFeatureTypes.Add(origin + relativePos, settings.FloorType);
                building.InsideTiles.Add(origin + relativePos);
            }
        }

        // Walls
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int relativePos = new Vector2Int(x, y);
                Vector2Int absolutePos = origin + relativePos;

                building.BuildingTiles.Add(absolutePos, building);
                roofTiles.Add(absolutePos);
                

                if (!building.BaseFeatureTypes.ContainsKey(absolutePos))
                {
                    if (Random.value < 0.9f)
                    {
                        building.BaseFeatureTypes.Add(absolutePos, settings.WallType);
                        building.BaseFeatureColor.Add(absolutePos, settings.WallColor);
                    }
                    else building.BaseFeatureTypes.Add(absolutePos, settings.FloorType);
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
