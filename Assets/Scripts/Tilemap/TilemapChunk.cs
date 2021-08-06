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
    public Region[,] Regions;

    public int MinGridX, MinGridY, MaxGridX, MaxGridY;

    // Loading state
    public TilemapChunk NorthWest, North, NorthEast, East, SouthEast, South, SouthWest, West;
    public ChunkLoadingState State;

    public TilemapChunk(Vector2Int coordinates)
    {
        Coordinates = coordinates;
        MinGridX = coordinates.x * ChunkSize;
        MaxGridX = coordinates.x * ChunkSize + ChunkSize - 1;
        MinGridY = coordinates.y * ChunkSize;
        MaxGridY = coordinates.y * ChunkSize + ChunkSize - 1;

        Tiles = new TileType[ChunkSize, ChunkSize];
        Regions = new Region[ChunkSize, ChunkSize];

        State = ChunkLoadingState.WaitingForAllNeighbours;
    }

    public void CheckLoadingState()
    {
        if (NorthWest != null && North != null && NorthEast != null && East != null && SouthEast != null && South != null && SouthWest != null && West != null)
            AllNeighbourChunksLoaded();
    }

    private void AllNeighbourChunksLoaded()
    {
        Debug.Log("Chunk at " + Coordinates.x + "/" + Coordinates.y + " fully loaded");
        State = ChunkLoadingState.RenderReady;
    }
}
