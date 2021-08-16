using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetSlicedFull : TileSet
{
    public List<BaseFeatureType> ConnectionTypes; // The sliced tile will "connect" to all surface types in this list

    // Single tiles of sliced set (the int refers to the rotation, the TileBase to the kind of tile)
    public Dictionary<int, TileBase> Surrounded = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> CenterEmpty = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> Single = new Dictionary<int, TileBase>();

    public Dictionary<int, TileBase> Straight = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> End = new Dictionary<int, TileBase>();

    public Dictionary<int, TileBase> CornerEmpty = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> CornerFull = new Dictionary<int, TileBase>();

    public Dictionary<int, TileBase> TEmpty = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> THalfFullA = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> THalfFullB = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> TFull = new Dictionary<int, TileBase>();

    public Dictionary<int, TileBase> CenterQuarterFull = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> CenterHalfFullStraight = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> CenterHalfFullCorners = new Dictionary<int, TileBase>();
    public Dictionary<int, TileBase> Center3QuartersFull = new Dictionary<int, TileBase>();

    public TileSetSlicedFull(List<BaseFeatureType> connectionTypes, TileSetData data) : base(data)
    {
        ConnectionTypes = connectionTypes;
    }

    public override void PlaceTile(TilemapGenerator generator, Tilemap tilemap, Vector3Int position)
    {
        PlaceSlicedTile(generator, tilemap, position.x, position.y);
    }

    private void PlaceSlicedTile(TilemapGenerator generator, Tilemap tilemap, int tileX, int tileY)
    {
        bool hasNorthNeighbour = HasNeighbour(generator, tileX, tileY + 1);
        bool hasSouthNeighbour = HasNeighbour(generator, tileX, tileY - 1);
        bool hasEastNeighbour = HasNeighbour(generator, tileX + 1, tileY);
        bool hasWestNeighbour = HasNeighbour(generator, tileX - 1, tileY);

        bool hasNorthWestNeighbour = HasNeighbour(generator, tileX - 1, tileY + 1);
        bool hasNorthEastNeighbour = HasNeighbour(generator, tileX + 1, tileY + 1);
        bool hasSouthWestNeighbour = HasNeighbour(generator, tileX - 1, tileY - 1);
        bool hasSouthEastNeighbour = HasNeighbour(generator, tileX + 1, tileY - 1);

        TileBase tileToPlace = GetTileAt(hasNorthNeighbour, hasSouthNeighbour, hasEastNeighbour, hasWestNeighbour, hasNorthEastNeighbour, hasNorthWestNeighbour, hasSouthEastNeighbour, hasSouthWestNeighbour);

        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        tilemap.SetTile(tilePos, tileToPlace);
    }

    private bool HasNeighbour(TilemapGenerator generator, int x, int y)
    {
        if(ConnectionTypes.Contains(generator.GetTileInfo(x, y).BaseFeatureType)) return true;
        return false;
    }

    private TileBase GetTileAt(bool hasNorthNeighbour, bool hasSouthNeighbour, bool hasEastNeighbour, bool hasWestNeighbour, bool hasNorthEastNeighbour, bool hasNorthWestNeighbour, bool hasSouthEastNeighbour, bool hasSouthWestNeighbour)
    {
        // Surrounded, Center0, Center1, Center2, Center3, Center4
        if (hasNorthNeighbour && hasSouthNeighbour && hasWestNeighbour && hasEastNeighbour)
        {
            // Surrounded
            if (hasNorthEastNeighbour && hasNorthWestNeighbour && hasSouthEastNeighbour && hasSouthWestNeighbour)
            {
                return Surrounded[0];
            }

            // Center4
            else if (hasNorthEastNeighbour && hasNorthWestNeighbour && hasSouthWestNeighbour)
            {
                return Center3QuartersFull[180];
            }
            else if (hasNorthWestNeighbour && hasSouthEastNeighbour && hasSouthWestNeighbour)
            {
                return Center3QuartersFull[270];
            }
            else if (hasNorthEastNeighbour && hasSouthEastNeighbour && hasSouthWestNeighbour)
            {
                return Center3QuartersFull[0];
            }
            else if (hasNorthEastNeighbour && hasNorthWestNeighbour && hasSouthEastNeighbour)
            {
                return Center3QuartersFull[90];
            }

            // Center2 
            else if (hasNorthWestNeighbour && hasSouthWestNeighbour)
            {
                return CenterHalfFullStraight[270];
            }
            else if (hasSouthWestNeighbour && hasSouthEastNeighbour)
            {
                return CenterHalfFullStraight[0];
            }
            else if (hasSouthEastNeighbour && hasNorthEastNeighbour)
            {
                return CenterHalfFullStraight[90];
            }
            else if (hasNorthEastNeighbour && hasNorthWestNeighbour)
            {
                return CenterHalfFullStraight[180];
            }

            // Center3
            else if (hasNorthWestNeighbour && hasSouthEastNeighbour)
            {
                return CenterHalfFullCorners[90];
            }
            else if (hasNorthEastNeighbour && hasSouthWestNeighbour)
            {
                return CenterHalfFullCorners[0];
            }

            // Center1
            else if (hasNorthWestNeighbour)
            {
                return CenterQuarterFull[180];
            }
            else if (hasSouthWestNeighbour)
            {
                return CenterQuarterFull[270];
            }
            else if (hasSouthEastNeighbour)
            {
                return CenterQuarterFull[0];
            }
            else if (hasNorthEastNeighbour)
            {
                return CenterQuarterFull[90];
            }

            // Center0
            else
            {
                return CenterEmpty[0];
            }

        }

        // T0, T1, T2, T3
        else if (hasNorthNeighbour && hasSouthNeighbour && hasWestNeighbour) // -|
        {
            int rotation = 0;
            if (hasNorthWestNeighbour && hasSouthWestNeighbour) return TFull[rotation];
            else if (hasSouthWestNeighbour) return THalfFullB[rotation];
            else if (hasNorthWestNeighbour) return THalfFullA[rotation];
            else return TEmpty[rotation];
        }
        else if (hasSouthNeighbour && hasWestNeighbour && hasEastNeighbour) // T
        {
            int rotation = 90;
            if (hasSouthWestNeighbour && hasSouthEastNeighbour) return TFull[rotation];
            else if (hasSouthEastNeighbour) return THalfFullB[rotation];
            else if (hasSouthWestNeighbour) return THalfFullA[rotation];
            else return TEmpty[rotation];
        }
        else if (hasNorthNeighbour && hasSouthNeighbour && hasEastNeighbour) // |-
        {
            int rotation = 180;
            if (hasNorthEastNeighbour && hasSouthEastNeighbour) return TFull[rotation];
            else if (hasNorthEastNeighbour) return THalfFullB[rotation];
            else if (hasSouthEastNeighbour) return THalfFullA[rotation];
            else return TEmpty[rotation];
        }
        else if (hasNorthNeighbour && hasWestNeighbour && hasEastNeighbour) // l
        {
            int rotation = 270;
            if (hasNorthWestNeighbour && hasNorthEastNeighbour) return TFull[rotation];
            else if (hasNorthWestNeighbour) return THalfFullB[rotation];
            else if (hasNorthEastNeighbour) return THalfFullA[rotation];
            else return TEmpty[rotation];
        }

        // Corner0, Corner1
        else if (hasWestNeighbour && hasNorthNeighbour) // J
        {
            int rotation = 0;
            if (hasNorthWestNeighbour) return CornerFull[rotation];
            else return CornerEmpty[rotation];
        }
        else if (hasWestNeighbour && hasSouthNeighbour) // ¬
        {
            int rotation = 90;
            if (hasSouthWestNeighbour) return CornerFull[rotation];
            else return CornerEmpty[rotation];
        }
        else if (hasSouthNeighbour && hasEastNeighbour) // L
        {
            int rotation = 180;
            if (hasSouthEastNeighbour) return CornerFull[rotation];
            else return CornerEmpty[rotation];
        }

        else if (hasEastNeighbour && hasNorthNeighbour) // r
        {
            int rotation = 270;
            if (hasNorthEastNeighbour) return CornerFull[rotation];
            else return CornerEmpty[rotation];
        }

        // Straight
        else if (hasWestNeighbour && hasEastNeighbour)
        {
            return Straight[0];
        }
        else if (hasSouthNeighbour && hasNorthNeighbour)
        {
            return Straight[90];
        }

        // End
        else if (hasWestNeighbour)
        {
            return End[90];
        }
        else if (hasSouthNeighbour)
        {
            return End[180];
        }
        else if (hasEastNeighbour)
        {
            return End[270];
        }
        else if (hasNorthNeighbour)
        {
            return End[0];
        }

        // Single
        else return Single[0];
    }

    /// <summary>
    /// This method returns the correct tile and rotation at the given position. The list param contains all positions of where tiles of this type also occur.
    /// </summary>
    public TileBase GetTileAt(Vector2Int gridPosition, List<Vector2Int> tiles)
    {
        bool hasNorthNeighbour = tiles.Contains(gridPosition + new Vector2Int(0, 1));
        bool hasSouthNeighbour = tiles.Contains(gridPosition + new Vector2Int(0, -1));
        bool hasEastNeighbour = tiles.Contains(gridPosition + new Vector2Int(1, 0));
        bool hasWestNeighbour = tiles.Contains(gridPosition + new Vector2Int(-1, 0));

        bool hasNorthEastNeighbour = tiles.Contains(gridPosition + new Vector2Int(1, 1));
        bool hasNorthWestNeighbour = tiles.Contains(gridPosition + new Vector2Int(-1, 1));
        bool hasSouthEastNeighbour = tiles.Contains(gridPosition + new Vector2Int(1, -1));
        bool hasSouthWestNeighbour = tiles.Contains(gridPosition + new Vector2Int(-1, -1));

        return GetTileAt(hasNorthNeighbour, hasSouthNeighbour, hasEastNeighbour, hasWestNeighbour, hasNorthEastNeighbour, hasNorthWestNeighbour, hasSouthEastNeighbour, hasSouthWestNeighbour);
    }

}
