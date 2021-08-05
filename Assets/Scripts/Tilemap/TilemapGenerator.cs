using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [Header("Tiles")]
    public TileSetSimple GrassSet1;
    public TileSetSliced WallSet1;

    private Dictionary<Vector2Int, TilemapChunk> Chunks = new Dictionary<Vector2Int, TilemapChunk>();

    public int MinGridX, MinGridY, MaxGridX, MaxGridY;

    public void GenerateTilemap(Tilemap tilemap, int chunkSize)
    {
        for(int y = -chunkSize / 2; y < (-chunkSize / 2) + chunkSize; y++)
        {
            for(int x = -chunkSize / 2; x < (-chunkSize / 2) + chunkSize; x++)
            {
                TilemapChunk chunk = GenerateChunk(new Vector2Int(x, y));
                GenerateMapLayout(chunk);
                PlaceTiles(tilemap, chunk);
            }
        }
    }

    /// <summary>
    /// Creates a chunk at the given coordinates if it doesn't already exist.
    /// </summary>
    public void TryCreateChunk(Tilemap tilemap, Vector2Int chunkCoordinates)
    {
        if(!Chunks.ContainsKey(chunkCoordinates))
        {
            TilemapChunk chunk = GenerateChunk(chunkCoordinates);
            GenerateMapLayout(chunk);
            PlaceTiles(tilemap, chunk);
        }
    }

    private TilemapChunk GenerateChunk(Vector2Int chunkCoordinates)
    {
        TilemapChunk chunk = new TilemapChunk(chunkCoordinates);
        if (chunk.MinGridX < MinGridX) MinGridX = chunk.MinGridX;
        if (chunk.MaxGridX > MaxGridX) MaxGridX = chunk.MaxGridX;
        if (chunk.MinGridY < MinGridY) MinGridY = chunk.MinGridY;
        if (chunk.MaxGridY > MaxGridY) MaxGridY = chunk.MaxGridY;
        //Debug.Log("adding chunk at " + chunkCoordinates);
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
                chunk.Tiles[x, y] = GetRandomTileType();
            }
        }
    }

    // Places the actual tiles for a chunk on the map according to a previously generated layout
    private void PlaceTiles(Tilemap tilemap, TilemapChunk chunk)
    {
        // Chunk center
        for (int y = 1; y < TilemapChunk.ChunkSize - 1; y++)
        {
            for(int x = 1; x < TilemapChunk.ChunkSize - 1; x++)
            {
                PlaceTile(tilemap, chunk, x, y);
            }
        }

        // Edge tiles (they are dependant on neighbouring chunks)
        TilemapChunk leftChunk, rightChunk, upperChunk, lowerChunk, upperRightChunk, upperLeftChunk, lowerRightChunk, lowerLeftChunk;

        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(-1, 0), out leftChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(1, 0), out rightChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(0, -1), out lowerChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(0, 1), out upperChunk);

        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(-1, 1), out upperLeftChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(1, 1), out upperRightChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(-1, -1), out lowerLeftChunk);
        Chunks.TryGetValue(chunk.Coordinates + new Vector2Int(1, -1), out lowerRightChunk);

        // Left edge
        if (leftChunk != null)
        {
            PlaceLeftEdgeTiles(tilemap, chunk);
            PlaceRightEdgeTiles(tilemap, leftChunk);
        }

        // Right edge
        if (rightChunk != null)
        {
            PlaceRightEdgeTiles(tilemap, chunk);
            PlaceLeftEdgeTiles(tilemap, rightChunk);
        }

        // Lower edge
        if (lowerChunk != null)
        {
            PlaceLowerEdgeTiles(tilemap, chunk);
            PlaceUpperEdgeTiles(tilemap, lowerChunk);
        }
        // Upper edge
        if (upperChunk != null)
        {
            PlaceUpperEdgeTiles(tilemap, chunk);
            PlaceLowerEdgeTiles(tilemap, upperChunk);
        }
        // Upper left corner
        if (upperChunk != null && leftChunk != null && upperLeftChunk != null)
        {
            PlaceUpperLeftCornerTile(tilemap, chunk);
            PlaceUpperRightCornerTile(tilemap, leftChunk);
            PlaceLowerRightCornerTile(tilemap, upperLeftChunk);
            PlaceLowerLeftCornerTile(tilemap, upperChunk);
        }
        // Upper right corner
        if (upperChunk != null && rightChunk != null && upperRightChunk != null)
        {
            PlaceUpperRightCornerTile(tilemap, chunk);
            PlaceUpperLeftCornerTile(tilemap, rightChunk);
            PlaceLowerLeftCornerTile(tilemap, upperRightChunk);
            PlaceLowerRightCornerTile(tilemap, upperChunk);
        }
        // Lower left corner
        if (lowerChunk != null && leftChunk != null && lowerLeftChunk != null)
        {
            PlaceLowerLeftCornerTile(tilemap, chunk);
            PlaceLowerRightCornerTile(tilemap, leftChunk);
            PlaceUpperRightCornerTile(tilemap, lowerLeftChunk);
            PlaceUpperLeftCornerTile(tilemap, lowerChunk);
        }
        // Lower right corner
        if (lowerChunk != null && rightChunk != null && lowerRightChunk != null)
        {
            PlaceLowerRightCornerTile(tilemap, chunk);
            PlaceLowerLeftCornerTile(tilemap, rightChunk);
            PlaceUpperLeftCornerTile(tilemap, lowerRightChunk);
            PlaceUpperRightCornerTile(tilemap, lowerChunk);
        }
    }

    private void PlaceTile(Tilemap tilemap, TilemapChunk chunk, int x, int y)
    {
        int tileX = chunk.Coordinates.x * TilemapChunk.ChunkSize + x;
        int tileY = chunk.Coordinates.y * TilemapChunk.ChunkSize + y;
        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);

        switch (chunk.Tiles[x, y])
        {
            case TileType.Ground:
                tilemap.SetTile(tilePos, GrassSet1.GetRandomTile());
                break;

            case TileType.Wall:
                PlaceSlicedTile(tilemap, tileX, tileY, WallSet1);
                break;
        }
    }

    #region Chunkedge Tiles
    private void PlaceLeftEdgeTiles(Tilemap tilemap, TilemapChunk chunk)
    {
        for(int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(tilemap, chunk, 0, i);
        }
    }
    private void PlaceRightEdgeTiles(Tilemap tilemap, TilemapChunk chunk)
    {
        for (int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(tilemap, chunk, TilemapChunk.ChunkSize - 1, i);
        }
    }
    private void PlaceLowerEdgeTiles(Tilemap tilemap, TilemapChunk chunk)
    {
        for (int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(tilemap, chunk, i, 0);
        }
    }
    private void PlaceUpperEdgeTiles(Tilemap tilemap, TilemapChunk chunk)
    {
        for (int i = 1; i < TilemapChunk.ChunkSize - 1; i++)
        {
            PlaceTile(tilemap, chunk, i, TilemapChunk.ChunkSize - 1);
        }
    }

    private void PlaceUpperLeftCornerTile(Tilemap tilemap, TilemapChunk chunk)
    {
        PlaceTile(tilemap, chunk, 0, TilemapChunk.ChunkSize - 1);
    }
    private void PlaceUpperRightCornerTile(Tilemap tilemap, TilemapChunk chunk)
    {
        PlaceTile(tilemap, chunk, TilemapChunk.ChunkSize - 1, TilemapChunk.ChunkSize - 1);
    }
    private void PlaceLowerLeftCornerTile(Tilemap tilemap, TilemapChunk chunk)
    {
        PlaceTile(tilemap, chunk, 0, 0);
    }
    private void PlaceLowerRightCornerTile(Tilemap tilemap, TilemapChunk chunk)
    {
        PlaceTile(tilemap, chunk, TilemapChunk.ChunkSize - 1, 0);
    }
    #endregion

    private void PlaceSlicedTile(Tilemap tilemap, int tileX, int tileY, TileSetSliced slicedSet)
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

        TileType type = TileType.Wall;

        // Surrounded, Center0, Center1, Center2, Center3, Center4
        if (up == type && down == type && left == type && right == type) 
        {
            // Surrounded
            if (upRight == type && upLeft == type && downRight == type && downLeft == type)
            {
                tileToPlace = slicedSet.Surrounded;
            }

            // Center4
            else if (upRight == type && upLeft == type && downLeft == type)
            {
                tileToPlace = slicedSet.Center4;
            }
            else if (upLeft == type && downRight == type && downLeft == type)
            {
                tileToPlace = slicedSet.Center4;
                tileRotation = 90;
            }
            else if (upRight == type && downRight == type && downLeft == type)
            {
                tileToPlace = slicedSet.Center4;
                tileRotation = 180;
            }
            else if(upRight == type && upLeft == type && downRight == type)
            {
                tileToPlace = slicedSet.Center4;
                tileRotation = 270;
            }

            // Center2 
            else if (upLeft == type && downLeft == type)
            {
                tileToPlace = slicedSet.Center2;
            }
            else if (downLeft == type && downRight == type)
            {
                tileToPlace = slicedSet.Center2;
                tileRotation = 90;
            }
            else if (downRight == type && upRight == type)
            {
                tileToPlace = slicedSet.Center2;
                tileRotation = 180;
            }
            else if(upRight == type && upLeft == type)
            {
                tileToPlace = slicedSet.Center2;
                tileRotation = 270;
            }

            // Center3
            else if(upLeft == type && downRight == type)
            {
                tileToPlace = slicedSet.Center3;
            }
            else if (upRight == type && downLeft == type)
            {
                tileToPlace = slicedSet.Center3;
                tileRotation = 90;
            }

            // Center1
            else if(upLeft == type)
            {
                tileToPlace = slicedSet.Center1;
            }
            else if (downLeft == type)
            {
                tileToPlace = slicedSet.Center1;
                tileRotation = 90;
            }
            else if (downRight == type)
            {
                tileToPlace = slicedSet.Center1;
                tileRotation = 180;
            }
            else if (upRight == type)
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
        else if (up == type && down == type && left == type) // -|
        {
            if (upLeft == type && downLeft == type) tileToPlace = slicedSet.T3;
            else if (downLeft == type) tileToPlace = slicedSet.T2;
            else if (upLeft == type) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }
        else if (down == type && left == type && right == type) // T
        {
            tileRotation = 90;
            if (downLeft == type && downRight == type) tileToPlace = slicedSet.T3;
            else if (downRight == type) tileToPlace = slicedSet.T2;
            else if (downLeft == type) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }
        else if (up == type && down == type && right == type) // |-
        {
            tileRotation = 180;
            if (upRight == type && downRight == type) tileToPlace = slicedSet.T3;
            else if (upRight == type) tileToPlace = slicedSet.T2;
            else if (downRight == type) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }
        else if (up == type && left == type && right == type) // l
        {
            tileRotation = 270;
            if (upLeft == type && upRight == type) tileToPlace = slicedSet.T3;
            else if (upLeft == type) tileToPlace = slicedSet.T2;
            else if (upRight == type) tileToPlace = slicedSet.T1;
            else tileToPlace = slicedSet.T0;
        }

        // Corner0, Corner1
        else if (left == type && up == type) // J
        {
            if (upLeft == type) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }
        else if (left == type && down == type) // ¬
        {
            tileRotation = 90;
            if (downLeft == type) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }
        else if (down == type && right == type) // L
        {
            tileRotation = 180;
            if (downRight == type) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }

        else if (right == type && up == type) // r
        {
            tileRotation = 270;
            if (upRight == type) tileToPlace = slicedSet.Corner1;
            else tileToPlace = slicedSet.Corner0;
        }

        // Straight
        else if (left == type && right == type)
        {
            tileToPlace = slicedSet.Straight;
        }
        else if (down == type && up == type)
        {
            tileToPlace = slicedSet.Straight;
            tileRotation = 90;
        }

        // End
        else if (left == type)
        {
            tileToPlace = slicedSet.End;
            tileRotation = 180;
        }
        else if (down == type)
        {
            tileToPlace = slicedSet.End;
            tileRotation = 270;
        }
        else if (right == type)
        {
            tileToPlace = slicedSet.End;
        }
        else if (up == type)
        {
            tileToPlace = slicedSet.End;
            tileRotation = 90;
        }

        // Single
        else tileToPlace = slicedSet.Single;

        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        tilemap.SetTile(tilePos, tileToPlace);
        if(tileRotation != 0) tilemap.SetTransformMatrix(tilePos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, tileRotation), Vector3.one));
    }

    private TileType GetTileType(int gridX, int gridY)
    {
        Vector2Int chunkCoordinates = GetChunkCoordinates(new Vector2Int(gridX, gridY));
        if (!Chunks.ContainsKey(chunkCoordinates)) return TileType.Ground;
        TilemapChunk chunk = Chunks[chunkCoordinates];

        int inChunkX = gridX - (chunkCoordinates.x * TilemapChunk.ChunkSize);
        int inChunkY = gridY - (chunkCoordinates.y * TilemapChunk.ChunkSize);
        //Debug.Log("chunk coordinates at " + gridX + "/" + gridY + " are " + chunkCoordinates.x + "/" + chunkCoordinates.y + " - " + inChunkX + "/" + inChunkY);
        return chunk.Tiles[inChunkX, inChunkY]; 
    }

    private TileType GetRandomTileType()
    {
        float rng = Random.value;
        if (rng < 0.6f) return TileType.Ground;
        else return TileType.Wall;
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
