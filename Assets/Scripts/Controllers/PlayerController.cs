using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    private Player Player;

    private float DirectionChangeTolerance = 0.12f; // When a direcion is pressed for less than this amount, then the player only changes facing direction instead of moving
    private float TimeSinceLastDirectionChange; // Time since last direction change

    public override void Awake()
    {
        base.Awake();
        Player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    protected override void GetCharacterMovement()
    {
        if (Input.GetAxisRaw("Horizontal") == 1f) OnInputAxis(Direction.Right);
        else if (Input.GetAxisRaw("Horizontal") == -1f) OnInputAxis(Direction.Left);
        else if (Input.GetAxisRaw("Vertical") == 1f) OnInputAxis(Direction.Up);
        else if (Input.GetAxisRaw("Vertical") == -1f) OnInputAxis(Direction.Down);
        else MoveDirection = Direction.None;
    }

    private void OnInputAxis(Direction dir)
    {
        if (!IsMoving)
        {
            if (MoveDirection == Direction.None && dir != FaceDirection)
            {
                TimeSinceLastDirectionChange = 0f;
            }
            else
            {
                if (TimeSinceLastDirectionChange < DirectionChangeTolerance) TimeSinceLastDirectionChange += Time.deltaTime;
                else MoveDirection = dir;
            }
            FaceDirection = dir;
        }
    }

    protected override void OnCharacterMove(TileInfo from, TileInfo to)
    {
        Player.Model.OnPlayerMove();
        if(from.Building != to.Building)
        {
            if(from.Building != null) from.Building.SetDrawRoof(Player.Model.TilemapGenerator, true);
            if (to.Building != null) to.Building.SetDrawRoof(Player.Model.TilemapGenerator, false);
        }
        
    }
}
