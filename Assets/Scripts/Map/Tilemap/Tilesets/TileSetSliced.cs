using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetSliced : TileSet
{
    // For order which piece is which, look at order of WallSet1.png
    public TileBase Surrounded;
    public TileBase Center1;
    public TileBase Center2;
    public TileBase Center3;

    public TileBase Center4;
    public TileBase T1;
    public TileBase T2;
    public TileBase T3;

    public TileBase Single;
    public TileBase Center0;
    public TileBase T0;
    public TileBase Corner1;

    public TileBase End;
    public TileBase Straight;
    public TileBase Corner0;
   
    public TileSetSliced() { }
    public TileSetSliced(TileSetData data, SurfaceType type) : base(data, type) { }

    public override void PlaceTile(TilemapGenerator generator, Tilemap tilemap, Vector3Int position)
    {
        PlaceSlicedTile(generator, tilemap, position.x, position.y);
    }

    private void PlaceSlicedTile(TilemapGenerator generator, Tilemap tilemap, int tileX, int tileY)
    {
        TileBase tileToPlace;
        int tileRotation = 0;

        bool hasNorthNeighbour = generator.GetTileType(tileX, tileY + 1) == Type;
        bool hasSouthNeighbour = generator.GetTileType(tileX, tileY - 1) == Type;
        bool hasEastNeighbour = generator.GetTileType(tileX + 1, tileY) == Type;
        bool hasWestNeighbour = generator.GetTileType(tileX - 1, tileY) == Type;

        bool hasNorthWestNeighbour = generator.GetTileType(tileX - 1, tileY + 1) == Type;
        bool hasNorthEastNeighbour = generator.GetTileType(tileX + 1, tileY + 1) == Type;
        bool hasSouthWestNeighbour = generator.GetTileType(tileX - 1, tileY - 1) == Type;
        bool hasSouthEastNeighbour = generator.GetTileType(tileX + 1, tileY - 1) == Type;

        GetTileAt(hasNorthNeighbour, hasSouthNeighbour, hasEastNeighbour, hasWestNeighbour, hasNorthEastNeighbour, hasNorthWestNeighbour, hasSouthEastNeighbour, hasSouthWestNeighbour, out tileToPlace, out tileRotation);

        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        tilemap.SetTile(tilePos, tileToPlace);
        if (tileRotation != 0) tilemap.SetTransformMatrix(tilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, tileRotation), Vector3.one));
    }

    /// <summary>
    /// This method returns the correct tile and rotation at the given position. The list param contains all positions of where tiles of this type also occur.
    /// </summary>
    public void GetTileAt(Vector2Int gridPosition, List<Vector2Int> tiles, out TileBase tile, out int rotation)
    {
        bool hasNorthNeighbour = tiles.Contains(gridPosition + new Vector2Int(0, 1));
        bool hasSouthNeighbour = tiles.Contains(gridPosition + new Vector2Int(0, -1));
        bool hasEastNeighbour = tiles.Contains(gridPosition + new Vector2Int(1, 0));
        bool hasWestNeighbour = tiles.Contains(gridPosition + new Vector2Int(-1, 0));

        bool hasNorthEastNeighbour = tiles.Contains(gridPosition + new Vector2Int(1, 1));
        bool hasNorthWestNeighbour = tiles.Contains(gridPosition + new Vector2Int(-1, 1));
        bool hasSouthEastNeighbour = tiles.Contains(gridPosition + new Vector2Int(1,-1));
        bool hasSouthWestNeighbour = tiles.Contains(gridPosition + new Vector2Int(-1, -1));

        GetTileAt(hasNorthNeighbour, hasSouthNeighbour, hasEastNeighbour, hasWestNeighbour, hasNorthEastNeighbour, hasNorthWestNeighbour, hasSouthEastNeighbour, hasSouthWestNeighbour, out tile, out rotation);
    }

    private void GetTileAt(bool hasNorthNeighbour, bool hasSouthNeighbour, bool hasEastNeighbour, bool hasWestNeighbour, bool hasNorthEastNeighbour, bool hasNorthWestNeighbour, bool hasSouthEastNeighbour, bool hasSouthWestNeighbour, out TileBase tile, out int rotation)
    {
        tile = Single;
        rotation = 0;

        // Surrounded, Center0, Center1, Center2, Center3, Center4
        if (hasNorthNeighbour && hasSouthNeighbour && hasWestNeighbour && hasEastNeighbour)
        {
            // Surrounded
            if (hasNorthEastNeighbour && hasNorthWestNeighbour && hasSouthEastNeighbour && hasSouthWestNeighbour)
            {
                tile = Surrounded;
            }

            // Center4
            else if (hasNorthEastNeighbour && hasNorthWestNeighbour && hasSouthWestNeighbour)
            {
                tile = Center4;
            }
            else if (hasNorthWestNeighbour && hasSouthEastNeighbour && hasSouthWestNeighbour)
            {
                tile = Center4;
                rotation = 90;
            }
            else if (hasNorthEastNeighbour && hasSouthEastNeighbour && hasSouthWestNeighbour)
            {
                tile = Center4;
                rotation = 180;
            }
            else if (hasNorthEastNeighbour && hasNorthWestNeighbour && hasSouthEastNeighbour)
            {
                tile = Center4;
                rotation = 270;
            }

            // Center2 
            else if (hasNorthWestNeighbour && hasSouthWestNeighbour)
            {
                tile = Center2;
            }
            else if (hasSouthWestNeighbour && hasSouthEastNeighbour)
            {
                tile = Center2;
                rotation = 90;
            }
            else if (hasSouthEastNeighbour && hasNorthEastNeighbour)
            {
                tile = Center2;
                rotation = 180;
            }
            else if (hasNorthEastNeighbour && hasNorthWestNeighbour)
            {
                tile = Center2;
                rotation = 270;
            }

            // Center3
            else if (hasNorthWestNeighbour && hasSouthEastNeighbour)
            {
                tile = Center3;
            }
            else if (hasNorthEastNeighbour && hasSouthWestNeighbour)
            {
                tile = Center3;
                rotation = 90;
            }

            // Center1
            else if (hasNorthWestNeighbour)
            {
                tile = Center1;
            }
            else if (hasSouthWestNeighbour)
            {
                tile = Center1;
                rotation = 90;
            }
            else if (hasSouthEastNeighbour)
            {
                tile = Center1;
                rotation = 180;
            }
            else if (hasNorthEastNeighbour)
            {
                tile = Center1;
                rotation = 270;
            }

            // Center0
            else
            {
                tile = Center0;
            }

        }

        // T0, T1, T2, T3
        else if (hasNorthNeighbour && hasSouthNeighbour && hasWestNeighbour) // -|
        {
            if (hasNorthWestNeighbour && hasSouthWestNeighbour) tile = T3;
            else if (hasSouthWestNeighbour) tile = T2;
            else if (hasNorthWestNeighbour) tile = T1;
            else tile = T0;
        }
        else if (hasSouthNeighbour && hasWestNeighbour && hasEastNeighbour) // T
        {
            rotation = 90;
            if (hasSouthWestNeighbour && hasSouthEastNeighbour) tile = T3;
            else if (hasSouthEastNeighbour) tile = T2;
            else if (hasSouthWestNeighbour) tile = T1;
            else tile = T0;
        }
        else if (hasNorthNeighbour && hasSouthNeighbour && hasEastNeighbour) // |-
        {
            rotation = 180;
            if (hasNorthEastNeighbour && hasSouthEastNeighbour) tile = T3;
            else if (hasNorthEastNeighbour) tile = T2;
            else if (hasSouthEastNeighbour) tile = T1;
            else tile = T0;
        }
        else if (hasNorthNeighbour && hasWestNeighbour && hasEastNeighbour) // l
        {
            rotation = 270;
            if (hasNorthWestNeighbour && hasNorthEastNeighbour) tile = T3;
            else if (hasNorthWestNeighbour) tile = T2;
            else if (hasNorthEastNeighbour) tile = T1;
            else tile = T0;
        }

        // Corner0, Corner1
        else if (hasWestNeighbour && hasNorthNeighbour) // J
        {
            if (hasNorthWestNeighbour) tile = Corner1;
            else tile = Corner0;
        }
        else if (hasWestNeighbour && hasSouthNeighbour) // ¬
        {
            rotation = 90;
            if (hasSouthWestNeighbour) tile = Corner1;
            else tile = Corner0;
        }
        else if (hasSouthNeighbour && hasEastNeighbour) // L
        {
            rotation = 180;
            if (hasSouthEastNeighbour) tile = Corner1;
            else tile = Corner0;
        }

        else if (hasEastNeighbour && hasNorthNeighbour) // r
        {
            rotation = 270;
            if (hasNorthEastNeighbour) tile = Corner1;
            else tile = Corner0;
        }

        // Straight
        else if (hasWestNeighbour && hasEastNeighbour)
        {
            tile = Straight;
        }
        else if (hasSouthNeighbour && hasNorthNeighbour)
        {
            tile = Straight;
            rotation = 90;
        }

        // End
        else if (hasWestNeighbour)
        {
            tile = End;
            rotation = 180;
        }
        else if (hasSouthNeighbour)
        {
            tile = End;
            rotation = 270;
        }
        else if (hasEastNeighbour)
        {
            tile = End;
        }
        else if (hasNorthNeighbour)
        {
            tile = End;
            rotation = 90;
        }

        // Single
        else tile = Single;
    }

}
