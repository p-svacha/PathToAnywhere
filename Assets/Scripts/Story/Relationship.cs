using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A relationship represents the attitude from one character to another and can influence behaviour.
/// </summary>
public class Relationship
{
    public Character SourceCharacter;
    public Character TargetCharacter;

    /// <summary>
    /// Describes how much the source character likes the target character, while a positive value means they like them, a negative value means they dislike them and a value of 0 means no opinion.
    /// </summary>
    public int Attitude;

    public Relationship(Character source, Character target)
    {
        SourceCharacter = source;
        TargetCharacter = target;
        Attitude = 0;
    }
}
