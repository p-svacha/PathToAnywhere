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

            if (MoveDirection == Direction.W && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x - 1, Character.GridPosition.y).Passable)
                Move(Direction.W);
            else if (MoveDirection == Direction.E && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x + 1, Character.GridPosition.y).Passable)
                Move(Direction.E);
            else if (MoveDirection == Direction.N && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x, Character.GridPosition.y + 1).Passable)
                Move(Direction.N);
            else if (MoveDirection == Direction.S && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x, Character.GridPosition.y - 1).Passable)
                Move(Direction.S);
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
            case Direction.W:
                Character.GridPosition.x -= 1;
                MovePoint.position += new Vector3(-1, 0, 0);
                break;

            case Direction.E:
                Character.GridPosition.x += 1;
                MovePoint.position += new Vector3(1, 0, 0);
                break;

            case Direction.N:
                Character.GridPosition.y += 1;
                MovePoint.position += new Vector3(0, 1, 0);
                break;

            case Direction.S:
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
            case Direction.W:
                Head.transform.localPosition = new Vector3(-0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Head.sortingOrder = 12;
                Body.sortingOrder = 11;
                break;

            case Direction.E:
                Head.transform.localPosition = new Vector3(0.1f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(0.5f, 1f, 1f);
                Head.sortingOrder = 12;
                Body.sortingOrder = 11;
                break;

            case Direction.N:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Head.sortingOrder = 12;
                Body.sortingOrder = 13;
                break;

            case Direction.S:
                Head.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                Body.transform.localScale = new Vector3(1f, 1f, 1f);
                Head.sortingOrder = 12;
                Body.sortingOrder = 11;
                break;

        }
    }


    protected virtual void OnCharacterMove(TileInfo from, TileInfo to) // Add referenced from where to where character is moving1
    {
        
    }
}
