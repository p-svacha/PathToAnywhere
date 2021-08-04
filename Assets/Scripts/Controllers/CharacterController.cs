using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Character Character;
    public Transform MovePoint;

    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public Direction MoveDirection;


    void Awake()
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
            {
                Character.GridPosition.x -= 1;
                MovePoint.position += new Vector3(-1, 0, 0);
            }
            else if (MoveDirection == Direction.Right && Character.Model.TilemapManager.GetTileData(Character.GridPosition.x + 1, Character.GridPosition.y).Passable)
            {
                Character.GridPosition.x += 1;
                MovePoint.position += new Vector3(1, 0, 0);
            }
            else if (MoveDirection == Direction.Up && Character.Model.TilemapManager.GetTileData(Character.GridPosition.x, Character.GridPosition.y + 1).Passable)
            {
                Character.GridPosition.y += 1;
                MovePoint.position += new Vector3(0, 1, 0);
            }
            else if (MoveDirection == Direction.Down && Character.Model.TilemapManager.GetTileData(Character.GridPosition.x, Character.GridPosition.y - 1).Passable)
            {
                Character.GridPosition.y -= 1;
                MovePoint.position += new Vector3(0, -1, 0);
            }
        }
    }
}
