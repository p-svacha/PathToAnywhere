using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tree : Structure
{
    public Tree(Vector2Int origin) : base(origin) { }

    public override List<Vector2Int> GetCollisionTiles()
    {
        return new List<Vector2Int>() { Origin };
    }
}
