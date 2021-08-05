using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A TilemapChunk consists of the layout data for a single map chunk. It is independent from the Tilemap object and it's job is to provide data which will be translated into the actual tilemap in the TilemapGenerator.
/// </summary>
public class TilemapChunk
{
    public static int ChunkSize = 12;

    public Vector2Int Coordinates;
    public TileType[,] Tiles;

    public int MinGridX, MinGridY, MaxGridX, MaxGridY;

    public TilemapChunk(Vector2Int coordinates)
    {
        Coordinates = coordinates;
        MinGridX = coordinates.x * ChunkSize;
        MaxGridX = coordinates.x * ChunkSize + ChunkSize - 1;
        MinGridY = coordinates.y * ChunkSize;
        MaxGridY = coordinates.y * ChunkSize + ChunkSize - 1;
        Tiles = new TileType[ChunkSize, ChunkSize];
    }
}
