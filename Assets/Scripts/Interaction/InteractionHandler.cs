using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    private GameModel Model;

    public UI_InteractionBox InteractionBox;

    public void Init(GameModel model)
    {
        Model = model;
        InteractionBox.gameObject.SetActive(false);
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
