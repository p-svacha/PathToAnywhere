using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetSlicedFull : TileSet
{
    public List<SurfaceType> ConnectionTypes; // The sliced tile will "connect" to all surface types in this list
    public bool HasOverlayEdges; // If this flag is true, the edges of the sliced tiles have transparency in them and are required to be overlayed over a surface texture

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

    public Dictionary<System.Tuple<TileBase, SurfaceType>, TileBase> OverlayTiles = new Dictionary<System.Tuple<TileBase, SurfaceType>, TileBase>(); // This maps a specific tile + surfacetype combination to the resulting tile

    public TileSetSlicedFull(List<SurfaceType> connectionTypes, bool hasOverlayEdges)
    {
        ConnectionTypes = connectionTypes;
        HasOverlayEdges = hasOverlayEdges;
    }
    public TileSetSlicedFull(List<SurfaceType> connectionTypes, TileSetData data, SurfaceType type, bool hasOverlayEdges) : base(data, type)
    {
        ConnectionTypes = connectionTypes;
        HasOverlayEdges = hasOverlayEdges;
    }

    public override void PlaceTile(TilemapGenerator generator, Tilemap tilemap, Vector3Int position)
    {
        PlaceSlicedTile(generator, tilemap, position.x, position.y);
    }

    private void PlaceSlicedTile(TilemapGenerator generator, Tilemap tilemap, int tileX, int tileY)
    {
        bool hasNorthNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX, tileY + 1).Type);
        bool hasSouthNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX, tileY - 1).Type);
        bool hasEastNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX + 1, tileY).Type);
        bool hasWestNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX - 1, tileY).Type);

        bool hasNorthWestNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX - 1, tileY + 1).Type);
        bool hasNorthEastNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX + 1, tileY + 1).Type);
        bool hasSouthWestNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX - 1, tileY - 1).Type);
        bool hasSouthEastNeighbour = ConnectionTypes.Contains(generator.GetTileInfo(tileX + 1, tileY - 1).Type);

        TileBase tileToPlace = GetTileAt(hasNorthNeighbour, hasSouthNeighbour, hasEastNeighbour, hasWestNeighbour, hasNorthEastNeighbour, hasNorthWestNeighbour, hasSouthEastNeighbour, hasSouthWestNeighbour);

        if(HasOverlayEdges && tileToPlace != Surrounded[0])
        {
            SurfaceType overlayType = GetOverlayType(generator, tileX, tileY);
            System.Tuple<TileBase, SurfaceType> overlayTile = new System.Tuple<TileBase, SurfaceType>(tileToPlace, overlayType);

            tileToPlace = OverlayTiles[overlayTile];
        }

        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        tilemap.SetTile(tilePos, tileToPlace);
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

    private SurfaceType GetOverlayType(TilemapGenerator generator, int tileX, int tileY)
    {
        Dictionary<SurfaceType, int> neighbourTypes = new Dictionary<SurfaceType, int>();
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX - 1, tileY).Type);
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX + 1, tileY).Type);
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX, tileY - 1).Type);
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX, tileY + 1).Type);
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX - 1, tileY - 1).Type);
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX - 1, tileY + 1).Type);
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX + 1, tileY - 1).Type);
        AddNeighbourType(neighbourTypes, generator.GetTileInfo(tileX + 1, tileY + 1).Type);
        if (neighbourTypes.Where(x => generator.GetBlendTypes().Contains(x.Key)).Count() == 0) return SurfaceType.Grass;
        else return neighbourTypes.Where(x => generator.GetBlendTypes().Contains(x.Key)).OrderByDescending(x => x.Value).First().Key;
    }
    private void AddNeighbourType(Dictionary<SurfaceType, int> neighbourTypes, SurfaceType type)
    {
        if (neighbourTypes.ContainsKey(type)) neighbourTypes[type]++;
        else neighbourTypes.Add(type, 1);
    }
}
