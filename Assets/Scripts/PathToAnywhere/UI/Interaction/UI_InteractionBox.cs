using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InteractionBox : MonoBehaviour
{
    [Header("Prefabs")]
    public UI_DialogueOption DialogueOptionPrefab;

    [Header("Elements")]
    public Text TargetName;
    public Text TargetAttitude;
    public Text Text;
    public GameObject OptionsContainer;
    public List<UI_DialogueOption> DialogueOptions = new List<UI_DialogueOption>();

    public void DisplayDialogueStep(DialogueStep step, Player player)
    {
        TargetName.text = step.Target.Name;
        TargetAttitude.text = "Attitude: " + step.Target.OutRelationships[player].Attitude;
        Text.text = step.Text;

        foreach(UI_DialogueOption option in DialogueOptions) GameObject.Destroy(option.gameObject);
        DialogueOptions.Clear();

        int optionId = 1;
        foreach(DialogueOption stepOption in step.DialogueOptions)
        {
            UI_DialogueOption option = Instantiate(DialogueOptionPrefab, OptionsContainer.transform);
            option.Init(optionId, stepOption);
            DialogueOptions.Add(option);
            optionId++;
        }
        Canvas.ForceUpdateCanvases();
        OptionsContainer.GetComponent<VerticalLayoutGroup>().enabled = false;
        OptionsContainer.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    public void SetCurrentDialogueOptionId(int optionId)
    {
        for(int i = 0; i < DialogueOptions.Count; i++)
        {
            DialogueOptions[i].Arrow.enabled = (i == optionId);
        }
    }
}
