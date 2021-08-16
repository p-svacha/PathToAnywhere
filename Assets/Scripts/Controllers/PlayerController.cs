using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    private Player Player;

    private float DirectionChangeTolerance = 0.12f; // When a direcion is pressed for less than this amount, then the player only changes facing direction instead of moving
    private float TimeSinceLastDirectionChange; // Time since last direction change

    private PlayerInputMode InputMode;

    public override void Awake()
    {
        base.Awake();
        Player = GetComponentInParent<Player>();
    }


    public override void Update()
    {
        base.Update();
        switch(InputMode)
        {
            case PlayerInputMode.Movement:
                if(!IsMoving && Input.GetKeyDown(KeyCode.Space))
                {
                    Player.Model.Interact();
                    InputMode = PlayerInputMode.Interaction;
                }
                break;

            case PlayerInputMode.Interaction:
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Player.Model.EndInteraction();
                    InputMode = PlayerInputMode.Movement;
                }
                break;
        }
    }

    protected override void GetCharacterMovement()
    {
        if (InputMode == PlayerInputMode.Movement)
        {
            if (Input.GetAxisRaw("Horizontal") == 1f) OnInputAxis(Direction.E);
            else if (Input.GetAxisRaw("Horizontal") == -1f) OnInputAxis(Direction.W);
            else if (Input.GetAxisRaw("Vertical") == 1f) OnInputAxis(Direction.N);
            else if (Input.GetAxisRaw("Vertical") == -1f) OnInputAxis(Direction.S);
            else MoveDirection = Direction.None;
        }
    }

    private void OnInputAxis(Direction dir)
    {
        if (!IsMoving)
        {
            if (MoveDirection == Direction.None && dir != Player.FaceDirection)
            {
                TimeSinceLastDirectionChange = 0f;
            }
            else
            {
                if (TimeSinceLastDirectionChange < DirectionChangeTolerance) TimeSinceLastDirectionChange += Time.deltaTime;
                else MoveDirection = dir;
            }
            Player.FaceDirection = dir;
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
