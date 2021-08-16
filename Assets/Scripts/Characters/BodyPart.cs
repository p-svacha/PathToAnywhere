using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public SpriteRenderer FrontSprite; // south facing
    public SpriteRenderer SideSprite; // east facing
    public SpriteRenderer BackSprite; // north facing

    public void Init(SpriteRenderer frontSprite, SpriteRenderer sideSprite, SpriteRenderer backSprite)
    {
        FrontSprite = frontSprite;
        SideSprite = sideSprite;
        BackSprite = backSprite;
        ShowDirection(Direction.S);
    }

    public void ShowDirection(Direction dir)
    {
        if(dir == Direction.N)
        {
            FrontSprite.gameObject.SetActive(false);
            SideSprite.gameObject.SetActive(false);
            BackSprite.gameObject.SetActive(true);
        }
        else if (dir == Direction.E)
        {
            FrontSprite.gameObject.SetActive(false);
            SideSprite.gameObject.SetActive(true);
            SideSprite.transform.localScale = new Vector3(1f, 1f, 1f);
            BackSprite.gameObject.SetActive(false);
        }
        else if (dir == Direction.W)
        {
            FrontSprite.gameObject.SetActive(false);
            SideSprite.gameObject.SetActive(true);
            SideSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
            BackSprite.gameObject.SetActive(false);
        }
        else if (dir == Direction.S)
        {
            FrontSprite.gameObject.SetActive(true);
            SideSprite.gameObject.SetActive(false);
            BackSprite.gameObject.SetActive(false);
        }
    }

    public void SetSortingOrder(int order)
    {
        FrontSprite.sortingOrder = order;
        SideSprite.sortingOrder = order;
        BackSprite.sortingOrder = order;
    }

    public void SetLocalPoisition(Vector3 pos)
    {
        FrontSprite.transform.localPosition = pos;
        SideSprite.transform.localPosition = pos;
        BackSprite.transform.localPosition = pos;
    }
}
