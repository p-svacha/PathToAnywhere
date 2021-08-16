using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// A blendmap consists of 8 tilemaps (one for each directions) that each have another blending direction. It is used as a central point to blend different tilemaps
/// </summary>
public class Blendmap : MonoBehaviour
{
    public Tilemap BlendTilemap_N;
    public Tilemap BlendTilemap_NE;
    public Tilemap BlendTilemap_E;
    public Tilemap BlendTilemap_SE;
    public Tilemap BlendTilemap_S;
    public Tilemap BlendTilemap_SW;
    public Tilemap BlendTilemap_W;
    public Tilemap BlendTilemap_NW;


    public void BlendTile(TilemapGenerator generator, int x, int y, BaseSurfaceType type, TileSetSimple tileset)
    {
        // Todo: Only blend when type is != null

        // N
        if (generator.GetTileInfo(new Vector2Int(x, y - 1)).BaseSurfaceType != type) BlendTilemap_N.SetTile(new Vector3Int(x, y - 1, 0), tileset.GetRandomTile());

        // NE
        if (generator.GetTileInfo(new Vector2Int(x - 1, y - 1)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x, y - 1)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x - 1, y)).BaseSurfaceType != type) 
            BlendTilemap_NE.SetTile(new Vector3Int(x - 1, y - 1, 0), tileset.GetRandomTile());

        // E
        if (generator.GetTileInfo(new Vector2Int(x - 1, y)).BaseSurfaceType != type) BlendTilemap_E.SetTile(new Vector3Int(x - 1, y, 0), tileset.GetRandomTile());

        // SE
        if (generator.GetTileInfo(new Vector2Int(x - 1, y + 1)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x - 1, y)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x, y + 1)).BaseSurfaceType != type) 
            BlendTilemap_SE.SetTile(new Vector3Int(x - 1, y + 1, 0), tileset.GetRandomTile());

        // S
        if (generator.GetTileInfo(new Vector2Int(x, y + 1)).BaseSurfaceType != type) BlendTilemap_S.SetTile(new Vector3Int(x, y + 1, 0), tileset.GetRandomTile());

        // SW
        if (generator.GetTileInfo(new Vector2Int(x + 1, y + 1)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x, y + 1)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x + 1, y)).BaseSurfaceType != type)
            BlendTilemap_SW.SetTile(new Vector3Int(x + 1, y + 1, 0), tileset.GetRandomTile());

        // W
        if (generator.GetTileInfo(new Vector2Int(x + 1, y)).BaseSurfaceType != type) BlendTilemap_W.SetTile(new Vector3Int(x + 1, y, 0), tileset.GetRandomTile());

        // NW
        if (generator.GetTileInfo(new Vector2Int(x + 1, y - 1)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x + 1, y)).BaseSurfaceType != type && generator.GetTileInfo(new Vector2Int(x, y - 1)).BaseSurfaceType != type)
            BlendTilemap_NW.SetTile(new Vector3Int(x + 1, y - 1, 0), tileset.GetRandomTile());
    }

}
