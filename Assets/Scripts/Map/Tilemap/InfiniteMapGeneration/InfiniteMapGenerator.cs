using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapGeneration.Infinite;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public class InfiniteMapGenerator : MapGenerator
    {
        // Debug tilemaps
        protected Tilemap TilemapDebugRegions;
        protected Tilemap TilemapDebugBuildings;

        // Infinite map data
        private RegionPartitioner RegionPartitioner;
        private Dictionary<Vector2Int, TilemapChunk> Chunks = new Dictionary<Vector2Int, TilemapChunk>();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int gridPosition = GetGridPosition(mousePosition);
                TileInfo info = GetTileInfo(gridPosition.x, gridPosition.y);
            }
        }

        public InfiniteMapGenerator() : base()
        {
            RegionPartitioner = new RegionPartitioner(this);

            TilemapDebugRegions = GameObject.Find("TilemapDebugRegions").GetComponent<Tilemap>();
            TilemapDebugBuildings = GameObject.Find("TilemapDebugBuildings").GetComponent<Tilemap>();
            TilemapDebugRegions.gameObject.SetActive(false);
            TilemapDebugBuildings.gameObject.SetActive(false);
        }

        /// <summary>
        /// Chunks that are relevant to the player are loaded. All regions that are within the visual range of the player will be fully loaded (as in all chunks that have tiles of this region)
        /// </summary>
        public void LoadChunksAroundPlayer(Vector2Int playerPosition, int visualRange)
        {
            Vector2Int playerChunkCoordinates = GetChunkCoordinates(playerPosition.x, playerPosition.y);
            for (int y = -visualRange; y <= visualRange; y++)
            {
                for (int x = -visualRange; x <= visualRange; x++)
                {
                    TilemapChunk chunk = LoadChunk(playerChunkCoordinates + new Vector2Int(x, y), true);
                    if (chunk.State == ChunkLoadingState.RegionsGenerated) PlaceChunkTiles(chunk);
                }
            }
        }

        /// <summary>
        /// Creates a chunk at the given coordinates if it doesn't already exist.
        /// Also creates all chunks containing a region that appears in the new chunk.
        /// </summary>
        public TilemapChunk LoadChunk(Vector2Int chunkCoordinates, bool loadRegions)
        {
            TilemapChunk chunk;
            Chunks.TryGetValue(chunkCoordinates, out chunk);
            bool generateNewChunk = (chunk == null);

            if (generateNewChunk)
            {
                chunk = GenerateChunk(chunkCoordinates);
                GenerateChunkRegions(chunk);
            }

            if (loadRegions)
            {
                foreach (Region region in chunk.RegionList)
                {
                    if (!region.FullyLoaded)
                    {
                        region.Chunks.Add(chunk);
                        LoadRegion(region, chunkCoordinates);
                        region.OnLoadingComplete();
                    }
                }
            }

            return chunk;
        }

        private void LoadRegion(Region region, Vector2Int chunkCoordinates)
        {
            CheckRegionChunk(region, chunkCoordinates + new Vector2Int(-1, 0));
            CheckRegionChunk(region, chunkCoordinates + new Vector2Int(1, 0));
            CheckRegionChunk(region, chunkCoordinates + new Vector2Int(0, -1));
            CheckRegionChunk(region, chunkCoordinates + new Vector2Int(0, 1));
        }

        private void CheckRegionChunk(Region region, Vector2Int coordinates)
        {
            TilemapChunk chunk = LoadChunk(coordinates, false);
            if (chunk.RegionList.Contains(region) && !region.Chunks.Contains(chunk))
            {
                region.Chunks.Add(chunk);
                LoadRegion(region, coordinates);
            }
        }

        private TilemapChunk GenerateChunk(Vector2Int chunkCoordinates)
        {
            TilemapChunk chunk = new TilemapChunk(chunkCoordinates);
            //Debug.Log("creating chunk at " + chunkCoordinates);
            Chunks.Add(chunkCoordinates, chunk);
            return chunk;
        }

        // Determines for each tile what region it is part of
        private void GenerateChunkRegions(TilemapChunk chunk)
        {
            for (int y = 0; y < TilemapChunk.ChunkSize; y++)
            {
                for (int x = 0; x < TilemapChunk.ChunkSize; x++)
                {
                    int gridX = chunk.Coordinates.x * TilemapChunk.ChunkSize + x;
                    int gridY = chunk.Coordinates.y * TilemapChunk.ChunkSize + y;
                    Vector2Int gridPosition = new Vector2Int(gridX, gridY);

                    // Get region
                    Region region = RegionPartitioner.GetRegionAt(gridPosition);
                    chunk.Tiles[x, y].Region = region;
                    region.TilePositions.Add(gridPosition);
                }
            }

            chunk.OnRegionsGenerated();
        }

        #region Place Chunk Tiles

        /// <summary>
        /// Places the actual tiles for a chunk on the map according to a previously generated layout.
        /// Also places edge tiles on adjacent chunks that were waiting on information on neighbouring tiles.
        /// This method can change the loading state of neighbouring chunks by checking if all their neighbours are generated.
        /// </summary>
        private void PlaceChunkTiles(TilemapChunk chunk)
        {
            chunk.State = ChunkLoadingState.Rendered;

            // Chunk center
            for (int y = 0; y < TilemapChunk.ChunkSize; y++)
            {
                for (int x = 0; x < TilemapChunk.ChunkSize; x++)
                {
                    PlaceTile(chunk, x, y);
                }
            }
        }

        private void PlaceTile(TilemapChunk chunk, int x, int y)
        {
            int tileX = chunk.Coordinates.x * TilemapChunk.ChunkSize + x;
            int tileY = chunk.Coordinates.y * TilemapChunk.ChunkSize + y;
            Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
            TileInfo tile = chunk.Tiles[x, y];

            PlaceTile(tileX, tileY);

            // Region debug tilemap
            TilemapDebugRegions.SetTile(tilePos, TestTile);
            TilemapDebugRegions.SetTileFlags(tilePos, TileFlags.None);
            TilemapDebugRegions.SetColor(tilePos, tile.Region.Color);

            // Building debug tilemap
            if (chunk.Tiles[x, y].Building != null)
            {
                TilemapDebugBuildings.SetTile(tilePos, TestTile);
                TilemapDebugBuildings.SetTileFlags(tilePos, TileFlags.None);
                TilemapDebugBuildings.SetColor(tilePos, tile.Building.DebugColor);
            }
        }

        

        #endregion

        #region Setters

        /// <summary>
        /// Sets the base surface type and maybe overrides the tile info (passability, speed modifier, etc) of the tile at the given position.
        /// </summary>
        public void SetBaseSurfaceType(Vector2Int gridPosition, BaseSurfaceType type)
        {
            Vector2Int chunkCoordinates = GetChunkCoordinates(gridPosition.x, gridPosition.y);
            TilemapChunk chunk = Chunks[chunkCoordinates];

            int inChunkX = gridPosition.x - (chunkCoordinates.x * TilemapChunk.ChunkSize);
            int inChunkY = gridPosition.y - (chunkCoordinates.y * TilemapChunk.ChunkSize);
            chunk.Tiles[inChunkX, inChunkY].BaseSurfaceType = type;
        }
        public void SetBaseSurfaceColor(Vector2Int gridPosition, Color? color)
        {
            Vector2Int chunkCoordinates = GetChunkCoordinates(gridPosition.x, gridPosition.y);
            TilemapChunk chunk = Chunks[chunkCoordinates];

            int inChunkX = gridPosition.x - (chunkCoordinates.x * TilemapChunk.ChunkSize);
            int inChunkY = gridPosition.y - (chunkCoordinates.y * TilemapChunk.ChunkSize);
            chunk.Tiles[inChunkX, inChunkY].BaseSurfaceColor = color;
        }

        /// <summary>
        /// Sets the base feature type and maybe overrides the tile info (passability, speed modifier, etc) of the tile at the given position.
        /// </summary>
        public void SetBaseFeatureType(Vector2Int gridPosition, BaseFeatureType type)
        {
            Vector2Int chunkCoordinates = GetChunkCoordinates(gridPosition.x, gridPosition.y);
            TilemapChunk chunk = Chunks[chunkCoordinates];

            int inChunkX = gridPosition.x - (chunkCoordinates.x * TilemapChunk.ChunkSize);
            int inChunkY = gridPosition.y - (chunkCoordinates.y * TilemapChunk.ChunkSize);
            chunk.Tiles[inChunkX, inChunkY].BaseFeatureType = type;
        }
        public void SetBaseFeatureColor(Vector2Int gridPosition, Color? color)
        {
            Vector2Int chunkCoordinates = GetChunkCoordinates(gridPosition.x, gridPosition.y);
            TilemapChunk chunk = Chunks[chunkCoordinates];

            int inChunkX = gridPosition.x - (chunkCoordinates.x * TilemapChunk.ChunkSize);
            int inChunkY = gridPosition.y - (chunkCoordinates.y * TilemapChunk.ChunkSize);
            chunk.Tiles[inChunkX, inChunkY].BaseFeautureColor = color;
        }


        public void SetOverlayTile(Vector2Int gridPosition, TileBase tile)
        {
            TilemapOverlay.SetTile(new Vector3Int(gridPosition.x, gridPosition.y, 0), tile);
        }

        public void SetRoofTile(Vector2Int gridPosition, TileBase tile)
        {
            TilemapRoof.SetTile(new Vector3Int(gridPosition.x, gridPosition.y, 0), tile);
        }

        public void SetFrontOfPlayerTile(Vector2Int gridPosition, TileBase tile)
        {
            Vector3Int tilePos = new Vector3Int(gridPosition.x, gridPosition.y, 0);
            if (TilemapFrontOfPlayer1.GetTile(tilePos) == null) TilemapFrontOfPlayer1.SetTile(tilePos, tile);
            else TilemapFrontOfPlayer2.SetTile(tilePos, tile);
        }

        #endregion

        #region Getters

        public Vector2Int GetChunkCoordinates(int gridX, int gridY)
        {
            int chunkX = gridX / TilemapChunk.ChunkSize;
            int chunkY = gridY / TilemapChunk.ChunkSize;
            if (gridX < 0 && gridX % TilemapChunk.ChunkSize != 0) chunkX--;
            if (gridY < 0 && gridY % TilemapChunk.ChunkSize != 0) chunkY--;
            return new Vector2Int(chunkX, chunkY);
        }

        public Vector3 GetWorldPosition(Vector2Int gridPosition)
        {
            return TilemapBaseSurface.GetCellCenterWorld(new Vector3Int(gridPosition.x, gridPosition.y, 1));
        }

        public Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            Vector3Int gridPos = TilemapBaseSurface.WorldToCell(worldPosition);
            return new Vector2Int(gridPos.x, gridPos.y);
        }

        public TileInfo GetTileInfo(Vector3 worldPosition)
        {
            Vector2Int gridPosition = GetGridPosition(worldPosition);
            return GetTileInfo(gridPosition.x, gridPosition.y);
        }
        public override TileInfo GetTile(Vector2Int cellCoordinates)
        {
            return GetTileInfo(cellCoordinates.x, cellCoordinates.y);
        }
        public TileInfo GetTileInfo(int gridX, int gridY)
        {
            Vector2Int chunkCoordinates = GetChunkCoordinates(gridX, gridY);
            TilemapChunk chunk = Chunks[chunkCoordinates];
            int inChunkX = gridX - (chunkCoordinates.x * TilemapChunk.ChunkSize);
            int inChunkY = gridY - (chunkCoordinates.y * TilemapChunk.ChunkSize);
            return chunk.Tiles[inChunkX, inChunkY];
        }

        public TileBase GetOverlayTile(Vector2Int gridPosition)
        {
            return TilemapOverlay.GetTile(new Vector3Int(gridPosition.x, gridPosition.y, 0));
        }

        #endregion
    }
}
