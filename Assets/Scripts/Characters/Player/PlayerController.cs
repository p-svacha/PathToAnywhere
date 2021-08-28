using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterController
{
    public Player Player;

    private bool WasMoving; // Flag used for fluid movement, so character doesn't always have to re-tap after changing direction
    private float DirectionChangeTolerance = 0.12f; // When a direcion is pressed for less than this amount, then the player only changes facing direction instead of moving
    private float TimeSinceLastDirectionChange; // Time since last direction change

    public PlayerInputMode InputMode;

    public override void Update()
    {
        base.Update();
        switch(InputMode)
        {
            case PlayerInputMode.Movement:
                if(!IsMoving && Input.GetKeyDown(KeyCode.Space))
                {
                    Player.Model.Interact();
                }
                break;

            case PlayerInputMode.Interaction:
                Player.Model.InteractionHandler.GetInteractionInput();
                break;
        }
    }

    protected override void GetCharacterMovement()
    {
        base.GetCharacterMovement();

        if (InputMode == PlayerInputMode.Movement)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TileInfo info = Player.Model.TilemapGenerator.GetTileInfo(mousePosition);
                SetTargetPath(Pathfinder.GetPath(Player.Model.TilemapGenerator, Player.GridPosition, info.Position));
            }

            if (Input.GetAxisRaw("Horizontal") == 1f) OnInputAxis(Direction.E);
            else if (Input.GetAxisRaw("Horizontal") == -1f) OnInputAxis(Direction.W);
            else if (Input.GetAxisRaw("Vertical") == 1f) OnInputAxis(Direction.N);
            else if (Input.GetAxisRaw("Vertical") == -1f) OnInputAxis(Direction.S);
            else WasMoving = false;
        }
    }

    private void OnInputAxis(Direction dir)
    {
        ClearTargetPath();
        if (!IsMoving)
        {
            if (!WasMoving && dir != Player.FaceDirection)
            {
                TimeSinceLastDirectionChange = 0f;
            }
            else
            {
                if (TimeSinceLastDirectionChange < DirectionChangeTolerance) TimeSinceLastDirectionChange += Time.deltaTime;
                else
                {
                    WasMoving = true;
                    MoveDirection = dir;
                }
            }
            Player.FaceDirection = dir;
        }
    }

    protected override void OnCharacterMove(TileInfo from, TileInfo to)
    {
        base.OnCharacterMove(from, to);
        Player.Model.OnPlayerMove();
        if(from.Building != to.Building)
        {
            if(from.Building != null) from.Building.SetDrawRoof(Player.Model.TilemapGenerator, true);
            if (to.Building != null) to.Building.SetDrawRoof(Player.Model.TilemapGenerator, false);
        }
    }
}
