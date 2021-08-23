using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameModel : MonoBehaviour
{
    [Header("Static Elements")]
    public CameraController CameraController;
    public TilemapGenerator TilemapGenerator;
    public InteractionHandler InteractionHandler;
    public CharacterGenerator CharacterGenerator;

    [Header("Map Rendering Attributes")]
    public static int MapRenderRange = 2;

    [Header("Game Elements")]
    public Player Player;

    void Start()
    {
        CharacterGenerator.Init(this);
        TilemapGenerator.Init(this);
        TilemapGenerator.LoadChunksAroundPlayer(new Vector2Int(0,0), MapRenderRange);
        SpawnPlayer(0, 0);
        InteractionHandler.Init(this);
        CameraController.FocusObject(Player.Controller.transform);
    }

    private void SpawnPlayer(int x, int y)
    {
        int curX = x;
        while (!TilemapGenerator.GetTileInfo(curX, y).IsPassable(TilemapGenerator)) curX++;
        Player = CharacterGenerator.GeneratePlayer(new Vector2Int(curX, y));
    }

    public void OnPlayerMove()
    {
        TilemapGenerator.LoadChunksAroundPlayer(Player.GridPosition, MapRenderRange);
    }

    #region Interaction

    public void Interact()
    {
        InteractionHandler.Interact();
    }

    #endregion
}
