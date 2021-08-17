using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : CharacterController
{
    public NPC NPC;
    public ActionState ActionState;

    protected override void GetCharacterMovement()
    {
        base.GetCharacterMovement();
        if (!IsMoving && ActionState == ActionState.Idle && Random.value < 0.002f) SetTargetPath(Pathfinder.GetPath(Character.Model.TilemapGenerator, Character.GridPosition, NPC.Home.GetRandomInsidePosition()));
    }
}
