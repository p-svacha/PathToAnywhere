using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An NPC represents a character that is not controlled by the player.
/// </summary>
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

    /// <summary>
    /// This method gets called when an interaction with this NPC is started. It returns the dialogue (all text, player options, emerging consequences, etc.) that will be held with the NPC.
    /// </summary>
    public Dialogue StartInteractionWith(Character target)
    {
        NPCController.ClearTargetPath();
        NPCController.ActionState = ActionState.Interacting;
        FacePosition(target.GridPosition);

        return GetDialogue();
    }

    public void EndInteraction()
    {
        NPCController.ActionState = ActionState.Idle;
    }

    private Dialogue GetDialogue()
    {
        return new Dialogue(GetDialogueStep());
    }

    private DialogueStep GetDialogueStep()
    {
        List<DialogueOption> options = new List<DialogueOption>();
        int numOptions = Random.Range(1, 4);
        for (int i = 0; i < numOptions; i++)
        {
            DialogueStep nextStep = null;
            string text = "This dialogue option will end the dialogue";
            if (Random.value < 0.25f)
            {
                nextStep = GetDialogueStep();
                text = "This dialogue option will continue the interaction.";
            }
            options.Add(new DialogueOption(text, () => { }, nextStep));
        }
        string helloText = "Hello, I am " + Name;
        if (Home.Settlement != null) helloText += " and I live in " + Home.Settlement.Name;
        helloText += ".";
        DialogueStep step = new DialogueStep(GridPosition, Name, helloText, options);
        return step;
    }
}
