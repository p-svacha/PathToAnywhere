using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Structure
{
    public Vector2Int Origin;

    public Dictionary<Vector2Int, BaseSurfaceType> BaseSurfaceTypes; // Base surface types of the structure by grid position
    public Dictionary<Vector2Int, BaseFeatureType> BaseFeatureTypes; // Base feature types of the structure by grid position
    public Dictionary<Vector2Int, TileBase> OverlayTiles; // Overlay tiles of the structure by grid position
    public Dictionary<Vector2Int, TileBase> FrontOfPlayerTiles; // Front of player tiles of the structure by grid position

    public Dictionary<Vector2Int, Building> BuildingTiles; // Tiles that will be assigned to a building

    public List<Vector2Int> ImpassableTiles; // List of tiles by their grid position that become impassable

    public Dictionary<Vector2Int, Color?> BaseSurfaceColor; // Color of base surface tiles that are not default
    public Dictionary<Vector2Int, Color?> BaseFeatureColor; // Color of base feature tiles that are not default

    public Settlement Settlement;

    public Structure(Vector2Int origin)
    {
        Origin = origin;
        BaseSurfaceTypes = new Dictionary<Vector2Int, BaseSurfaceType>();
        BaseFeatureTypes = new Dictionary<Vector2Int, BaseFeatureType>();
        OverlayTiles = new Dictionary<Vector2Int, TileBase>();
        FrontOfPlayerTiles = new Dictionary<Vector2Int, TileBase>();
        BuildingTiles = new Dictionary<Vector2Int, Building>();
        ImpassableTiles = new List<Vector2Int>();
        BaseSurfaceColor = new Dictionary<Vector2Int, Color?>();
        BaseFeatureColor = new Dictionary<Vector2Int, Color?>();
    }

    public virtual void PlaceStructure(GameModel model)
    {
        foreach (KeyValuePair<Vector2Int, BaseSurfaceType> kvp in BaseSurfaceTypes) model.TilemapGenerator.SetBaseSurfaceType(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, BaseFeatureType> kvp in BaseFeatureTypes) model.TilemapGenerator.SetBaseFeatureType(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in OverlayTiles) model.TilemapGenerator.SetOverlayTile(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in FrontOfPlayerTiles) model.TilemapGenerator.SetFrontOfPlayerTile(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, Building> kvp in BuildingTiles) model.TilemapGenerator.GetTileInfo(kvp.Key).Building = kvp.Value;
        foreach (Vector2Int impassableTilePos in ImpassableTiles) model.TilemapGenerator.GetTileInfo(impassableTilePos).Blocked = true;
        foreach (KeyValuePair<Vector2Int, Color?> kvp in BaseSurfaceColor) model.TilemapGenerator.SetBaseSurfaceColor(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, Color?> kvp in BaseFeatureColor) model.TilemapGenerator.SetBaseFeatureColor(kvp.Key, kvp.Value);
    }

    public bool IsColliding(Structure other)
    {
        return GetCollisionTiles().Intersect(other.GetCollisionTiles()).Count() > 0;
    }

    public bool IsFullyWithinRegion(Region region)
    {
        foreach (Vector2Int tile in GetCollisionTiles())
        {
            if (!region.TilePositions.Contains(tile)) return false;
        }
        return true;
    }

    public abstract List<Vector2Int> GetCollisionTiles();

    public List<Vector2Int> BaseTiles
    {
        get
        {
            return BaseSurfaceTypes.Keys.Concat(BaseFeatureTypes.Keys).ToList();
        }
    }

}
