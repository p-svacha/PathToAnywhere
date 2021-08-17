using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character Character;
    public Transform MovePoint;

    public bool IsMoving;
    public Direction MoveDirection;

    private List<Vector2Int> TargetPath = new List<Vector2Int>(); // The path the character will walk

    // Update is called once per frame
    public virtual void Update()
    {
        TileInfo currentTile = Character.Model.TilemapGenerator.GetTileInfo(transform.position);
        if (Character.CurrentTile != currentTile) Character.CurrentTile = currentTile;
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, Character.MovementSpeed * Time.deltaTime * Character.CurrentTile.GetSpeedModifier(Character.Model.TilemapGenerator));


        if (Vector3.Distance(transform.position, MovePoint.position) <= 0.05f) // Character is near the destination and next movement command can be given
        {
            IsMoving = false;

            GetCharacterMovement();

            if (MoveDirection == Direction.W && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x - 1, Character.GridPosition.y).IsPassable(Character.Model.TilemapGenerator))
                Move(Direction.W);
            else if (MoveDirection == Direction.E && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x + 1, Character.GridPosition.y).IsPassable(Character.Model.TilemapGenerator))
                Move(Direction.E);
            else if (MoveDirection == Direction.N && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x, Character.GridPosition.y + 1).IsPassable(Character.Model.TilemapGenerator))
                Move(Direction.N);
            else if (MoveDirection == Direction.S && Character.Model.TilemapGenerator.GetTileInfo(Character.GridPosition.x, Character.GridPosition.y - 1).IsPassable(Character.Model.TilemapGenerator))
                Move(Direction.S);
        }

        ShowCharacterSide(Character.FaceDirection);
    }

    protected virtual void GetCharacterMovement()
    {
        MoveDirection = Direction.None;
        FollowTargetPath();
    }

    protected void FollowTargetPath()
    {
        if (TargetPath.Count > 0)
        {
            if (TargetPath[0] == Character.GridPosition)
            {
                TargetPath.RemoveAt(0);
                if(TargetPath.Count > 0)
                {
                    Direction dir = GetDirectionTo(TargetPath[0]);
                    MoveDirection = dir;
                    Character.FaceDirection = dir;
                }
            }
        }
    }

    private Direction GetDirectionTo(Vector2Int pos)
    {
        if (pos == Character.GridPosition + new Vector2Int(1, 0)) return Direction.E;
        if (pos == Character.GridPosition + new Vector2Int(-1, 0)) return Direction.W;
        if (pos == Character.GridPosition + new Vector2Int(0, 1)) return Direction.N;
        if (pos == Character.GridPosition + new Vector2Int(0, -1)) return Direction.S;
        throw new System.Exception("Position is not adjacent to character position.");
    }

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
        Character.Body.ShowDirection(dir);
        Character.Head.ShowDirection(dir);

        switch (dir)
        {
            case Direction.W:
                Character.Head.SetLocalPoisition(new Vector3(-0.1f, 0.5f, 0f));
                Character.Head.SetSortingOrder(1);
                Character.Body.SetSortingOrder(0);
                break;

            case Direction.E:
                Character.Head.SetLocalPoisition(new Vector3(0.1f, 0.5f, 0f));
                Character.Head.SetSortingOrder(1);
                Character.Body.SetSortingOrder(0);
                break;

            case Direction.N:
                Character.Head.SetLocalPoisition(new Vector3(0f, 0.5f, 0f));
                Character.Head.SetSortingOrder(1);
                Character.Body.SetSortingOrder(2);
                break;

            case Direction.S:
                Character.Head.SetLocalPoisition(new Vector3(0f, 0.5f, 0f));
                Character.Head.SetSortingOrder(1);
                Character.Body.SetSortingOrder(0);
                break;

        }
    }

    protected virtual void OnCharacterMove(TileInfo from, TileInfo to)
    {
        from.Character = null;
        to.Character = Character;
    }

    public void SetTargetPath(List<Vector2Int> path)
    {
        //if(path.Count > 0) Debug.Log("Setting target path from " + path[0].ToString() + " to " + path[path.Count - 1].ToString() + " over " + path.Count + " tiles.");
        TargetPath = path;
    }
    public void ClearTargetPath()
    {
        TargetPath.Clear();
    }
}
