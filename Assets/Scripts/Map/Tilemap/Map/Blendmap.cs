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

    public Tilemap BlendTilemap_E_Empty;
    public Tilemap BlendTilemap_E_EmptyN;
    public Tilemap BlendTilemap_E_EmptyS;
    public Tilemap BlendTilemap_W_Empty;
    public Tilemap BlendTilemap_W_EmptyN;
    public Tilemap BlendTilemap_W_EmptyS;
    public Tilemap BlendTilemap_N_Empty;
    public Tilemap BlendTilemap_N_EmptyE;
    public Tilemap BlendTilemap_N_EmptyW;
    public Tilemap BlendTilemap_S_Empty;
    public Tilemap BlendTilemap_S_EmptyE;
    public Tilemap BlendTilemap_S_EmptyW;


    public void BlendTile(TilemapGenerator generator, int x, int y, BaseSurfaceType type, TileSetSimple tileset)
    {
        // Care: Directions refer to the direction on the blend tile, not the neighbour direction, so it's actually reversed when looking from the source tile!
        Dictionary<Direction, BaseSurfaceType> neighbourTypes = new Dictionary<Direction, BaseSurfaceType>(); // Dictionary that contains info about each neighbour if it the same type (we only blend to other types)
        neighbourTypes.Add(Direction.N, generator.GetTileInfo(new Vector2Int(x, y - 1)).BaseSurfaceType);
        neighbourTypes.Add(Direction.NE, generator.GetTileInfo(new Vector2Int(x - 1, y - 1)).BaseSurfaceType);
        neighbourTypes.Add(Direction.E, generator.GetTileInfo(new Vector2Int(x - 1, y)).BaseSurfaceType);
        neighbourTypes.Add(Direction.SE, generator.GetTileInfo(new Vector2Int(x - 1, y + 1)).BaseSurfaceType);
        neighbourTypes.Add(Direction.S, generator.GetTileInfo(new Vector2Int(x, y + 1)).BaseSurfaceType);
        neighbourTypes.Add(Direction.SW, generator.GetTileInfo(new Vector2Int(x + 1, y + 1)).BaseSurfaceType);
        neighbourTypes.Add(Direction.W, generator.GetTileInfo(new Vector2Int(x + 1, y)).BaseSurfaceType);
        neighbourTypes.Add(Direction.NW, generator.GetTileInfo(new Vector2Int(x + 1, y - 1)).BaseSurfaceType);

        // N
        if (neighbourTypes[Direction.N] != type)
        {
            if (type != neighbourTypes[Direction.NE] && type != neighbourTypes[Direction.NW]) BlendTilemap_N.SetTile(new Vector3Int(x, y - 1, 0), tileset.GetRandomTile());
            else if (type != neighbourTypes[Direction.NE]) BlendTilemap_N_EmptyE.SetTile(new Vector3Int(x, y - 1, 0), tileset.GetRandomTile());
            else if (type != neighbourTypes[Direction.NW]) BlendTilemap_N_EmptyW.SetTile(new Vector3Int(x, y - 1, 0), tileset.GetRandomTile());
            else BlendTilemap_N_Empty.SetTile(new Vector3Int(x, y - 1, 0), tileset.GetRandomTile());
        }

        // NE
        if (neighbourTypes[Direction.NE] != type && neighbourTypes[Direction.N] != type && neighbourTypes[Direction.E] != type) 
            BlendTilemap_NE.SetTile(new Vector3Int(x - 1, y - 1, 0), tileset.GetRandomTile());

        // E
        if (neighbourTypes[Direction.E] != type)
        {
            if(type != neighbourTypes[Direction.NE] && type != neighbourTypes[Direction.SE]) BlendTilemap_E.SetTile(new Vector3Int(x - 1, y, 0), tileset.GetRandomTile());
            else if(type != neighbourTypes[Direction.NE]) BlendTilemap_E_EmptyN.SetTile(new Vector3Int(x - 1, y, 0), tileset.GetRandomTile());
            else if(type != neighbourTypes[Direction.SE]) BlendTilemap_E_EmptyS.SetTile(new Vector3Int(x - 1, y, 0), tileset.GetRandomTile());
            else BlendTilemap_E_Empty.SetTile(new Vector3Int(x - 1, y, 0), tileset.GetRandomTile());
        }

        // SE
        if (neighbourTypes[Direction.SE] != type && neighbourTypes[Direction.E] != type && neighbourTypes[Direction.S] != type) 
            BlendTilemap_SE.SetTile(new Vector3Int(x - 1, y + 1, 0), tileset.GetRandomTile());

        // S
        if (neighbourTypes[Direction.S] != type)
        {
            if (type != neighbourTypes[Direction.SE] && type != neighbourTypes[Direction.SW]) BlendTilemap_S.SetTile(new Vector3Int(x, y + 1, 0), tileset.GetRandomTile());
            else if (type != neighbourTypes[Direction.SE]) BlendTilemap_S_EmptyE.SetTile(new Vector3Int(x, y + 1, 0), tileset.GetRandomTile());
            else if (type != neighbourTypes[Direction.SW]) BlendTilemap_S_EmptyW.SetTile(new Vector3Int(x, y + 1, 0), tileset.GetRandomTile());
            else BlendTilemap_S_Empty.SetTile(new Vector3Int(x, y + 1, 0), tileset.GetRandomTile());
        }

        // SW
        if (neighbourTypes[Direction.SW] != type && neighbourTypes[Direction.S] != type && neighbourTypes[Direction.W] != type)
            BlendTilemap_SW.SetTile(new Vector3Int(x + 1, y + 1, 0), tileset.GetRandomTile());

        // W
        if (neighbourTypes[Direction.W] != type)
        {
            if (type != neighbourTypes[Direction.NW] && type != neighbourTypes[Direction.SW]) BlendTilemap_W.SetTile(new Vector3Int(x + 1, y, 0), tileset.GetRandomTile());
            else if (type != neighbourTypes[Direction.NW]) BlendTilemap_W_EmptyN.SetTile(new Vector3Int(x + 1, y, 0), tileset.GetRandomTile());
            else if (type != neighbourTypes[Direction.SW]) BlendTilemap_W_EmptyS.SetTile(new Vector3Int(x + 1, y, 0), tileset.GetRandomTile());
            else BlendTilemap_W_Empty.SetTile(new Vector3Int(x + 1, y, 0), tileset.GetRandomTile());
        }

        // NW
        if (neighbourTypes[Direction.NW] != type && neighbourTypes[Direction.W] != type && neighbourTypes[Direction.N] != type)
            BlendTilemap_NW.SetTile(new Vector3Int(x + 1, y - 1, 0), tileset.GetRandomTile());
    }

}
