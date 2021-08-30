using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The HUD (Head-up-Display) is the part of the UI that the player always sees.
/// </summary>
public class UI_HUD : MonoBehaviour
{
    private GameModel Model;

    [Header("Elements")]
    public UI_HudIcon ObjectivesIcon;
    public UI_HudIcon RelationshipsIcon;
    public UI_HudIcon MapIcon;

    public void Init(GameModel model)
    {
        Model = model;
        ObjectivesIcon.Init(model);
        RelationshipsIcon.Init(model);
        MapIcon.Init(model);
    }

    public void OnRelationshipUpdate()
    {
        if (RelationshipsIcon.IsWindowActive) RelationshipsIcon.Window.Init(Model);
    }
}
