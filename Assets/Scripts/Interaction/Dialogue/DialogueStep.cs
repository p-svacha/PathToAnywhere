using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A dialogue step represents one page of text/player options in a dialogue.
/// </summary>
public class DialogueStep
{
    public Vector2Int TargetPosition;
    public string TargetName;

    public string Text;
    public List<DialogueOption> DialogueOptions;

    public DialogueStep(Vector2Int targetPosition, string targetName, string text, List<DialogueOption> dialogueOptions)
    {
        TargetPosition = targetPosition;
        TargetName = targetName;
        Text = text;
        DialogueOptions = dialogueOptions;
    }
}
