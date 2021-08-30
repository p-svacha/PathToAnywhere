using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains the information about how a character looks and controls the visual display of the character.
/// </summary>
public class CharacterAppearance
{
    public BodyPart Body;
    public BodyPart Head;
    public BodyPart Hair;

    public CharacterAppearance(BodyPart body, BodyPart head, BodyPart hair)
    {
        Body = body;
        Head = head;
        Hair = hair;

        Hair.SetSortingOrder(3);
    }

    public void ShowCharacterSide(Direction dir)
    {
        Body.ShowDirection(dir);
        Head.ShowDirection(dir);
        Hair.ShowDirection(dir);

        switch (dir)
        {
            case Direction.W:
                Head.SetSortingOrder(1);
                Body.SetSortingOrder(0);
                break;

            case Direction.E:
                Head.SetSortingOrder(1);
                Body.SetSortingOrder(0);
                break;

            case Direction.N:
                Head.SetSortingOrder(1);
                Body.SetSortingOrder(2);
                break;

            case Direction.S:
                Head.SetSortingOrder(1);
                Body.SetSortingOrder(0);
                break;
        }
    }
}
