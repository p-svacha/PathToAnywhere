using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    private Player Player;

    public override void Awake()
    {
        base.Awake();
        Player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1f) MoveDirection = Direction.Right;
        else if (Input.GetAxisRaw("Horizontal") == -1f) MoveDirection = Direction.Left;
        else if (Input.GetAxisRaw("Vertical") == 1f) MoveDirection = Direction.Up;
        else if (Input.GetAxisRaw("Vertical") == -1f) MoveDirection = Direction.Down;
        else MoveDirection = Direction.None;

        base.Update();
    }

    protected override void OnCharacterMove()
    {
        Player.Model.OnPlayerMove();
    }
}
