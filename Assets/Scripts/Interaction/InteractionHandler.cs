using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour
{
    public Camera InteractionCamera;
    private GameModel Model;
    private Player Player;
    private NPC InteractionTargetNPC;

    public UI_InteractionBox InteractionBox;

    // Dialogue
    public Dialogue ActiveDialogue;
    public int CurrentDialogueOptionId;

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

        if(targetCharacter != null && targetCharacter.GetType() == typeof(NPC))
        {
            InteractionTargetNPC = (NPC)targetCharacter;
            Dialogue dialogue = InteractionTargetNPC.StartInteractionWith(Player);
            StartDialogue(dialogue);
        }

        /*
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
        */
    }

    public void GetInteractionInput()
    {
        int pressedNumber = GetPressedNumber();

        if (Input.GetKeyDown(KeyCode.Space)) // Select
        {
            ContinueInteraction(CurrentDialogueOptionId);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)) // Option down
        {
            CurrentDialogueOptionId++;
            if (CurrentDialogueOptionId >= ActiveDialogue.ActiveStep.DialogueOptions.Count) CurrentDialogueOptionId = 0;
            SetCurrentDialogueOptionId(CurrentDialogueOptionId);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) // Option up
        {
            CurrentDialogueOptionId--;
            if (CurrentDialogueOptionId < 0) CurrentDialogueOptionId = ActiveDialogue.ActiveStep.DialogueOptions.Count - 1;
            SetCurrentDialogueOptionId(CurrentDialogueOptionId);
        }
        else if(pressedNumber > 0 && pressedNumber <= ActiveDialogue.ActiveStep.DialogueOptions.Count)
        {
            ContinueInteraction(pressedNumber - 1);
        }
    }

    private void SetCurrentDialogueOptionId(int optionId)
    {
        CurrentDialogueOptionId = optionId;
        InteractionBox.SetCurrentDialogueOptionId(optionId);
    }

    private void StartDialogue(Dialogue dialogue)
    {
        ActiveDialogue = dialogue;
        Player.PlayerController.InputMode = PlayerInputMode.Interaction;
        InteractionBox.gameObject.SetActive(true);

        ShowStep(ActiveDialogue.ActiveStep);
    }

    private void ShowStep(DialogueStep step)
    {
        InteractionCamera.transform.position = Model.TilemapGenerator.GetWorldPosition(step.Target.GridPosition) + new Vector3(0f, 0f, -1f);
        InteractionBox.DisplayDialogueStep(step, Player);
        SetCurrentDialogueOptionId(0);
    }

    private void ContinueInteraction(int optionId)
    {
        ActiveDialogue.SubmitPlayerOption(optionId);
        if (ActiveDialogue.ActiveStep == null) EndInteraction();
        else ShowStep(ActiveDialogue.ActiveStep);
    }

    private void EndInteraction()
    {
        Player.PlayerController.InputMode = PlayerInputMode.Movement;
        if (InteractionTargetNPC != null)
        {
            InteractionTargetNPC.EndInteraction();
            InteractionTargetNPC = null;
        }
        InteractionBox.gameObject.SetActive(false);
    }

    private int GetPressedNumber()
    {
        for (int number = 0; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()))
                return number;
        }

        return -1;
    }
}
