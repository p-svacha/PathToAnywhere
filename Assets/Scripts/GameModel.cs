using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameModel : MonoBehaviour
{
    [Header("Static Elements")]
    public CameraController CameraController;
    public Tilemap Tilemap;
    public TilemapGenerator TilemapGenerator;
    public TilemapManager TilemapManager;

    [Header("Game Elements")]
    public Player Player;

    [Header("Prefabs")]
    public Player PlayerPrefab;

    void Start()
    {
        TilemapGenerator.GenerateTilemap(Tilemap, 4);
        TilemapManager.Init(Tilemap);
        SpawnPlayer(0, 0);
        CameraController.FocusObject(Player.Controller.transform);
    }

    private void SpawnPlayer(int x, int y)
    {
        int curX = x;
        while (!GetTileData(curX, y).Passable) curX++;
        Vector3 cellPos = Tilemap.GetCellCenterWorld(new Vector3Int(curX, y, 1));
        Vector3 worldSpawn = cellPos + new Vector3(0, 0, -1f);
        Player = Instantiate(PlayerPrefab);
        Player.Init(this, curX, y);
        Player.transform.position = worldSpawn;
    }

    public void OnPlayerMove()
    {
        LoadChunks();
    }

    private void LoadChunks()
    {
        Vector2Int playerChunkCoordinates = TilemapGenerator.GetChunkCoordinates(Player.GridPosition);
        for(int y = -1; y <= 1; y++)
        {
            for(int x = -1; x <= 1; x++)
            {
                TilemapGenerator.TryCreateChunk(Tilemap, playerChunkCoordinates + new Vector2Int(x, y));
            }
        }
    }

    private TileData GetTileData(int x, int y)
    {
        return TilemapManager.GetTileData(x, y);
    }
}
