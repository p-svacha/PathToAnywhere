using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public const int TilePixelSize = 64; // pixels
    private GameModel Model;

    [Header("Tilemaps")]
    [SerializeField]
    private Tilemap TilemapBaseSurface;
    [SerializeField]
    private Blendmap SurfaceBlendMap;
    [SerializeField]
    private Tilemap TilemapBaseFeature;
    [SerializeField]
    private Tilemap TilemapOverlay;
    [SerializeField]
    private Tilemap TilemapRoof;
    [SerializeField]
    private Tilemap TilemapFrontOfPlayer1;
    [SerializeField]
    private Tilemap TilemapFrontOfPlayer2;

    [Header("Debug Tilemaps")]
    [SerializeField]
    private Tilemap TilemapRegions;
    [SerializeField]
    private Tilemap TilemapBuildings;

    [Header("Tiles")]
    public TileBase TestTile;

    public Texture2D SandSet1;
    public Texture2D GrassSet1;
    public Texture2D GrassSet2;
    public Texture2D DirtSet1;

    public Texture2D WoodFloorSet1;
    public Texture2D RoofSet1;
    public Texture2D MountainSet1;
    public Texture2D WallSet1;
    public Texture2D RockSet1;
    public Texture2D WaterSet1;

    public Dictionary<BaseSurfaceType, TileSetSimple> BaseSurfaceTilesets;
    public Dictionary<BaseFeatureType, TileSet> BaseFeatureTilesets;
    public Dictionary<RoofType, TileSetSliced> RoofTilesets;

    // Chunks
    private Dictionary<Vector2Int, TilemapChunk> Chunks = new Dictionary<Vector2Int, TilemapChunk>();

    // Regions
    private RegionPartitioner Voronoi;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPosition = GetGridPosition(mousePosition);
            TileInfo info = GetTileInfo(gridPosition.x, gridPosition.y);
        }
    }

    public void Init(GameModel model)
    {
        Model = model;
        Voronoi = new RegionPartitioner(model);
        TilemapRegions.gameObject.SetActive(false);
        TilemapBuildings.gameObject.SetActive(false);

        // Tile datas
        BaseSurfaceTilesets = new Dictionary<BaseSurfaceType, TileSetSimple>();
        BaseFeatureTilesets = new Dictionary<BaseFeatureType, TileSet>();
        RoofTilesets = new Dictionary<RoofType, TileSetSliced>();
        TileSetData ground = new TileSetData(true, 1);
        TileSetData wall = new TileSetData(false, 0);

        // Generate tiles from textures
        BaseSurfaceTilesets.Add(BaseSurfaceType.Grass1, TileGenerator.GenerateSimpleTilset(GrassSet1, ground, TilePixelSize, 2, 2));
        BaseSurfaceTilesets.Add(BaseSurfaceType.Grass2, TileGenerator.GenerateAnimatedSimpleTileset(GrassSet2, ground, TilePixelSize, 4, 2, 1.5f));
        BaseSurfaceTilesets.Add(BaseSurfaceType.Sand, TileGenerator.GenerateSimpleTilset(SandSet1, ground, TilePixelSize, 2, 2));
        BaseSurfaceTilesets.Add(BaseSurfaceType.Dirt, TileGenerator.GenerateSimpleTilset(DirtSet1, ground, TilePixelSize, 2, 2));

        BaseFeatureTilesets.Add(BaseFeatureType.Floor, TileGenerator.GenerateSimpleTilset(WoodFloorSet1, ground, TilePixelSize, 1, 1));
        BaseFeatureTilesets.Add(BaseFeatureType.Rock, TileGenerator.GenerateSimpleTilset(RockSet1, wall, TilePixelSize, 1, 2));
        BaseFeatureTilesets.Add(BaseFeatureType.Mountain, TileGenerator.GenerateSlicedTileset(MountainSet1, wall, TilePixelSize, new List<BaseFeatureType>() { BaseFeatureType.Mountain }));
        BaseFeatureTilesets.Add(BaseFeatureType.Wall, TileGenerator.GenerateSlicedTileset(WallSet1, wall, TilePixelSize, new List<BaseFeatureType>() { BaseFeatureType.Wall }));
        BaseFeatureTilesets.Add(BaseFeatureType.Water, TileGenerator.GenerateAnimatedSlicedTileset(WaterSet1, wall, TilePixelSize, new List<BaseFeatureType>() { BaseFeatureType.Water, BaseFeatureType.Mountain }, 1f));

        RoofTilesets.Add(RoofType.DefaultRoof, TileGenerator.GenerateSlicedTileset(RoofSet1, null, TilePixelSize, new List<BaseFeatureType>()));
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
                Region region = Voronoi.GetRegionAt(gridPosition);
                chunk.Tiles[x, y].Region = region;
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

        // Base surface tilemap
        BaseSurfaceType baseSurfaceType = chunk.Tiles[x, y].BaseSurfaceType;
        if (baseSurfaceType != BaseSurfaceType.None)
        {
            BaseSurfaceTilesets[baseSurfaceType].PlaceTile(this, TilemapBaseSurface, tilePos);
            SurfaceBlendMap.BlendTile(this, tileX, tileY, baseSurfaceType, BaseSurfaceTilesets[baseSurfaceType]);
        }

        // Base feature tilemap
        BaseFeatureType baseFeatureType = chunk.Tiles[x, y].BaseFeatureType;
        if(baseFeatureType != BaseFeatureType.None) BaseFeatureTilesets[baseFeatureType].PlaceTile(this, TilemapBaseFeature, tilePos);

        // Region debug tilemap
        TilemapRegions.SetTile(tilePos, TestTile);
        TilemapRegions.SetTileFlags(tilePos, TileFlags.None);
        TilemapRegions.SetColor(tilePos, chunk.Tiles[x,y].Region.Color);

        // Building debug tilemap
        if(chunk.Tiles[x, y].Building != null)
        {
            TilemapBuildings.SetTile(tilePos, TestTile);
            TilemapBuildings.SetTileFlags(tilePos, TileFlags.None);
            TilemapBuildings.SetColor(tilePos, chunk.Tiles[x, y].Building.Color);
        }

    }

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
        if (TilemapFrontOfPlayer1.GetTile(tilePos) == null ) TilemapFrontOfPlayer1.SetTile(tilePos, tile);
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
    public TileInfo GetTileInfo(Vector2Int gridPosition)
    {
        return GetTileInfo(gridPosition.x, gridPosition.y);
    }
    public TileInfo GetTileInfo(int gridX, int gridY)
    {
        Vector2Int chunkCoordinates = GetChunkCoordinates(gridX, gridY);
        TilemapChunk chunk = Chunks[chunkCoordinates];
        int inChunkX = gridX - (chunkCoordinates.x * TilemapChunk.ChunkSize);
        int inChunkY = gridY - (chunkCoordinates.y * TilemapChunk.ChunkSize);
        return chunk.Tiles[inChunkX, inChunkY]; // we need tileinfo here, not tilesetdata
    }

    public TileBase GetOverlayTile(Vector2Int gridPosition)
    {
        return TilemapOverlay.GetTile(new Vector3Int(gridPosition.x, gridPosition.y, 0));
    }

    #endregion
}
