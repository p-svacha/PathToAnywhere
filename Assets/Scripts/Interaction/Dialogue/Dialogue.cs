using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A dialogue object contains all information about npc text, player options and consequences of an interaction with an npc.
/// </summary>
public class Dialogue
{
    public DialogueStep ActiveStep;

    public Dialogue(DialogueStep startStep)
    {
        ActiveStep = startStep;
    }

    public void SubmitPlayerOption(int optionId)
    {
        DialogueOption chosenOption = ActiveStep.DialogueOptions[optionId];
        chosenOption.Action();
        ActiveStep = chosenOption.NextStep;
    }
}
