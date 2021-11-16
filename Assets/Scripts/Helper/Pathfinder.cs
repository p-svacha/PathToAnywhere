using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    // A* algorithm implementation. https://pavcreations.com/tilemap-based-a-star-algorithm-implementation-in-unity-game/
    public static List<Vector2Int> GetPath(MapGenerator map, Vector2Int from, Vector2Int to)
    {
        if (from == to) return new List<Vector2Int>();
        if (!map.GetTile(to).IsPassable(map)) return new List<Vector2Int>();

        List<Vector2Int> openList = new List<Vector2Int>() { from }; // tiles that are queued for searching
        List<Vector2Int> closedList = new List<Vector2Int>(); // tiles that have already been searched

        Dictionary<Vector2Int, int> gCosts = new Dictionary<Vector2Int, int>();
        Dictionary<Vector2Int, int> fCosts = new Dictionary<Vector2Int, int>();
        Dictionary<Vector2Int, Vector2Int> previousTiles = new Dictionary<Vector2Int, Vector2Int>();

        gCosts.Add(from, 0);
        fCosts.Add(from, gCosts[from] + GetHCost(from, to));

        while(openList.Count > 0)
        {
            Vector2Int currentTile = GetLowestFCostTile(openList, fCosts);
            if(currentTile == to) // Reached goal
            {
                return GetFinalPath(to, previousTiles);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach(Vector2Int neighbour in GetAdjacentTiles(currentTile))
            {
                if (closedList.Contains(neighbour)) continue;
                if (!map.GetTile(neighbour).IsPassable(map)) continue;

                int tentativeGCost = gCosts[currentTile] + GetHCost(currentTile, neighbour);
                if(!gCosts.ContainsKey(neighbour) || tentativeGCost < gCosts[neighbour])
                {
                    previousTiles[neighbour] = currentTile;
                    gCosts[neighbour] = tentativeGCost;
                    fCosts[neighbour] = tentativeGCost + GetHCost(neighbour, to);

                    if (!openList.Contains(neighbour)) openList.Add(neighbour);
                }
            }
        }

        // Out of tiles -> no path
        return null;
    }

    private static int GetHCost(Vector2Int from, Vector2Int to)
    {
        return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
    }

    private static Vector2Int GetLowestFCostTile(List<Vector2Int> list, Dictionary<Vector2Int, int> fCosts)
    {
        int lowestCost = int.MaxValue;
        Vector2Int lowestCostTile = list[0];
        foreach(Vector2Int tile in list)
        {
            if(fCosts[tile] < lowestCost)
            {
                lowestCostTile = tile;
                lowestCost = fCosts[tile];
            }
        }
        return lowestCostTile;
    }

    private static List<Vector2Int> GetAdjacentTiles(Vector2Int pos)
    {
        return new List<Vector2Int>()
        {
            pos + new Vector2Int(-1, 0),
            pos + new Vector2Int(1, 0),
            pos + new Vector2Int(0, -1),
            pos + new Vector2Int(0, 1),
        };
    }

    private static List<Vector2Int> GetFinalPath(Vector2Int to, Dictionary<Vector2Int, Vector2Int> previousTiles)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(to);
        Vector2Int currentTile = to;
        while(previousTiles.ContainsKey(currentTile))
        {
            path.Add(previousTiles[currentTile]);
            currentTile = previousTiles[currentTile];
        }
        path.Reverse();
        return path;
    }
}
