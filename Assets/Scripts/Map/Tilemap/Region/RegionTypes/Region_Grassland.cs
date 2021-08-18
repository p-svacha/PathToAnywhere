using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Grassland : Region
{
    private const float ROCK_CHANCE = 0.02f;
    private const float TREE_CHANCE = 0.05f;

    private List<Building> Buildings = new List<Building>();
    private List<Tree> Trees = new List<Tree>();

    public Region_Grassland(GameModel model, Vector2Int id) : base(model, id)
    {
        Type = RegionType.Grassland;
    }
    
    protected override void GenerateLayout()
    {
        // Base landscape (grass & grassrock tiles, tree structures)
        foreach (Vector2Int pos in TilePositions)
        {
            if(Mathf.PerlinNoise(5000f + pos.x * 0.1f, 90000f + pos.y * 0.1f) < 0.5f) MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Grass1);
            else MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Grass2);

            float rng = Random.value;
            if (rng <= ROCK_CHANCE) MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Rock);
            else if (rng <= ROCK_CHANCE + TREE_CHANCE)
            {
                Tree tree = TreeGenerator.Instance.GenerateTree(pos);
                Trees.Add(tree);
            }
        }

        // Generate buildings
        int numBuildings = 5;
        for (int i = 0; i < numBuildings; i++)
        {
            Building building = null;
            bool canSpawnBuilding = false;
            int attempts = 0;

            while (!canSpawnBuilding && attempts <= 20)
            {
                attempts++;
                canSpawnBuilding = true;
                Vector2Int buildingPosition = TilePositions[Random.Range(0, TilePositions.Count)];

                building = BuildingGenerator.Instance.GenerateBuilding(MapGenerator, buildingPosition, BaseFeatureType.Wall, BaseFeatureType.Floor, RoofType.DefaultRoof);

                // Check if building is fully within region
                if(!building.IsFullyWithinRegion(this))
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

            Buildings.Add(building);
        }

        // Place buildings
        foreach(Building b in Buildings)
        {
            // Remove all trees that are within building
            List<Tree> treesToRemove = Trees.Where(x => b.BaseTilePositions.Contains(x.Origin)).ToList();
            foreach (Tree t in treesToRemove) Trees.Remove(t);
            b.PlaceStructure(Model);
        }

        // Place trees
        foreach (Tree t in Trees) t.PlaceStructure(Model);
    }
}
