using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Character Character;
    public Transform MovePoint;

    public SpriteRenderer Head;
    public SpriteRenderer Body;

    public bool IsMoving;
    public Direction MoveDirection;

    public virtual void Awake()
    {
        Character = GetComponentInParent<Character>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Character.CurrentTile = Character.Model.TilemapGenerator.GetTileInfo(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, Character.MovementSpeed * Time.deltaTime * Character.CurrentTile.SpeedModifier);


        if (Vector3.Distance(transform.position, MovePoint.position) <= 0.05f) // Character is near the destination and next movement command can be given
        {
            IsMoving = false;

            GetCharacterMovement();

            if (MoveDirection == Direction.West && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x - 1, Character.GridPosition.y).Passable)
                Move(Direction.West);
            else if (MoveDirection == Direction.East && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x + 1, Character.GridPosition.y).Passable)
                Move(Direction.East);
            else if (MoveDirection == Direction.North && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x, Character.GridPosition.y + 1).Passable)
                Move(Direction.North);
            else if (MoveDirection == Direction.South && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x, Character.GridPosition.y - 1).Passable)
                Move(Direction.South);
        }

        ShowCharacterSide(Character.FaceDirection);
    }

    protected virtual void GetCharacterMovement() { }

    private void Move(Direction moveDirection)
    {
        IsMoving = true;

        Vector2Int fromPosition = Character.GridPosition;

        switch(moveDirection)
        {
            case Direction.West:
                Character.GridPosition.x -= 1;
                MovePoint.position += new Vector3(-1, 0, 0);
                break;

            case Direction.East:
                Character.GridPosition.x += 1;
                MovePoint.position += new Vector3(1, 0, 0);
                break;

            case Direction.North:
                Character.GridPosition.y += 1;
                MovePoint.position += new Vector3(0, 1, 0);
                break;

            case Direction.South:
                Character.GridPosition.y -= 1;
                MovePoint.position += new Vector3(0, -1, 0);
                break;
        }

        
        OnCharacterMove(Character.Model.TilemapGenerator.GetTileInfo(fromPosition), Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition));
    }

    private void ShowCharacterSide(Direction dir)
    {
        switch(dir)
        {
            case Direction.West:
                Head.transform.localPosition = new Vector3(-0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 10;
                break;

            case Direction.East:
                Head.transform.localPosition = new Vector3(0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 10;
                break;

            case Direction.North:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 12;
                break;

            case Direction.South:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 10;
                break;

        }
    }


    protected virtual void OnCharacterMove(TileInfo from, TileInfo to) // Add referenced from where to where character is moving1
    {
        
    }
}
