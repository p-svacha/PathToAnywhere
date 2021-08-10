using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Structure
{
    public Vector2Int Origin;

    public Dictionary<Vector2Int, SurfaceType> TileTypes; // Tile types of the structure by grid position
    public Dictionary<Vector2Int, TileBase> OverlayTiles; // Overlay tiles of the structure by grid position
    public Dictionary<Vector2Int, TileBase> FrontOfPlayerTiles; // Front of player tiles of the structure by grid position

    public Dictionary<Vector2Int, Building> BuildingTiles; // Tiles that will be assigned to a building

    public List<Vector2Int> ImpassableTiles; // List of tiles by their grid position that become impassable

    public Structure(Vector2Int origin)
    {
        Origin = origin;
        TileTypes = new Dictionary<Vector2Int, SurfaceType>();
        OverlayTiles = new Dictionary<Vector2Int, TileBase>();
        FrontOfPlayerTiles = new Dictionary<Vector2Int, TileBase>();
        BuildingTiles = new Dictionary<Vector2Int, Building>();
        ImpassableTiles = new List<Vector2Int>();
    }

    public virtual void PlaceStructure(TilemapGenerator generator)
    {
        foreach (KeyValuePair<Vector2Int, SurfaceType> kvp in TileTypes) generator.SetTileTypeWithInfo(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in OverlayTiles) generator.SetOverlayTile(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in FrontOfPlayerTiles) generator.SetFrontOfPlayerTile(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, Building> kvp in BuildingTiles) generator.GetTileInfo(kvp.Key).Building = kvp.Value;
        foreach (Vector2Int impassableTilePos in ImpassableTiles) generator.GetTileInfo(impassableTilePos).Passable = false;
    }

}
