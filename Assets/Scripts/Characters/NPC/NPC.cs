using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An NPC represents a character that is not controlled by the player.
/// </summary>
public class NPC : Character
{
    public NPCController NPCController;

    public override void Init(GameModel model, Vector2Int position, CharacterController controller, Transform movePoint, CharacterAppearance appearance)
    {
        base.Init(model, position, controller, movePoint, appearance);
        MovementSpeed = Random.Range(1f, 2f);
        NPCController = (NPCController)controller;
        NPCController.NPC = this;
    }

    /// <summary>
    /// This method gets called when an interaction with this NPC is started. It returns the dialogue (all text, player options, emerging consequences, etc.) that will be held with the NPC.
    /// </summary>
    public Dialogue StartInteractionWith(Character target)
    {
        // Stop movement and look at target
        NPCController.ClearTargetPath();
        NPCController.ActionState = ActionState.Interacting;
        FacePosition(target.GridPosition);

        // Create relationship towards target if not existing
        if (!OutRelationships.ContainsKey(target)) Model.AddRelationship(this, target);

        return GetDialogue(target);
    }

    public void EndInteraction()
    {
        NPCController.ActionState = ActionState.Idle;
    }

    #region Dialogue

    private Dialogue GetDialogue(Character target)
    {
        return new Dialogue(GetInitialDialogueStep(target));
    }

    private DialogueStep GetInitialDialogueStep(Character target)
    {
        Relationship rel = OutRelationships[target];

        string helloText = "Hello, I am " + Name;
        if (Home.Settlement != null) helloText += " and I live in " + Home.Settlement.Name;
        helloText += ".";

        List<DialogueOption> options = new List<DialogueOption>();
        options.Add(new DialogueOption("I love you", () => Model.ChangeAttitude(this, target, 10), GetEndDialogueStep("Aw, that is sweet.")));
        options.Add(new DialogueOption("I hate you", () => Model.ChangeAttitude(this, target, -10), GetEndDialogueStep("Fuck off.")));
        options.Add(new DialogueOption("Ok", () => { }, GetEndDialogueStep("Ok Bye.")));

        return new DialogueStep(this, helloText, options);
    }

    private DialogueStep GetEndDialogueStep(string text)
    {
        return new DialogueStep(this, text, new List<DialogueOption>() { new DialogueOption("Leave", () => { }, null) });
    }

    #endregion
}
