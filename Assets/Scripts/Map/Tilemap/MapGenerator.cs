using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MapGenerator
{
    public const int TilePixelSize = 64; // pixels

    // Tilemaps
    protected Tilemap TilemapBaseSurface;
    protected Blendmap SurfaceBlendMap;
    protected Tilemap TilemapBaseFeature;
    protected Tilemap TilemapOverlay;
    protected Tilemap TilemapRoof;
    protected Tilemap TilemapFrontOfPlayer1;
    protected Tilemap TilemapFrontOfPlayer2;

    // Textures
    public TileBase TestTile;

    public Texture2D SandSet1;
    public Texture2D GrassSet1;
    public Texture2D GrassSet2;
    public Texture2D DirtSet1;
    public Texture2D WaterSetSimple1;

    public Texture2D WoodFloorSet1;
    public Texture2D RoofSet1;
    public Texture2D MountainSet1;
    public Texture2D WallSetSliced1;
    public Texture2D RockSet1;
    public Texture2D WaterSet1;
    public Texture2D PathSet1;

    // Mapping tile types to tilesets
    public Dictionary<BaseSurfaceType, TileSetSimple> BaseSurfaceTilesets;
    public Dictionary<BaseFeatureType, TileSet> BaseFeatureTilesets;
    public Dictionary<RoofType, TileSetSliced> RoofTilesets;

    public MapGenerator()
    {
        // Load tilemaps from scene game objects
        TilemapBaseSurface = GameObject.Find("TilemapBaseSurface").GetComponent<Tilemap>();
        SurfaceBlendMap = GameObject.Find("SurfaceBlendMap").GetComponent<Blendmap>();
        TilemapBaseFeature = GameObject.Find("TilemapBaseFeature").GetComponent<Tilemap>();
        TilemapOverlay = GameObject.Find("TilemapOverlay").GetComponent<Tilemap>();
        TilemapRoof = GameObject.Find("TilemapRoof").GetComponent<Tilemap>();
        TilemapFrontOfPlayer1 = GameObject.Find("TilemapFrontOfPlayer1").GetComponent<Tilemap>();
        TilemapFrontOfPlayer2 = GameObject.Find("TilemapFrontOfPlayer2").GetComponent<Tilemap>();

        // Load textures for tilesets from resources
        TestTile = Resources.Load<TileBase>("Tilesets/DebugTile");
        SandSet1 = Resources.Load<Texture2D>("Tilesets/Simple/SandSet1");
        GrassSet1 = Resources.Load<Texture2D>("Tilesets/Simple/GrassSet1");
        GrassSet2 = Resources.Load<Texture2D>("Tilesets/Simple/GrassSet2");
        DirtSet1 = Resources.Load<Texture2D>("Tilesets/Simple/DirtSet1");
        WoodFloorSet1 = Resources.Load<Texture2D>("Tilesets/Simple/WoodFloorSet1");
        WaterSetSimple1 = Resources.Load<Texture2D>("Tilesets/Simple/WaterSet1");

        RockSet1 = Resources.Load<Texture2D>("Objects/RockSet1");

        RoofSet1 = Resources.Load<Texture2D>("Tilesets/Sliced/RoofSet1");
        MountainSet1 = Resources.Load<Texture2D>("Tilesets/Sliced/MountainSet1");
        WallSetSliced1 = Resources.Load<Texture2D>("Tilesets/Sliced/WallSet1");
        PathSet1 = Resources.Load<Texture2D>("Tilesets/Sliced/PathSet1");
        WaterSet1 = Resources.Load<Texture2D>("Tilesets/Sliced/WaterSet1");

        // Create datas that are used to for tilesets
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
        BaseSurfaceTilesets.Add(BaseSurfaceType.Water, TileGenerator.GenerateSimpleTilset(WaterSetSimple1, wall, TilePixelSize, 2, 2));

        BaseFeatureTilesets.Add(BaseFeatureType.Floor, TileGenerator.GenerateSimpleTilset(WoodFloorSet1, ground, TilePixelSize, 1, 1));
        BaseFeatureTilesets.Add(BaseFeatureType.Rock, TileGenerator.GenerateSimpleTilset(RockSet1, wall, TilePixelSize, 1, 2));
        BaseFeatureTilesets.Add(BaseFeatureType.Mountain, TileGenerator.GenerateSlicedTileset(MountainSet1, wall, TilePixelSize, new List<BaseFeatureType>() { BaseFeatureType.Mountain }));
        BaseFeatureTilesets.Add(BaseFeatureType.Wall, TileGenerator.GenerateSlicedTileset(WallSetSliced1, wall, TilePixelSize, new List<BaseFeatureType>() { BaseFeatureType.Wall }));
        BaseFeatureTilesets.Add(BaseFeatureType.Water, TileGenerator.GenerateAnimatedSlicedTileset(WaterSet1, wall, TilePixelSize, new List<BaseFeatureType>() { BaseFeatureType.Water, BaseFeatureType.Mountain }, 1f));
        BaseFeatureTilesets.Add(BaseFeatureType.Path, TileGenerator.GenerateSlicedTileset(PathSet1, ground, TilePixelSize, new List<BaseFeatureType>() { BaseFeatureType.Path, BaseFeatureType.Floor }));

        RoofTilesets.Add(RoofType.DefaultRoof, TileGenerator.GenerateSlicedTileset(RoofSet1, null, TilePixelSize, new List<BaseFeatureType>()));
    }

    public TileInfo GetTile(int x, int y)
    {
        return GetTile(new Vector2Int(x, y));
    }
    public abstract TileInfo GetTile(Vector2Int cellCoordinates);

    /// <summary>
    /// Places a tile on the tilemap at the given coordinates based on the TileInfo at that position.
    /// </summary>
    protected void PlaceTile(int x, int y)
    {
        Vector3Int tilePos = new Vector3Int(x, y, 0);
        TileInfo tile = GetTile(x, y);

        // Base surface tilemap
        BaseSurfaceType baseSurfaceType = tile.BaseSurfaceType;
        if (baseSurfaceType != BaseSurfaceType.None)
        {
            BaseSurfaceTilesets[baseSurfaceType].PlaceTile(this, TilemapBaseSurface, tilePos);
            SurfaceBlendMap.BlendTile(this, x, y, baseSurfaceType, BaseSurfaceTilesets[baseSurfaceType]);
            SetTileColor(TilemapBaseSurface, tilePos, tile.BaseSurfaceColor);
        }


        // Base feature tilemap
        BaseFeatureType baseFeatureType = tile.BaseFeatureType;
        if (baseFeatureType != BaseFeatureType.None)
        {
            BaseFeatureTilesets[baseFeatureType].PlaceTile(this, TilemapBaseFeature, tilePos);
            SetTileColor(TilemapBaseFeature, tilePos, tile.BaseFeautureColor);
        }
    }

    private void SetTileColor(Tilemap tilemap, Vector3Int tilePos, Color? c)
    {
        if (c != null)
        {
            tilemap.SetTileFlags(tilePos, TileFlags.None);
            tilemap.SetColor(tilePos, (Color)c);
        }
    }
}
