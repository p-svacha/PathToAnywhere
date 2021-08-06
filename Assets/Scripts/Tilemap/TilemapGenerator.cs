using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    private GameModel Model;

    [Header("Tilemaps")]
    public Tilemap TilemapBase;
    public Tilemap TilemapRegions;

    [Header("Tiles")]
    public TileBase TestTile;
    public TileSetSimple GrassSet1;
    public TileSetSimple DesertSet1;
    public TileSetSimple WoodFloorSet1;
    public TileSetSliced WallSet1;
    public TileSetSliced WallSet2;

    private Dictionary<Vector2Int, TilemapChunk> Chunks = new Dictionary<Vector2Int, TilemapChunk>();
    private List<TilemapChunk> LoadedChunks = new List<TilemapChunk>(); // Loaded chunks include all chanks that are within generation are of the player

    public int MinGridX, MinGridY, MaxGridX, MaxGridY;

    [Header("Regions")]
    private Voronoi Voronoi;

    public void Init(GameModel model)
    {
        Model = model;
        Voronoi = new Voronoi();
    }

    public void GenerateTilemap(int chunkSize)
    {
        for(int y = -chunkSize / 2; y < (-chunkSize / 2) + chunkSize; y++)
        {
            for(int x = -chunkSize / 2; x < (-chunkSize / 2) + chunkSize; x++)
            {
                TilemapChunk chunk = GenerateChunk(new Vector2Int(x, y));
                GenerateMapLayout(chunk);
                PlaceTiles(chunk);
            }
        }
    }

    public void LoadChunksAroundPlayer(Vector2Int playerPosition, int visualRange, int interestRange)
    {
        Vector2Int playerChunkCoordinates = GetChunkCoordinates(playerPosition);
        for (int y = -interestRange; y <= interestRange; y++)
        {
            for (int x = -interestRange; x <= interestRange; x++)
            {
                TryCreateChunk(playerChunkCoordinates + new Vector2Int(x, y));
            }
        }
    }

    /// <summary>
    /// Creates a chunk at the given coordinates if it doesn't already exist.
    /// </summary>
    public void TryCreateChunk(Vector2Int chunkCoordinates)
    {
        if(!Chunks.ContainsKey(chunkCoordinates))
        {
            TilemapChunk chunk = GenerateChunk(chunkCoordinates);
            GenerateMapLayout(chunk);
            PlaceTiles(chunk);
        }
    }

    private TilemapChunk GenerateChunk(Vector2Int chunkCoordinates)
    {
        TilemapChunk chunk = new TilemapChunk(chunkCoordinates);
        if (chunk.MinGridX < MinGridX) MinGridX = chunk.MinGridX;
        if (chunk.MaxGridX > MaxGridX) MaxGridX = chunk.MaxGridX;
        if (chunk.MinGridY < MinGridY) MinGridY = chunk.MinGridY;
        if (chunk.MaxGridY > MaxGridY) MaxGridY = chunk.MaxGridY;
        Debug.Log("adding chunk at " + chunkCoordinates);
        Chunks.Add(chunkCoordinates, chunk);
        return chunk;
    }

    // Determines for each tile of what type it is, without placing an actual tile
    private void GenerateMapLayout(TilemapChunk chunk)
    {
        for (int y = 0; y < TilemapChunk.ChunkSize; y++)
        {
            for (int x = 0; x < TilemapChunk.ChunkSize; x++)
            {
                int tileX = chunk.Coordinates.x * TilemapChunk.ChunkSize + x;
                int tileY = chunk.Coordinates.y * TilemapChunk.ChunkSize + y;
                 
                // Get region
                Region region = Voronoi.GetRegionAt(new Vector2Int(tileX, tileY));
                chunk.Regions[x, y] = region;

                chunk.Tiles[x, y] = region.GetTileType(tileX, tileY);
            }
        }
    }

    /// <summary>
    /// Places the actual tiles for a chunk on the map according to a previously generated layout.
    /// Also places edge tiles on adjacent chunks that were waiting on information on neighbouring tiles.
    /// This method can change the loading state of neighbouring chunks by checking if all their neighbours are generated.
    /// </summary>
    private void PlaceTiles(TilemapChunk chunk)
    {
        // Chunk center
        for (int y = 1; y < TilemapChunk.ChunkSize - 1; y++)
        {
            for(int x = 1; x < TilemapChunk.ChunkSize - 1; x++)
            {
                PlaceTile(chunk, x, y);
            }
        }

        // Edge tiles (they are dependant on neighbouring chunks)
        TilemapChunk westChunk, eastChunk, northChunk, southChunk, northEastChunk, northWestChunk, southEastChunk, southWestChunk;

        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(-1, 0), out westChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(1, 0), out eastChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(0, -1), out southChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(0, 1), out northChunk);

        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(-1, 1), out northWestChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(1, 1), out northEastChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(-1, -1), out southWestChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(1, -1), out southEastChunk);

        // Left edge
        if (westChunk != null)
        {
            PlaceLeftEdgeTiles(chunk);
            PlaceRightEdgeTiles(westChunk);
            chunk.West = westChunk;
            westChunk.East = chunk;
            westChunk.CheckLoadingState();
        }

        // Right edge
        if (eastChunk != null)
        {
            PlaceRightEdgeTiles(chunk);
            PlaceLeftEdgeTiles(eastChunk);
            chunk.East = eastChunk;
            eastChunk.West = chunk;
            eastChunk.CheckLoadingState();
        }

        // Lower edge
        if (southChunk != null)
        {
            PlaceLowerEdgeTiles(chunk);
            PlaceUpperEdgeTiles(southChunk);
            chunk.South = southChunk;
            southChunk.North = chunk;
            southChunk.CheckLoadingState();
        }
        // Upper edge
        if (northChunk != null)
        {
            PlaceUpperEdgeTiles(chunk);
            PlaceLowerEdgeTiles(northChunk);
            chunk.North = northChunk;
            northChunk.South = chunk;
            northChunk.CheckLoadingState();
        }
        // Upper left corner
        if (northWestChunk != null)
        {
            chunk.NorthWest = northWestChunk;
            northWestChunk.SouthEast = chunk;
            northWestChunk.CheckLoadingState();
            if(northChunk != null && westChunk != null)
            {
                PlaceUpperLeftCornerTile(chunk);
                PlaceUpperRightCornerTile(westChunk);
                PlaceLowerRightCornerTile(northWestChunk);
                PlaceLowerLeftCornerTile(northChunk);
            }
        }
        // Upper right corner
        if (northEastChunk != null)
        {
            chunk.NorthEast = northEastChunk;
            northEastChunk.SouthWest = chunk;
            northEastChunk.CheckLoadingState();
            if(northChunk != null && eastChunk != null)
            {
                PlaceUpperRightCornerTile(chunk);
                PlaceUpperLeftCornerTile(eastChunk);
                PlaceLowerLeftCornerTile(northEastChunk);
                PlaceLowerRightCornerTile(northChunk);
            }
        }
        // Lower left corner
        if (southWestChunk != null)
        {
            chunk.SouthWest = southWestChunk;
            southWestChunk.NorthEast = chunk;
            southWestChunk.CheckLoadingState();
            if(southChunk != null && westChunk != null)
            {
                PlaceLowerLeftCornerTile(chunk);
                PlaceLowerRightCornerTile(westChunk);
                PlaceUpperRightCornerTile(southWestChunk);
                PlaceUpperLeftCornerTile(southChunk);
            }
        }
        // Lower right corner
        if (southEastChunk != null)
        {
            chunk.SouthEast = southEastChunk;
            southEastChunk.NorthWest = chunk;
            southEastChunk.CheckLoadingState();
            if(southChunk != null && eastChunk != null)
            {
                PlaceLowerRightCornerTile(chunk);
                PlaceLowerLeftCornerTile(eastChunk);
                PlaceUpperLeftCornerTile(southEastChunk);
                PlaceUpperRightCornerTile(southChunk);
            }
        }

        chunk.CheckLoadingState();
    }

    private void PlaceTile(TilemapChunk chunk, int x, int y)
    {
        int tileX = chunk.Coordinates.x * TilemapChunk.ChunkSize + x;
        int tileY = chunk.Coordinates.y * TilemapChunk.ChunkSize + y;
        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);

        // Base tilemap
        switch (chunk.Tiles[x, y])
        {
            case TileType.Grass:
                TilemapBase.SetTile(tilePos, GrassSet1.GetRandomTile());
                break;

            case TileType.Desert:
                TilemapBase.SetTile(tilePos, DesertSet1.GetRandomTile());
                break;

            case TileType.WoodFloor:
                TilemapBase.SetTile(tilePos, WoodFloorSet1.GetRandomTile());
                break;

            case TileType.Mountain:
                PlaceSlicedTile(tileX, tileY, WallSet1, TileType.Mountain);
                break;

            case TileType.Wall:
                PlaceSlicedTile(tileX, tileY, WallSet2, TileType.Wall);
                break;
        }

        // Region tilemap
        TilemapRegions.SetTile(tilePos, TestTile);
        TilemapRegions.SetTileFlags(tilePos, TileFlags.None);
        TilemapRegions.SetColor(tilePos, Voronoi.GetColorFor(chunk.Regions[x, y].Type));
    }

    #region Chunkedge Tiles
    private void PlaceLeftEdgeTiles(TilemapChunk chunk)
    {
        for(int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(chunk, 0, i);
        }
    }
    private void PlaceRightEdgeTiles(TilemapChunk chunk)
    {
        for (int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(chunk, TilemapChunk.ChunkSize - 1, i);
        }
    }
    private void PlaceLowerEdgeTiles(TilemapChunk chunk)
    {
        for (int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(chunk, i, 0);
        }
    }
    private void PlaceUpperEdgeTiles(TilemapChunk chunk)
    {
        for (int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(chunk, i, TilemapChunk.ChunkSize - 1);
        }
    }

    private void PlaceUpperLeftCornerTile(TilemapChunk chunk)
    {
        PlaceTile(chunk, 0, TilemapChunk.ChunkSize - 1);
    }
    private void PlaceUpperRightCornerTile(TilemapChunk chunk)
    {
        PlaceTile(chunk, TilemapChunk.ChunkSize - 1, TilemapChunk.ChunkSize - 1);
    }
    private void PlaceLowerLeftCornerTile(TilemapChunk chunk)
    {
        PlaceTile(chunk, 0, 0);
    }
    private void PlaceLowerRightCornerTile(TilemapChunk chunk)
    {
        PlaceTile(chunk, TilemapChunk.ChunkSize - 1, 0);
    }
    #endregion

    private void PlaceSlicedTile(int tileX, int tileY, TileSetSliced slicedSet, TileType connectionType)
    {
        TileBase tileToPlace;
        int tileRotation = 0;

        TileType up = GetTileType(tileX, tileY + 1);
        TileType down = GetTileType(tileX, tileY - 1);
        TileType left = GetTileType(tileX - 1, tileY);
        TileType right = GetTileType(tileX + 1, tileY);

        TileType upLeft = GetTileType(tileX - 1, tileY + 1);
        TileType upRight = GetTileType(tileX + 1, tileY + 1);
        TileType downLeft = GetTileType(tileX - 1, tileY - 1);
        TileType downRight = GetTileType(tileX + 1, tileY - 1);

        // Surrounded, Center0, Center1, Center2, Center3, Center4
        if (up == connectionType && down == connectionType && left == connectionType && right == connectionType) 
        {
            // Surrounded
            if (upRight == connectionType && upLeft == connectionType && downRight == connectionType && downLeft == connectionType)
            {
                tileToPlace = slicedSet.Surrounded;
            }

            // Center4
            else if (upRight == connectionType && upLeft == connectionType && downLeft == connectionType)
            {
                tileToPlace = slicedSet.Center4;
            }
            else if (upLeft == connectionType && downRight == connectionType && downLeft == connectionType)
            {
                tileToPlace = slicedSet.Center4;
                tileRotation = 90;
            }
            else if (upRight == connectionType && downRight == connectionType && downLeft == connectionType)
            {
                tileToPlace = slicedSet.Center4;
                tileRotation = 180;
            }
            else if(upRight == connectionType && upLeft == connectionType && downRight == connectionType)
            {
                tileToPlace = slicedSet.Center4;
                tileRotation = 270;
            }

            // Center2 
            else if (upLeft == connectionType && downLeft == connectionType)
            {
                tileToPlace = slicedSet.Center2;
            }
            else if (downLeft == connectionType && downRight == connectionType)
            {
                tileToPlace = slicedSet.Center2;
                tileRotation = 90;
            }
            else if (downRight == connectionType && upRight == connectionType)
            {
                tileToPlace = slicedSet.Center2;
                tileRotation = 180;
            }
            else if(upRight == connectionType && upLeft == connectionType)
            {
                tileToPlace = slicedSet.Center2;
                tileRotation = 270;
            }

            // Center3
            else if(upLeft == connectionType && downRight == connectionType)
            {
                tileToPlace = slicedSet.Center3;
            }
            else if (upRight == connectionType && downLeft == connectionType)
            {
                tileToPlace = slicedSet.Center3;
                tileRotation = 90;
            }

            // Center1
            else if(upLeft == connectionType)
            {
                tileToPlace = slicedSet.Center1;
            }
            else if (downLeft == connectionType)
            {
                tileToPlace = slicedSet.Center1;
                tileRotation = 90;
            }
            else if (downRight == connectionType)
            {
                tileToPlace = slicedSet.Center1;
                tileRotation = 180;
            }
            else if (upRight == connectionType)
            {
                tileToPlace = slicedSet.Center1;
                tileRotation = 270;
            }

            // Center0
            else
            {
                tileToPlace = slicedSet.Center0;
            }

        }

        // T0, T1, T2, T3
        else if (up == connectionType && down == connectionType && left == connectionType) // -|
        {
            if (upLeft == connectionType && downLeft == connectionType) tileToPlace = slicedSet.T3;
            else if (downLeft == connectionType) tileToPlace = slicedSet.T2;
            else if (upLeft == connectionType) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }
        else if (down == connectionType && left == connectionType && right == connectionType) // T
        {
            tileRotation = 90;
            if (downLeft == connectionType && downRight == connectionType) tileToPlace = slicedSet.T3;
            else if (downRight == connectionType) tileToPlace = slicedSet.T2;
            else if (downLeft == connectionType) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }
        else if (up == connectionType && down == connectionType && right == connectionType) // |-
        {
            tileRotation = 180;
            if (upRight == connectionType && downRight == connectionType) tileToPlace = slicedSet.T3;
            else if (upRight == connectionType) tileToPlace = slicedSet.T2;
            else if (downRight == connectionType) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }
        else if (up == connectionType && left == connectionType && right == connectionType) // l
        {
            tileRotation = 270;
            if (upLeft == connectionType && upRight == connectionType) tileToPlace = slicedSet.T3;
            else if (upLeft == connectionType) tileToPlace = slicedSet.T2;
            else if (upRight == connectionType) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }

        // Corner0, Corner1
        else if (left == connectionType && up == connectionType) // J
        {
            if (upLeft == connectionType) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }
        else if (left == connectionType && down == connectionType) // ¬
        {
            tileRotation = 90;
            if (downLeft == connectionType) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }
        else if (down == connectionType && right == connectionType) // L
        {
            tileRotation = 180;
            if (downRight == connectionType) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }

        else if (right == connectionType && up == connectionType) // r
        {
            tileRotation = 270;
            if (upRight == connectionType) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }

        // Straight
        else if (left == connectionType && right == connectionType)
        {
            tileToPlace = slicedSet.Straight;
        }
        else if (down == connectionType && up == connectionType)
        {
            tileToPlace = slicedSet.Straight;
            tileRotation = 90;
        }

        // End
        else if (left == connectionType)
        {
            tileToPlace = slicedSet.End;
            tileRotation = 180;
        }
        else if (down == connectionType)
        {
            tileToPlace = slicedSet.End;
            tileRotation = 270;
        }
        else if (right == connectionType)
        {
            tileToPlace = slicedSet.End;
        }
        else if (up == connectionType)
        {
            tileToPlace = slicedSet.End;
            tileRotation = 90;
        }

        // Single
        else tileToPlace = slicedSet.Single;

        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        TilemapBase.SetTile(tilePos, tileToPlace);
        if(tileRotation != 0) TilemapBase.SetTransformMatrix(tilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, tileRotation), Vector3.one));
    }

    private TileType GetTileType(int gridX, int gridY)
    {
        Vector2Int chunkCoordinates = GetChunkCoordinates(new Vector2Int(gridX, gridY));
        if (!Chunks.ContainsKey(chunkCoordinates)) return TileType.Grass;
        TilemapChunk chunk = Chunks[chunkCoordinates];

        int inChunkX = gridX - (chunkCoordinates.x * TilemapChunk.ChunkSize);
        int inChunkY = gridY - (chunkCoordinates.y * TilemapChunk.ChunkSize);
        //Debug.Log("chunk coordinates at " + gridX + "/" + gridY + " are " + chunkCoordinates.x + "/" + chunkCoordinates.y + " - " + inChunkX + "/" + inChunkY);
        return chunk.Tiles[inChunkX, inChunkY]; 
    }

    public Vector2Int GetChunkCoordinates(Vector2Int gridPosition)
    {
        int chunkX = gridPosition.x / TilemapChunk.ChunkSize;
        int chunkY = gridPosition.y / TilemapChunk.ChunkSize;
        if (gridPosition.x < 0 && gridPosition.x % TilemapChunk.ChunkSize != 0) chunkX--;
        if (gridPosition.y < 0 && gridPosition.y % TilemapChunk.ChunkSize != 0) chunkY--;
        return new Vector2Int(chunkX, chunkY);
    }
}
