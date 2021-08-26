using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SettlementGenerator
{
    /// <summary>
    /// Generates the metadata for a settlement without yet placing it.
    /// The settlement will be placed within the region considering already existing buildings but removing trees.
    /// </summary>
    public static Settlement GenerateSettlement(GameModel model, Region region)
    {
        if (!region.FullyLoaded) throw new System.Exception("Cannot create a settlement in a region that is not fully loaded.");

        List<Structure> structures = new List<Structure>();
        List<Building> buildings = new List<Building>();

        int numBuildings = 5;
        for (int i = 0; i < numBuildings; i++)
        {
            // Generate a building
            Building building = null;
            bool canSpawnBuilding = false;
            int attempts = 0;

            while (!canSpawnBuilding && attempts <= 20)
            {
                attempts++;
                canSpawnBuilding = true;
                Vector2Int buildingPosition = region.TilePositions[Random.Range(0, region.TilePositions.Count)];

                BuildingGenerationSettings settings = new BuildingGenerationSettings(4, 10, 4, 10, BaseFeatureType.Wall, BaseFeatureType.Floor, RoofType.DefaultRoof);
                building = BuildingGenerator.GenerateBuilding(region.MapGenerator, buildingPosition, settings);

                // Check if building is fully within region
                if (!building.IsFullyWithinRegion(region))
                {
                    canSpawnBuilding = false;
                    continue;
                }

                // Check if building overlaps with any other building
                List<Building> collisionCheckBuildings = new List<Building>();
                collisionCheckBuildings.AddRange(buildings);
                collisionCheckBuildings.AddRange(region.Buildings);
                foreach (Building b in collisionCheckBuildings)
                {
                    if (building.IntersectsWith(b))
                    {
                        canSpawnBuilding = false;
                        continue;
                    }
                }
            }

            // Remove trees that were in the way of the building
            List<Tree> treesToRemove = region.Trees.Where(x => building.BaseTilePositions.Contains(x.Origin)).ToList();
            foreach (Tree t in treesToRemove) region.Trees.Remove(t);

            buildings.Add(building);
        }

        structures.AddRange(buildings);

        Settlement newSettlement = new Settlement(model, NameGenerator.GenerateName(NameGenerationType.Settlement), structures);
        return newSettlement;
    }
}
