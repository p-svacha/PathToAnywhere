using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour
{
    public Camera InteractionCamera;
    private GameModel Model;
    private Player Player;

    public UI_InteractionBox InteractionBox;

    public void Init(GameModel model)
    {
        Model = model;
        Player = model.Player;
        InteractionBox.gameObject.SetActive(false);
    }

    public void Interact()
    {
        TileInfo targetTile = Player.GetFacedTile();
        Character targetCharacter = targetTile.Character;

        if(targetCharacter != null)
        {
            targetCharacter.FacePosition(Player.GridPosition);
            DisplayText(targetTile, targetCharacter.Name, "„Hey“");
        }
        else
        {
            string targetName = targetTile.BaseFeatureType != BaseFeatureType.None ? targetTile.BaseFeatureType.ToString() : targetTile.BaseSurfaceType.ToString();
            // Base
            string text = "";
            if (targetTile.BaseFeatureType != BaseFeatureType.None)
            {
                text += "You are looking at " + targetTile.BaseFeatureType.ToString();
                if (targetTile.BaseSurfaceType != BaseSurfaceType.None) text += " on " + targetTile.BaseSurfaceType.ToString();
            }
            else text += "You are looking at " + targetTile.BaseSurfaceType.ToString();
            
            // Overlay
            if (Model.TilemapGenerator.GetOverlayTile(targetTile.Position) != null) text += " with " + Model.TilemapGenerator.GetOverlayTile(targetTile.Position).name;
            text += ".";

            // Misc
            if (Player.CurrentTile.Building == null && targetTile.Building != null) text += " A building appears in front of you.";
            if (Player.CurrentTile.Building != null) text += " You are inside a building.";
            if (!targetTile.IsPassable(Model.TilemapGenerator)) text += " Something is blocking your way.";

            DisplayText(targetTile, targetName, text);
        }

    }

    public void DisplayText(TileInfo targetTile, string targetName, string text)
    {
        InteractionCamera.transform.position = Model.TilemapGenerator.GetWorldPosition(targetTile.Position) + new Vector3(0f, 0f, -1f);
        Player.PlayerController.InputMode = PlayerInputMode.Interaction;

        InteractionBox.gameObject.SetActive(true);
        InteractionBox.DisplayText(targetName, text);
    }

    public void EndInteraction()
    {
        Player.PlayerController.InputMode = PlayerInputMode.Movement;

        InteractionBox.gameObject.SetActive(false);
    }
}
