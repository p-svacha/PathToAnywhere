using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InteractionBox : MonoBehaviour
{
    public GameObject TargetBox;
    public Text TargetName;
    public Image TargetImage;

    public Text Text;

    public void DisplayText(string targetName, string text)
    {
        TargetName.text = targetName;
        Text.text = text;
    }
}
