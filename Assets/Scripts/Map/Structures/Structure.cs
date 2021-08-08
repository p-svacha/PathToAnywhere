using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Structure
{
    public Vector2Int Origin;

    public Dictionary<Vector2Int, TileType> TileTypes; // Tile types of the structure by grid position
    public Dictionary<Vector2Int, TileBase> OverlayTiles; // Overlay tiles of the structure by grid position
    public Dictionary<Vector2Int, TileBase> FrontOfPlayerTiles; // Front of player tiles of the structure by grid position

    public List<Vector2Int> ImpassableTiles; // List of tiles by their grid position that become impassable

    public Structure(Vector2Int origin)
    {
        Origin = origin;
        TileTypes = new Dictionary<Vector2Int, TileType>();
        OverlayTiles = new Dictionary<Vector2Int, TileBase>();
        FrontOfPlayerTiles = new Dictionary<Vector2Int, TileBase>();
        ImpassableTiles = new List<Vector2Int>();
    }

    public void PlaceStructure(TilemapGenerator generator)
    {
        foreach (KeyValuePair<Vector2Int, TileType> kvp in TileTypes) generator.SetTileTypeWithInfo(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in OverlayTiles) generator.SetOverlayTile(kvp.Key, kvp.Value);
        foreach (KeyValuePair<Vector2Int, TileBase> kvp in FrontOfPlayerTiles) generator.SetFrontOfPlayerTile(kvp.Key, kvp.Value);
        foreach (Vector2Int impassableTilePos in ImpassableTiles) generator.GetTileInfo(impassableTilePos).Passable = false;
    }

}
