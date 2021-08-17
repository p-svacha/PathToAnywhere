using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Region
{
    protected GameModel Model;
    protected TilemapGenerator MapGenerator;
    protected CharacterGenerator CharacterGenerator;
    public Vector2Int Id;
    public RegionType Type;
    public Color Color;
    public bool FullyLoaded;
    public List<TilemapChunk> Chunks;
    public List<Vector2Int> TilePositions;

    public Region(GameModel model, Vector2Int id)
    {
        Model = model;
        MapGenerator = model.TilemapGenerator;
        CharacterGenerator = model.CharacterGenerator;
        Id = id;
        Color = ColorManager.GetRandomGreenishColor();
        Chunks = new List<TilemapChunk>();
        TilePositions = new List<Vector2Int>();
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
