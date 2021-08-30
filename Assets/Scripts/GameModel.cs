using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameModel : MonoBehaviour
{
    [Header("Static Elements")]
    public UI_HUD UI;
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
        NameGenerator.Init();
        UI.Init(this);
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

    public void AddRelationship(Character source, Character target)
    {
        Relationship newRelationship = new Relationship(source, target);
        source.OutRelationships.Add(target, newRelationship);
        target.InRelationships.Add(source, newRelationship);
        UI.OnRelationshipUpdate();
    }

    public void ChangeAttitude(Character source, Character target, int value)
    {
        source.OutRelationships[target].Attitude += value; // Change attitude
        UI.OnRelationshipUpdate();
    }
}
