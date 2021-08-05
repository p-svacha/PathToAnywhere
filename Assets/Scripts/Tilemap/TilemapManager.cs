using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// The TilemapManager is a single instance class which is responsible for mapping data to single tiles and providing general tilemap functionality.
/// </summary>
public class TilemapManager : MonoBehaviour
{
    private Tilemap Tilemap;

    [SerializeField]
    private List<TileData> TileDatas;

    private Dictionary<TileBase, TileData> TileData;

    public void Init(Tilemap tilemap)
    {
        Tilemap = tilemap;
        TileData = new Dictionary<TileBase, TileData>();
        foreach(TileData tileData in TileDatas)
        {
            foreach(TileSet tileSet in tileData.TileSets)
            {
                foreach(TileBase tile in tileSet.GetTiles())
                {
                    TileData.Add(tile, tileData);
                }
            }
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = Tilemap.WorldToCell(mousePosition);

            TileBase clickedTile = Tilemap.GetTile(gridPosition);

            Debug.Log(gridPosition.x + "/" + gridPosition.y + ": " + clickedTile + " passable?: " + TileData[clickedTile].Passable);
        }
    }

    public Vector3Int GetGridPosition(Vector3 worldPosition)
    {
        return Tilemap.WorldToCell(worldPosition);
    }

    public TileBase GetTile(int gridX, int gridY)
    {
        Vector3Int gridPosition = new Vector3Int(gridX, gridY, 0);
        return Tilemap.GetTile(gridPosition);
    }
    public TileBase GetTile(Vector3 worldPosition)
    {
        Vector3Int gridPosition = GetGridPosition(worldPosition);
        return Tilemap.GetTile(gridPosition);
    }

    public TileData GetTileData(int gridX, int gridY)
    {
        TileBase tile = GetTile(gridX, gridY);
        return GetTileData(tile);
    }
    public TileData GetTileData(Vector3 worldPosition)
    {
        TileBase tile = GetTile(worldPosition);
        return GetTileData(tile);
    }
    public TileData GetTileData(TileBase tile)
    {
        return TileData[tile];
    }

}
