using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : Structure
{
    public TileType WallType;
    public TileType FloorType;

    public Color Color;

    public TileSetSliced RoofTileSet;
    public Dictionary<Vector2Int, TileBase> RoofTiles; // Tiles that are rendered on the roof tilemap but only when player is outside of the building
    public Dictionary<Vector2Int, int> RoofTilesRotation; // Tiles that are rendered on the roof tilemap but only when player is outside of the building

    public Building(Vector2Int origin, TileType wallType, TileType floorType) : base(origin)
    {
        WallType = wallType;
        FloorType = floorType;
        Color = ColorManager.GetRandomRedishColor();
        RoofTiles = new Dictionary<Vector2Int, TileBase>();
        RoofTilesRotation = new Dictionary<Vector2Int, int>();
    }

    public bool IntersectsWith(Building other)
    {
        return TileTypes.Keys.Intersect(other.TileTypes.Keys).Count() > 0;
    }

    public bool IsFullyWithinRegion(Region region)
    {
        foreach(Vector2Int tile in TileTypes.Keys)
        {
            if (!region.TilePositions.Contains(tile)) return false;
        }
        return true;
    }

    public void SetDrawRoof(TilemapGenerator generator, bool draw)
    {
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in RoofTiles)
        {
            if (draw)
            {
                generator.SetRoofTile(kvp.Key, kvp.Value, RoofTilesRotation[kvp.Key]);
            }
            else generator.SetRoofTile(kvp.Key, null, 0);
            
        }
    }

    public override void PlaceStructure(TilemapGenerator generator)
    {
        base.PlaceStructure(generator);
        SetDrawRoof(generator, true);
    }

}
