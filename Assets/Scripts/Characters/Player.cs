using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerController PlayerController;


    public override void Init(GameModel model, Vector2Int position, CharacterController controller, Transform movePoint, BodyPart body, BodyPart head)
    {
        base.Init(model, position, controller, movePoint, body, head);
        PlayerController = (PlayerController)controller;
        PlayerController.Player = this;
    }
}
