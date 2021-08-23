using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogueOption : MonoBehaviour
{
    public DialogueOption DialogueOption;
    public Image Arrow;
    public Text Text;

    public void Init(int id, DialogueOption option)
    {
        DialogueOption = option;
        Text.text = id + ") " + option.Text;
    }
}
