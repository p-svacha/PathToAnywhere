using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Character Character;
    public Transform MovePoint;

    public SpriteRenderer Head;
    public SpriteRenderer Body;

    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public bool IsMoving;
    public Direction FaceDirection;
    public Direction MoveDirection;

    public virtual void Awake()
    {
        Character = GetComponentInParent<Character>();
    }

    // Update is called once per frame
    public void Update()
    {
        Character.CurrentTile = Character.Model.TilemapGenerator.GetTileData(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, Character.MovementSpeed * Time.deltaTime * Character.CurrentTile.SpeedModifier);


        if (Vector3.Distance(transform.position, MovePoint.position) <= 0.05f) // Character is near the destination and next movement command can be given
        {
            IsMoving = false;

            GetCharacterMovement();

            if (MoveDirection == Direction.Left && Character.Model.TilemapGenerator.GetTileData(Character.GridPosition.x - 1, Character.GridPosition.y).Passable)
                Move(Direction.Left);
            else if (MoveDirection == Direction.Right && Character.Model.TilemapGenerator.GetTileData(Character.GridPosition.x + 1, Character.GridPosition.y).Passable)
                Move(Direction.Right);
            else if (MoveDirection == Direction.Up && Character.Model.TilemapGenerator.GetTileData(Character.GridPosition.x, Character.GridPosition.y + 1).Passable)
                Move(Direction.Up);
            else if (MoveDirection == Direction.Down && Character.Model.TilemapGenerator.GetTileData(Character.GridPosition.x, Character.GridPosition.y - 1).Passable)
                Move(Direction.Down);
        }

        ShowCharacterSide(FaceDirection);
    }

    protected virtual void GetCharacterMovement() { }

    private void Move(Direction moveDirection)
    {
        IsMoving = true;

        switch(moveDirection)
        {
            case Direction.Left:
                Character.GridPosition.x -= 1;
                MovePoint.position += new Vector3(-1, 0, 0);
                break;

            case Direction.Right:
                Character.GridPosition.x += 1;
                MovePoint.position += new Vector3(1, 0, 0);
                break;

            case Direction.Up:
                Character.GridPosition.y += 1;
                MovePoint.position += new Vector3(0, 1, 0);
                break;

            case Direction.Down:
                Character.GridPosition.y -= 1;
                MovePoint.position += new Vector3(0, -1, 0);
                break;
        }
        
        OnCharacterMove();
    }

    private void ShowCharacterSide(Direction dir)
    {
        switch(dir)
        {
            case Direction.Left:
                Head.transform.localPosition = new Vector3(-0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 10;
                break;

            case Direction.Right:
                Head.transform.localPosition = new Vector3(0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 10;
                break;

            case Direction.Up:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 12;
                break;

            case Direction.Down:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Head.sortingOrder = 11;
                Body.sortingOrder = 10;
                break;

        }
    }

    protected virtual void OnCharacterMove()
    {
        
    }
}
