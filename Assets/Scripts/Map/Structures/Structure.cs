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

    public Structure(Vector2Int origin)
    {
        Origin = origin;
        BaseSurfaceTypes = new Dictionary<Vector2Int, BaseSurfaceType>();
        BaseFeatureTypes = new Dictionary<Vector2Int, BaseFeatureType>();
        OverlayTiles = new Dictionary<Vector2Int, TileBase>();
        FrontOfPlayerTiles = new Dictionary<Vector2Int, TileBase>();
        BuildingTiles = new Dictionary<Vector2Int, Building>();
        ImpassableTiles = new List<Vector2Int>();
    }

    public virtual void PlaceStructure(TilemapGenerator generator)
    {
        foreach (KeyValuePair<Vector2Int, BaseSurfaceType> kvp in BaseSurfaceTypes) generator.SetBaseSurfaceType(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, BaseFeatureType> kvp in BaseFeatureTypes) generator.SetBaseFeatureType(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in OverlayTiles) generator.SetOverlayTile(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in FrontOfPlayerTiles) generator.SetFrontOfPlayerTile(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, Building> kvp in BuildingTiles) generator.GetTileInfo(kvp.Key).Building = kvp.Value;
        foreach (Vector2Int impassableTilePos in ImpassableTiles) generator.GetTileInfo(impassableTilePos).Passable = false;
    }

    public List<Vector2Int> BaseTilePositions
    {
        get
        {
            return BaseSurfaceTypes.Keys.Concat(BaseFeatureTypes.Keys).ToList();
        }
    }

}
