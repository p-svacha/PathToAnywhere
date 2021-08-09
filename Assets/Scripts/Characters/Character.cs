using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Character : MonoBehaviour
{
    public GameModel Model;

    public TileInfo CurrentTile; // The data of the exact tile the character is on at this moment
    public Vector2Int GridPosition; // The position of the character after he is done moving
    public Direction FaceDirection;

    public float MovementSpeed;
    public CharacterController Controller;

    public virtual void Awake()
    {
        Controller = GetComponentInChildren<CharacterController>();
    }

    public void Init(GameModel model, int x, int y)
    {
        Model = model;
        GridPosition = new Vector2Int(x, y);
    }

    public TileInfo GetFacedTile()
    {
        if (FaceDirection == Direction.North) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(0, 1));
        if (FaceDirection == Direction.East) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(1, 0));
        if (FaceDirection == Direction.South) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(0, -1));
        if (FaceDirection == Direction.West) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(-1, 0));
        throw new System.Exception();
    }
}
