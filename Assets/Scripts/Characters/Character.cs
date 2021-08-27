using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Character : MonoBehaviour
{
    public GameModel Model;

    // Attributes
    public string Name;
    public float MovementSpeed;

    // Controls
    public CharacterController Controller;
    public TileInfo CurrentTile;// The data of the exact tile the character is on at this moment
    public Vector2Int GridPosition; // The position of the character after he is done moving
    public Direction FaceDirection;

    // Body Parts
    public BodyPart Body;
    public BodyPart Head;

    // Relationships
    /// <summary>
    /// Relationships this character has towards other characters.
    /// </summary>
    public Dictionary<Character, Relationship> OutRelationships;

    /// <summary>
    /// Relationships other characters have towards this character.
    /// </summary>
    public Dictionary<Character, Relationship> InRelationships;

    public virtual void Init(GameModel model, Vector2Int position, CharacterController controller, Transform movePoint, BodyPart body, BodyPart head)
    {
        Model = model;
        GridPosition = position;
        FaceDirection = Direction.S;
        Controller = controller;
        Controller.Character = this;
        Controller.MovePoint = movePoint;
        Body = body;
        Head = head;
        OutRelationships = new Dictionary<Character, Relationship>();
        InRelationships = new Dictionary<Character, Relationship>();

        transform.position = model.TilemapGenerator.GetWorldPosition(position);
        CurrentTile = Model.TilemapGenerator.GetTileInfo(position);
        CurrentTile.Character = this;

        MovementSpeed = 4f;
    }

    public TileInfo GetFacedTile()
    {
        if (FaceDirection == Direction.N) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(0, 1));
        if (FaceDirection == Direction.E) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(1, 0));
        if (FaceDirection == Direction.S) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(0, -1));
        if (FaceDirection == Direction.W) return Model.TilemapGenerator.GetTileInfo(GridPosition + new Vector2Int(-1, 0));
        throw new System.Exception();
    }

    protected void FacePosition(Vector2Int position)
    {
        int xDistance = position.x - GridPosition.x;
        int yDistance = position.y - GridPosition.y;
        if(Mathf.Abs(xDistance) > Mathf.Abs(yDistance))
        {
            if (xDistance > 0) FaceDirection = Direction.E;
            else FaceDirection = Direction.W;
        }
        else
        {
            if (yDistance > 0) FaceDirection = Direction.N;
            else FaceDirection = Direction.S;
        }
    }

}
