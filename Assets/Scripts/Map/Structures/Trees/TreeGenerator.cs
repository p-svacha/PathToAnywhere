using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeGenerator : MonoBehaviour
{
    public Texture2D CypressTree1;
    private TileBase CypressTree1Base;
    private TileBase CypressTree1Mid;
    private TileBase CypressTree1Top;

    private void Awake()
    {
        CypressTree1Base = TileGenerator.GetTileAt(CypressTree1, TilemapGenerator.TilePixelSize, 0, 0);
        CypressTree1Mid = TileGenerator.GetTileAt(CypressTree1, TilemapGenerator.TilePixelSize, 0, 1);
        CypressTree1Top = TileGenerator.GetTileAt(CypressTree1, TilemapGenerator.TilePixelSize, 0, 2);
    }

    public Tree GenerateTree(Vector2Int origin)
    {
        Tree tree = new Tree(origin);

        int numMidParts = Random.Range(0, 3);

        tree.OverlayTiles.Add(origin, CypressTree1Base);
        tree.ImpassableTiles.Add(origin);

        for (int i = 0; i < numMidParts; i++) tree.FrontOfPlayerTiles.Add(origin + new Vector2Int(0, i + 1), CypressTree1Mid);

        tree.FrontOfPlayerTiles.Add(origin + new Vector2Int(0, numMidParts + 1), CypressTree1Top);

        return tree;
    }

    public static TreeGenerator Instance
    {
        get
        {
            return GameObject.Find("TreeGenerator").GetComponent<TreeGenerator>();
        }
    }

}
