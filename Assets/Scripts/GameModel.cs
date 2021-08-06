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

    [Header("Map Rendering Attributes")]
    public static int MapRenderRange = 2; // Chunks up to this far away from the player are rendered
    public static int MapGenerationRange = 4; // Chunks up to this far away from the player are generated. This needs to be higher than the render range because already generated chunks can change when new neighbouring chunks are generated or influenced by a generation

    [Header("Game Elements")]
    public Player Player;

    [Header("Prefabs")]
    public Player PlayerPrefab;

    void Start()
    {
        TilemapGenerator.Init(this);
        TilemapGenerator.GenerateTilemap(MapGenerationRange + 1);
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
        TilemapGenerator.LoadChunksAroundPlayer(Player.GridPosition, MapRenderRange, MapGenerationRange);
    }


    private TileData GetTileData(int x, int y)
    {
        return TilemapManager.GetTileData(x, y);
    }
}
