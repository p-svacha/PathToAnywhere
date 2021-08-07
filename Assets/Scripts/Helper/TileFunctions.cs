using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileFunctions
{
    public static bool DoRectanglesOverlap(Vector2Int posA, int widthA, int heightA, Vector2Int posB, int widthB, int heightB)
    {
        Vector2Int l1 = posA;
        Vector2Int r1 = posA + new Vector2Int(widthA, heightA);
        Vector2Int l2 = posB;
        Vector2Int r2 = posB + new Vector2Int(widthB, heightB);

        // If either rectangle is actually a line
        if (l1.x == r1.x || l1.y == r1.y || l2.x == r2.x || l2.y == r2.y) return false;
        // If one rectangle is on left side of other
        if (l1.x > r2.x || l2.x > r1.x) return false;
        // If one rectangle is above other
        if (r1.y < l2.y || r2.y < l1.y) return false;

        return true;
    }
}
