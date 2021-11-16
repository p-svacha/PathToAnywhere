using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RelationshipWindow : UI_Window
{
    [Header("Prefabs")]
    public UI_RelationshipElement ListElement;

    [Header("Prefab")]
    public GameObject ListContainer;


    public override void Init(GameModel model)
    {
        base.Init(model);
        foreach(Transform child in ListContainer.transform) Destroy(child.gameObject);

        foreach(Relationship r in model.Player.InRelationships.Values)
        {
            UI_RelationshipElement elem = Instantiate(ListElement, ListContainer.transform);
            elem.Init(r);
        }

        Canvas.ForceUpdateCanvases();
        ListContainer.GetComponent<VerticalLayoutGroup>().enabled = false;
        ListContainer.GetComponent<VerticalLayoutGroup>().enabled = true;
    }
}
