using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public NPCController NPCController;

    public Building Home;

    public override void Init(GameModel model, Vector2Int position, CharacterController controller, Transform movePoint, BodyPart body, BodyPart head)
    {
        base.Init(model, position, controller, movePoint, body, head);
        MovementSpeed = Random.Range(1f, 2f);
        NPCController = (NPCController)controller;
        NPCController.NPC = this;
    }

    public void StartInteractionWith(Character target)
    {
        NPCController.ClearTargetPath();
        NPCController.ActionState = ActionState.Interacting;
        FacePosition(target.GridPosition);
    }

    public void EndInteraction()
    {
        NPCController.ActionState = ActionState.Idle;
    }
}
