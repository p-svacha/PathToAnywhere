using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerController PlayerController;

    public override void Init(GameModel model, Vector2Int position, CharacterController controller, Transform movePoint, CharacterAppearance appearance)
    {
        base.Init(model, position, controller, movePoint, appearance);
        PlayerController = (PlayerController)controller;
        PlayerController.Player = this;
        if(CurrentTile.Building != null) CurrentTile.Building.SetDrawRoof(Model.TilemapGenerator, false);
    }
}
