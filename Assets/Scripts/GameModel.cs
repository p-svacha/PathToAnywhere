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

    [Header("Map Rendering Attributes")]
    public static int MapRenderRange = 2;

    [Header("Game Elements")]
    public Player Player;

    [Header("Prefabs")]
    public Player PlayerPrefab;

    void Start()
    {
        TilemapGenerator.Init(this);
        TilemapGenerator.LoadChunksAroundPlayer(new Vector2Int(0,0), MapRenderRange);
        SpawnPlayer(0, 0);
        CameraController.FocusObject(Player.Controller.transform);
    }

    private void SpawnPlayer(int x, int y)
    {
        int curX = x;
        while (!TilemapGenerator.GetTileInfo(curX, y).Passable) curX++;
        Vector3 cellPos = Tilemap.GetCellCenterWorld(new Vector3Int(curX, y, 1));
        Vector3 worldSpawn = cellPos + new Vector3(0, 0, -1f);
        Player = Instantiate(PlayerPrefab);
        Player.Init(this, curX, y);
        Player.transform.position = worldSpawn;
    }

    public void OnPlayerMove()
    {
        TilemapGenerator.LoadChunksAroundPlayer(Player.GridPosition, MapRenderRange);
    }
}
