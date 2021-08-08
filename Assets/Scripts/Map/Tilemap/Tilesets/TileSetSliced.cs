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
   
    public TileSetSliced(TileSetData data, TileType type) : base(data, type) { }

    public override void PlaceTile(TilemapGenerator generator, Tilemap tilemap, Vector3Int position)
    {
        PlaceSlicedTile(generator, tilemap, position.x, position.y);
    }

    private void PlaceSlicedTile(TilemapGenerator generator, Tilemap tilemap, int tileX, int tileY)
    {
        TileBase tileToPlace;
        int tileRotation = 0;

        TileType up = generator.GetTileType(tileX, tileY + 1);
        TileType down = generator.GetTileType(tileX, tileY - 1);
        TileType left = generator.GetTileType(tileX - 1, tileY);
        TileType right = generator.GetTileType(tileX + 1, tileY);

        TileType upLeft = generator.GetTileType(tileX - 1, tileY + 1);
        TileType upRight = generator.GetTileType(tileX + 1, tileY + 1);
        TileType downLeft = generator.GetTileType(tileX - 1, tileY - 1);
        TileType downRight = generator.GetTileType(tileX + 1, tileY - 1);

        // Surrounded, Center0, Center1, Center2, Center3, Center4
        if (up == Type && down == Type && left == Type && right == Type)
        {
            // Surrounded
            if (upRight == Type && upLeft == Type && downRight == Type && downLeft == Type)
            {
                tileToPlace = Surrounded;
            }

            // Center4
            else if (upRight == Type && upLeft == Type && downLeft == Type)
            {
                tileToPlace = Center4;
            }
            else if (upLeft == Type && downRight == Type && downLeft == Type)
            {
                tileToPlace = Center4;
                tileRotation = 90;
            }
            else if (upRight == Type && downRight == Type && downLeft == Type)
            {
                tileToPlace = Center4;
                tileRotation = 180;
            }
            else if (upRight == Type && upLeft == Type && downRight == Type)
            {
                tileToPlace = Center4;
                tileRotation = 270;
            }

            // Center2 
            else if (upLeft == Type && downLeft == Type)
            {
                tileToPlace = Center2;
            }
            else if (downLeft == Type && downRight == Type)
            {
                tileToPlace = Center2;
                tileRotation = 90;
            }
            else if (downRight == Type && upRight == Type)
            {
                tileToPlace = Center2;
                tileRotation = 180;
            }
            else if (upRight == Type && upLeft == Type)
            {
                tileToPlace = Center2;
                tileRotation = 270;
            }

            // Center3
            else if (upLeft == Type && downRight == Type)
            {
                tileToPlace = Center3;
            }
            else if (upRight == Type && downLeft == Type)
            {
                tileToPlace = Center3;
                tileRotation = 90;
            }

            // Center1
            else if (upLeft == Type)
            {
                tileToPlace = Center1;
            }
            else if (downLeft == Type)
            {
                tileToPlace = Center1;
                tileRotation = 90;
            }
            else if (downRight == Type)
            {
                tileToPlace = Center1;
                tileRotation = 180;
            }
            else if (upRight == Type)
            {
                tileToPlace = Center1;
                tileRotation = 270;
            }

            // Center0
            else
            {
                tileToPlace = Center0;
            }

        }

        // T0, T1, T2, T3
        else if (up == Type && down == Type && left == Type) // -|
        {
            if (upLeft == Type && downLeft == Type) tileToPlace = T3;
            else if (downLeft == Type) tileToPlace = T2;
            else if (upLeft == Type) tileToPlace = T1;
            else tileToPlace = T0;
        }
        else if (down == Type && left == Type && right == Type) // T
        {
            tileRotation = 90;
            if (downLeft == Type && downRight == Type) tileToPlace = T3;
            else if (downRight == Type) tileToPlace = T2;
            else if (downLeft == Type) tileToPlace = T1;
            else tileToPlace = T0;
        }
        else if (up == Type && down == Type && right == Type) // |-
        {
            tileRotation = 180;
            if (upRight == Type && downRight == Type) tileToPlace = T3;
            else if (upRight == Type) tileToPlace = T2;
            else if (downRight == Type) tileToPlace = T1;
            else tileToPlace = T0;
        }
        else if (up == Type && left == Type && right == Type) // l
        {
            tileRotation = 270;
            if (upLeft == Type && upRight == Type) tileToPlace = T3;
            else if (upLeft == Type) tileToPlace = T2;
            else if (upRight == Type) tileToPlace = T1;
            else tileToPlace = T0;
        }

        // Corner0, Corner1
        else if (left == Type && up == Type) // J
        {
            if (upLeft == Type) tileToPlace = Corner1;
            else tileToPlace = Corner0;
        }
        else if (left == Type && down == Type) // ¬
        {
            tileRotation = 90;
            if (downLeft == Type) tileToPlace = Corner1;
            else tileToPlace = Corner0;
        }
        else if (down == Type && right == Type) // L
        {
            tileRotation = 180;
            if (downRight == Type) tileToPlace = Corner1;
            else tileToPlace = Corner0;
        }

        else if (right == Type && up == Type) // r
        {
            tileRotation = 270;
            if (upRight == Type) tileToPlace = Corner1;
            else tileToPlace = Corner0;
        }

        // Straight
        else if (left == Type && right == Type)
        {
            tileToPlace = Straight;
        }
        else if (down == Type && up == Type)
        {
            tileToPlace = Straight;
            tileRotation = 90;
        }

        // End
        else if (left == Type)
        {
            tileToPlace = End;
            tileRotation = 180;
        }
        else if (down == Type)
        {
            tileToPlace = End;
            tileRotation = 270;
        }
        else if (right == Type)
        {
            tileToPlace = End;
        }
        else if (up == Type)
        {
            tileToPlace = End;
            tileRotation = 90;
        }

        // Single
        else tileToPlace = Single;

        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        tilemap.SetTile(tilePos, tileToPlace);
        if (tileRotation != 0) tilemap.SetTransformMatrix(tilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, tileRotation), Vector3.one));
    }

}
