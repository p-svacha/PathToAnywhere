using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    private GameModel Model;

    [Header("Tilemaps")]
    public Tilemap TilemapBase;
    public Tilemap TilemapRegions;

    [Header("Tiles")]
    public Dictionary<TileType, TileSet> TileSets;

    private const int TilePixelSize = 64; // pixels

    public TileBase TestTile;

    public Texture2D DesertSet1;
    public Texture2D GrassSet1;
    public Texture2D WoodFloorSet1;

    public Texture2D MountainSet1;
    public Texture2D WallSet2;

    private Dictionary<Vector2Int, TilemapChunk> Chunks = new Dictionary<Vector2Int, TilemapChunk>();
    private List<TilemapChunk> LoadedChunks = new List<TilemapChunk>(); // Loaded chunks include all chanks that are within generation are of the player

    public int MinGridX, MinGridY, MaxGridX, MaxGridY;

    [Header("Regions")]
    private Voronoi Voronoi;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPosition = GetGridPosition(mousePosition);
            TileData data = GetTileData(gridPosition);
        }
    }

    public void Init(GameModel model)
    {
        Model = model;
        Voronoi = new Voronoi(this);
        TilemapRegions.gameObject.SetActive(false);

        // Tile datas
        TileSets = new Dictionary<TileType, TileSet>();
        TileData ground = new TileData(true, 1);
        TileData wall = new TileData(false, 0);

        // Generate tiles from textures
        TileGenerator.GenerateSimpleTilset(this, GrassSet1, TileType.Grass, ground, TilePixelSize);
        TileGenerator.GenerateSimpleTilset(this, DesertSet1, TileType.Desert, ground, TilePixelSize);
        TileGenerator.GenerateSimpleTilset(this, WoodFloorSet1, TileType.WoodFloor, ground, TilePixelSize);

        TileGenerator.GenerateSlicedTileset(this, MountainSet1, TileType.Mountain, wall, TilePixelSize);
        TileGenerator.GenerateSlicedTileset(this, WallSet2, TileType.Wall, wall, TilePixelSize);
    }

    /// <summary>
    /// Chunks that are relevant to the player are loaded. All regions that are within the visual range of the player will be fully loaded (as in all chunks that have tiles of this region)
    /// </summary>
    public void LoadChunksAroundPlayer(Vector2Int playerPosition, int visualRange)
    {
        Vector2Int playerChunkCoordinates = GetChunkCoordinates(playerPosition);
        for (int y = -visualRange; y <= visualRange; y++)
        {
            for (int x = -visualRange; x <= visualRange; x++)
            {
                TilemapChunk chunk = LoadChunk(playerChunkCoordinates + new Vector2Int(x, y), true);
                if (chunk.State == ChunkLoadingState.RegionsGenerated) PlaceTiles(chunk);
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
        if (chunk.MinGridX < MinGridX) MinGridX = chunk.MinGridX;
        if (chunk.MaxGridX > MaxGridX) MaxGridX = chunk.MaxGridX;
        if (chunk.MinGridY < MinGridY) MinGridY = chunk.MinGridY;
        if (chunk.MaxGridY > MaxGridY) MaxGridY = chunk.MaxGridY;
        Debug.Log("creating chunk at " + chunkCoordinates);
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
                Region region = Voronoi.GetRegionAt(gridPosition);
                chunk.Regions[x, y] = region;
                region.TilePositions.Add(gridPosition);
            }
        }

        chunk.OnRegionsGenerated();
    }

    /// <summary>
    /// Places the actual tiles for a chunk on the map according to a previously generated layout.
    /// Also places edge tiles on adjacent chunks that were waiting on information on neighbouring tiles.
    /// This method can change the loading state of neighbouring chunks by checking if all their neighbours are generated.
    /// </summary>
    private void PlaceTiles(TilemapChunk chunk)
    {
        chunk.State = ChunkLoadingState.Rendered;

        // Chunk center
        for (int y = 0; y < TilemapChunk.ChunkSize; y++)
        {
            for(int x = 0; x < TilemapChunk.ChunkSize; x++)
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
        
        // Base tilemap
        TileSets[chunk.Tiles[x, y]].PlaceTile(this, TilemapBase, tilePos);

        // Region tilemap
        TilemapRegions.SetTile(tilePos, TestTile);
        TilemapRegions.SetTileFlags(tilePos, TileFlags.None);
        TilemapRegions.SetColor(tilePos, chunk.Regions[x,y].Color);
    }

    public TileType GetTileType(int gridX, int gridY)
    {
        Vector2Int chunkCoordinates = GetChunkCoordinates(new Vector2Int(gridX, gridY));
        if (!Chunks.ContainsKey(chunkCoordinates)) return TileType.Grass;
        TilemapChunk chunk = Chunks[chunkCoordinates];

        int inChunkX = gridX - (chunkCoordinates.x * TilemapChunk.ChunkSize);
        int inChunkY = gridY - (chunkCoordinates.y * TilemapChunk.ChunkSize);
        return chunk.Tiles[inChunkX, inChunkY]; 
    }

    public void SetTileType(Vector2Int gridPosition, TileType type)
    {
        Vector2Int chunkCoordinates = GetChunkCoordinates(gridPosition);
        TilemapChunk chunk = Chunks[chunkCoordinates];

        int inChunkX = gridPosition.x - (chunkCoordinates.x * TilemapChunk.ChunkSize);
        int inChunkY = gridPosition.y - (chunkCoordinates.y * TilemapChunk.ChunkSize);
        chunk.Tiles[inChunkX, inChunkY] = type;
    }

   

    public Vector2Int GetChunkCoordinates(Vector2Int gridPosition)
    {
        int chunkX = gridPosition.x / TilemapChunk.ChunkSize;
        int chunkY = gridPosition.y / TilemapChunk.ChunkSize;
        if (gridPosition.x < 0 && gridPosition.x % TilemapChunk.ChunkSize != 0) chunkX--;
        if (gridPosition.y < 0 && gridPosition.y % TilemapChunk.ChunkSize != 0) chunkY--;
        return new Vector2Int(chunkX, chunkY);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        Vector3Int gridPos = TilemapBase.WorldToCell(worldPosition);
        return new Vector2Int(gridPos.x, gridPos.y);
    }

    public TileData GetTileData(Vector3 worldPosition)
    {
        Vector2Int gridPosition = GetGridPosition(worldPosition);
        return GetTileData(gridPosition);
    }
    public TileData GetTileData(Vector2Int gridPosition)
    {
        return GetTileData(gridPosition.x, gridPosition.y);
    }
    public TileData GetTileData(int gridX, int gridY)
    {
        return TileSets[GetTileType(gridX, gridY)].Data;
    }
}
