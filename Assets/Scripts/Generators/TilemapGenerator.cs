using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [Header("Tiles")]
    public Tile White;
    public Tile Grey;
    public Tile Black;

    public void GenerateTilemap(Tilemap tilemap)
    {
        for(int y = -50; y < 50; y++)
        {
            for(int x = -50; x < 50; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), GetRandomTile());
            }
        }
    }

    private Tile GetRandomTile()
    {
        float rng = Random.value;
        if (rng < 0.4f) return White;
        else if (rng < 0.6f) return Grey;
        else return Black;
    }
}
