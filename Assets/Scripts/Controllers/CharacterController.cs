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

    public Direction MoveDirection;

    public virtual void Awake()
    {
        Character = GetComponentInParent<Character>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Character.CurrentTile = Character.Model.TilemapManager.GetTileData(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, Character.MovementSpeed * Time.deltaTime * Character.CurrentTile.SpeedModifier);
        

        if (Vector3.Distance(transform.position, MovePoint.position) <= 0.05f)
        {
            if (MoveDirection == Direction.Left && Character.Model.TilemapManager.GetTileData(Character.GridPosition.x - 1, Character.GridPosition.y).Passable)
                Move(Direction.Left);
            else if (MoveDirection == Direction.Right && Character.Model.TilemapManager.GetTileData(Character.GridPosition.x + 1, Character.GridPosition.y).Passable)
                Move(Direction.Right);
            else if (MoveDirection == Direction.Up && Character.Model.TilemapManager.GetTileData(Character.GridPosition.x, Character.GridPosition.y + 1).Passable)
                Move(Direction.Up);
            else if (MoveDirection == Direction.Down && Character.Model.TilemapManager.GetTileData(Character.GridPosition.x, Character.GridPosition.y - 1).Passable)
                Move(Direction.Down);
        }
    }

    private void Move(Direction moveDirection)
    {
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

        ShowCharacterSide(moveDirection);
        OnCharacterMove();
    }

    private void ShowCharacterSide(Direction dir)
    {
        switch(dir)
        {
            case Direction.Left:
                Head.transform.localPosition = new Vector3(-0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Body.sortingOrder = 0;
                break;

            case Direction.Right:
                Head.transform.localPosition = new Vector3(0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Body.sortingOrder = 0;
                break;

            case Direction.Up:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Body.sortingOrder = 1;
                break;

            case Direction.Down:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Body.sortingOrder = 0;
                break;

        }
    }

    protected virtual void OnCharacterMove()
    {

    }
}
