using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : Structure
{
    public BaseFeatureType WallType;
    public BaseFeatureType FloorType;
    public RoofType RoofType;

    public Color Color;

    public Dictionary<Vector2Int, TileBase> RoofTiles; // Tiles that are rendered on the roof tilemap but only when player is outside of the building

    public Building(Vector2Int origin, BaseFeatureType wallType, BaseFeatureType floorType, RoofType roofType) : base(origin)
    {
        WallType = wallType;
        FloorType = floorType;
        RoofType = roofType;
        Color = ColorManager.GetRandomRedishColor();
        RoofTiles = new Dictionary<Vector2Int, TileBase>();
    }

    public bool IntersectsWith(Building other)
    {
        return BaseTilePositions.Intersect(other.BaseTilePositions).Count() > 0;
    }

    public bool IsFullyWithinRegion(Region region)
    {
        foreach(Vector2Int tile in BaseTilePositions)
        {
            if (!region.TilePositions.Contains(tile)) return false;
        }
        return true;
    }

    public void SetDrawRoof(TilemapGenerator generator, bool draw)
    {
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in RoofTiles)
        {
            if (draw) generator.SetRoofTile(kvp.Key, kvp.Value);
            else generator.SetRoofTile(kvp.Key, null);
        }
    }

    public override void PlaceStructure(TilemapGenerator generator)
    {
        base.PlaceStructure(generator);
        SetDrawRoof(generator, true);
    }

}
