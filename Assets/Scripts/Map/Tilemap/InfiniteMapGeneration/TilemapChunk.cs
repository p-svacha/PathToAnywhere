using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapGeneration.Infinite
{
    /// <summary>
    /// A TilemapChunk consists of the layout data for a single map chunk. It is independent from the Tilemap object and it's job is to provide data which will be translated into the actual tilemap in the TilemapGenerator.
    /// </summary>
    public class TilemapChunk
    {
        public static int ChunkSize = 12;

        public Vector2Int Coordinates;
        public TileInfo[,] Tiles;

        public List<Region> RegionList;

        // Loading state
        public TilemapChunk NorthWest, North, NorthEast, East, SouthEast, South, SouthWest, West;
        public ChunkLoadingState State;

        public TilemapChunk(Vector2Int coordinates)
        {
            Coordinates = coordinates;

            Tiles = new TileInfo[ChunkSize, ChunkSize];
            for (int y = 0; y < ChunkSize; y++)
                for (int x = 0; x < ChunkSize; x++)
                    Tiles[x, y] = new TileInfo(new Vector2Int(Coordinates.x * ChunkSize + x, Coordinates.y * ChunkSize + y));


            State = ChunkLoadingState.RegionsGenerated;
        }

        public void OnRegionsGenerated()
        {
            RegionList = Tiles.Cast<TileInfo>().Select(x => x.Region).Distinct().ToList();
        }
    }
}
