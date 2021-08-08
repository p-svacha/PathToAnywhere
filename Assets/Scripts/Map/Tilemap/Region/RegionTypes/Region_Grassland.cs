using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Grassland : Region
{
    private List<Building> Buildings = new List<Building>();

    public Region_Grassland(TilemapGenerator generator, Vector2Int id) : base(generator, id)
    {
        Type = RegionType.Grassland;
    }

    protected override void GenerateLayout()
    {
        int numBuildings = 5;
        for (int i = 0; i < numBuildings; i++)
        {
            bool canSpawnBuilding = false;
            Vector2Int buildingPosition = Vector2Int.zero;
            int buildingWidth = 0;
            int buildingHeight = 0;
            int attempts = 0;

            while (!canSpawnBuilding && attempts <= 20)
            {
                attempts++;
                canSpawnBuilding = true;
                buildingPosition = TilePositions[Random.Range(0, TilePositions.Count)];
                buildingWidth = Random.Range(3, 9);
                buildingHeight = Random.Range(3, 9);

                // Check if any corner is outside of region
                if (!TilePositions.Contains(buildingPosition + new Vector2Int(buildingWidth, 0)))
                {
                    canSpawnBuilding = false;
                    continue;
                }
                if (!TilePositions.Contains(buildingPosition + new Vector2Int(0, buildingHeight)))
                {
                    canSpawnBuilding = false;
                    continue;
                }
                if (!TilePositions.Contains(buildingPosition + new Vector2Int(buildingWidth, buildingHeight)))
                {
                    canSpawnBuilding = false;
                    continue;
                }

                // Check if it overlaps with any other building
                foreach (Building b in Buildings)
                {
                    if (TileFunctions.DoRectanglesOverlap(buildingPosition, buildingWidth, buildingHeight, b.Position, b.Width, b.Height))
                    {
                        canSpawnBuilding = false;
                        continue;
                    }
                }
            }

            Buildings.Add(new Building(buildingPosition, buildingWidth, buildingHeight, TileType.Wall, TileType.WoodFloor));
        }


        // Base grass
        foreach (Vector2Int pos in TilePositions)
        {
            if(Random.value < 0.95f) Generator.SetTileType(pos, TileType.Grass);
            else Generator.SetTileType(pos, TileType.GrassRock);

        }

        // Buildings
        foreach(Building b in Buildings)
        {
            b.PlaceBuilding(Generator);
        }
    }
}
