using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RelationshipElement : MonoBehaviour
{
    [Header("Prefabs")]
    public UI_CharacterPreview CharacterPreview;
    public Text NameText;
    public Text FromText;
    public Text AttitudeText;

    [Header("Elements")]
    public GameObject CharacterDisplay;

    public void Init(Relationship rel)
    {
        Character sourceChar = rel.SourceCharacter;
        CharacterPreview.Init(sourceChar);
        NameText.text = sourceChar.Name;
        FromText.text = sourceChar.GetHome();
        AttitudeText.text = rel.Attitude.ToString();
    }
}
