using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A dialogue step represents one page of text/player options in a dialogue.
/// </summary>
public class DialogueStep
{
    public Character Target;

    public string Text;
    public List<DialogueOption> DialogueOptions;

    public DialogueStep(Character target, string text, List<DialogueOption> dialogueOptions)
    {
        Target = target;
        Text = text;
        DialogueOptions = dialogueOptions;
    }
}
