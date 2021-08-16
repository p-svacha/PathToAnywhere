using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
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
        TileInfo facedTile = Player.GetFacedTile();
        string mainFeature = facedTile.BaseFeatureType != BaseFeatureType.None ? facedTile.BaseFeatureType.ToString() : facedTile.BaseSurfaceType.ToString();
        string text = "You are looking at " + mainFeature;
        if (Model.TilemapGenerator.GetOverlayTile(facedTile.Position) != null) text += " with " + Model.TilemapGenerator.GetOverlayTile(facedTile.Position).name;
        text += ".";
        if (Player.CurrentTile.Building == null && facedTile.Building != null) text += " A building appears in front of you.";
        if (Player.CurrentTile.Building != null) text += " You are inside a building.";
        if (!facedTile.Passable) text += " Something is blocking your way.";

        DisplayText(text);
    }

    public void DisplayText(string text)
    {
        InteractionBox.Text.text = text;
        InteractionBox.gameObject.SetActive(true);
    }

    public void HideInteractionBox()
    {
        InteractionBox.gameObject.SetActive(false);
    }
}
