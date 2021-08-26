using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Grassland : Region
{
    private const float ROCK_CHANCE = 0.02f;
    private const float TREE_CHANCE = 0.05f;

    public Region_Grassland(GameModel model, Vector2Int id) : base(model, id)
    {
        Type = RegionType.Grassland;
        SettlementChance = 1f;
    }

    protected override void GenerateLayout()
    {
        // Base landscape (grass & grassrock tiles, tree structures)
        foreach (Vector2Int pos in TilePositions)
        {
            if (Mathf.PerlinNoise(5000f + pos.x * 0.1f, 90000f + pos.y * 0.1f) < 0.5f) MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Grass1);
            else MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Grass2);

            float rng = Random.value;
            if (rng <= ROCK_CHANCE) MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Rock);
            else if (rng <= ROCK_CHANCE + TREE_CHANCE)
            {
                Tree tree = TreeGenerator.Instance.GenerateTree(pos);
                Trees.Add(tree);
            }
        }

        // Small building
        int numBuildings = 1;
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
                Vector2Int buildingPosition = TilePositions[Random.Range(0, TilePositions.Count)];

                BuildingGenerationSettings settings = new BuildingGenerationSettings(3, 4, 3, 4, BaseFeatureType.Wall, BaseFeatureType.Floor, RoofType.DefaultRoof, ColorManager.GetRandomColor());
                building = BuildingGenerator.GenerateBuilding(MapGenerator, buildingPosition, settings);

                // Check if building is fully within region
                if (!building.IsFullyWithinRegion(this))
                {
                    canSpawnBuilding = false;
                    continue;
                }

                // Check if building overlaps with any other building
                foreach (Building b in Buildings)
                {
                    if (building.IntersectsWith(b))
                    {
                        canSpawnBuilding = false;
                        continue;
                    }
                }
            }

            // Remove trees that were in the way of the building
            List<Tree> treesToRemove = Trees.Where(x => building.BaseTilePositions.Contains(x.Origin)).ToList();
            foreach (Tree t in treesToRemove) Trees.Remove(t);

            Buildings.Add(building);
        }
    }
}
