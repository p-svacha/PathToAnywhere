using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingGenerator : MonoBehaviour
{
    public Building GenerateBuilding(TilemapGenerator generator, Vector2Int origin, BaseFeatureType wallType, BaseFeatureType floorType, RoofType roofType)
    {
        Building building = new Building(origin, wallType, floorType, roofType);

        List<Vector2Int> roofTiles = new List<Vector2Int>();

        int width = Random.Range(4, 10);
        int height = Random.Range(4, 10);

        // Floor
        for (int y = 1; y < height - 1; y++)
        {
            for(int x = 1; x < width - 1; x++)
            {
                Vector2Int relativePos = new Vector2Int(x, y);
                building.BaseFeatureTypes.Add(origin + relativePos, floorType);
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
                    if(Random.value < 0.9f) building.BaseFeatureTypes.Add(absolutePos, wallType);
                    else building.BaseFeatureTypes.Add(absolutePos, floorType);
                }
            }
        }

        // Roof
        foreach(Vector2Int roofPos in roofTiles)
        {
            TileBase roofTile = generator.RoofTilesets[roofType].GetTileAt(roofPos, roofTiles);

            building.RoofTiles.Add(roofPos, roofTile);
        }

        return building;
    }

    public static BuildingGenerator Instance
    {
        get
        {
            return GameObject.Find("BuildingGenerator").GetComponent<BuildingGenerator>();
        }
    }
}
