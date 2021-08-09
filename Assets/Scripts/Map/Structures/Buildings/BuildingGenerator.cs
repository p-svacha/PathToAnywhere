using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingGenerator : MonoBehaviour
{
    public Texture2D RoofSet1;

    public Building GenerateBuilding(TilemapGenerator generator, Vector2Int origin, TileType wallType, TileType floorType)
    {
        Building building = new Building(origin, wallType, floorType);

        List<Vector2Int> roofTiles = new List<Vector2Int>();
        building.RoofTileSet = TileGenerator.GenerateSlicedTileset(RoofSet1, TilemapGenerator.TilePixelSize);

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

                building.BuildingTiles.Add(absolutePos, building);
                roofTiles.Add(absolutePos);
                

                if (!building.TileTypes.ContainsKey(absolutePos) && Random.value < 0.9f)
                    building.TileTypes.Add(absolutePos, wallType);
            }
        }

        foreach(Vector2Int roofPos in roofTiles)
        {
            building.RoofTileSet.GetTileAt(roofPos, roofTiles, out TileBase roofTile, out int rotation);

            building.RoofTiles.Add(roofPos, roofTile);
            building.RoofTilesRotation.Add(roofPos, rotation);
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
