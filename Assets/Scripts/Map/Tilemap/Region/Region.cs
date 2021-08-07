using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Region
{
    protected TilemapGenerator Generator;
    public Vector2Int Id;
    public RegionType Type;
    public Color Color;
    public bool FullyLoaded;
    public List<TilemapChunk> Chunks;
    public List<Vector2Int> TilePositions;

    public Region(TilemapGenerator generator, Vector2Int id)
    {
        Generator = generator;
        Id = id;
        Color = GetRandomRegionColor();
        Chunks = new List<TilemapChunk>();
        TilePositions = new List<Vector2Int>();
    }

    private Color GetRandomRegionColor()
    {
        return new Color(Random.value * 0.5f, 0.3f + Random.value * 0.7f, 0.3f + Random.value * 0.7f);
    }

    public void OnLoadingComplete()
    {
        FullyLoaded = true;
        GenerateLayout();
    }

    /// <summary>
    /// All tiles within the region are known when this method is called.
    /// This method should set the TileType of all tiles in the region with Generator.SetTileType(...).
    /// </summary>
    protected abstract void GenerateLayout();
}
