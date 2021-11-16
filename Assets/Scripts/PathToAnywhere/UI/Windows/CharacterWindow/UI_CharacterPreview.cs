using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterPreview : MonoBehaviour
{
    private float Width;
    private float Height;

    public void Init(Character c)
    {
        Width = GetComponent<RectTransform>().rect.width;
        Height = GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(0).gameObject);

        Image body = GetBodyPartImage(c.Appearance.Body, transform);
        Image head = GetBodyPartImage(c.Appearance.Head, transform);
        Image hair = GetBodyPartImage(c.Appearance.Hair, transform);
    }

    private Image GetBodyPartImage(BodyPart part, Transform parent)
    {
        GameObject imgObject = new GameObject("DisplayPart");
        imgObject.transform.SetParent(parent);

        RectTransform rect = imgObject.AddComponent<RectTransform>();
        rect.localScale = Vector2.one;
        rect.sizeDelta = new Vector2(Width * part.SpriteScale, Height * part.SpriteScale);
        rect.localPosition = new Vector3(Width * part.SpritePositions[Direction.S].x, Height * part.SpritePositions[Direction.S].y, 0f);

        Image image = imgObject.AddComponent<Image>();
        image.sprite = part.FrontSprite;
        image.color = part.Color;
        return image;
    }
}
