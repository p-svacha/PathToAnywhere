using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public Color Color;

    public Sprite FrontSprite;
    public Sprite SideSprite;
    public Sprite BackSprite;

    public SpriteRenderer FrontSpriteRenderer; // south facing
    public SpriteRenderer SideSpriteRenderer; // east facing
    public SpriteRenderer BackSpriteRenderer; // north facing

    public void Init(Color color, Sprite frontSprite, Sprite sideSprite, Sprite backSprite)
    {
        Color = color;
        FrontSprite = frontSprite;
        SideSprite = sideSprite;
        BackSprite = backSprite;
        FrontSpriteRenderer = CreateRenderer(FrontSprite);
        SideSpriteRenderer = CreateRenderer(SideSprite);
        BackSpriteRenderer = CreateRenderer(BackSprite);
        ShowDirection(Direction.S);
    }

    private SpriteRenderer CreateRenderer(Sprite sprite)
    {
        GameObject rendererObject = new GameObject("BodyPartRenderer");
        rendererObject.transform.SetParent(transform);
        SpriteRenderer renderer = rendererObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = Color;
        renderer.sortingLayerName = CharacterGenerator.PlayerLayerName;
        return renderer;
    }

    public void ShowDirection(Direction dir)
    {
        if(dir == Direction.N)
        {
            FrontSpriteRenderer.gameObject.SetActive(false);
            SideSpriteRenderer.gameObject.SetActive(false);
            BackSpriteRenderer.gameObject.SetActive(true);
        }
        else if (dir == Direction.E)
        {
            FrontSpriteRenderer.gameObject.SetActive(false);
            SideSpriteRenderer.gameObject.SetActive(true);
            SideSpriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
            BackSpriteRenderer.gameObject.SetActive(false);
        }
        else if (dir == Direction.W)
        {
            FrontSpriteRenderer.gameObject.SetActive(false);
            SideSpriteRenderer.gameObject.SetActive(true);
            SideSpriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
            BackSpriteRenderer.gameObject.SetActive(false);
        }
        else if (dir == Direction.S)
        {
            FrontSpriteRenderer.gameObject.SetActive(true);
            SideSpriteRenderer.gameObject.SetActive(false);
            BackSpriteRenderer.gameObject.SetActive(false);
        }
    }

    public void SetSortingOrder(int order)
    {
        FrontSpriteRenderer.sortingOrder = order;
        SideSpriteRenderer.sortingOrder = order;
        BackSpriteRenderer.sortingOrder = order;
    }

    public void SetLocalPoisition(Vector3 pos)
    {
        FrontSpriteRenderer.transform.localPosition = pos;
        SideSpriteRenderer.transform.localPosition = pos;
        BackSpriteRenderer.transform.localPosition = pos;
    }
}
