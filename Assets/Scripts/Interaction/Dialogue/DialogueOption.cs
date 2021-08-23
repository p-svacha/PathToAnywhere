using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A dialogue option represents an option the player can chose from during a dialogue, and its consequences.
/// </summary>
public class DialogueOption
{
    public string Text;
    public Action Action;
    public DialogueStep NextStep;

    public DialogueOption(string text, Action action, DialogueStep nextStep)
    {
        Text = text;
        Action = action;
        NextStep = nextStep;
    }
}
