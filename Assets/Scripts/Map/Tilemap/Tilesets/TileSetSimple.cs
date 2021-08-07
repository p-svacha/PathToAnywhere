using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileSetSimple : TileSet
{
    public List<TileBase> Tiles;

    public override List<TileBase> GetTiles()
    {
        return Tiles;
    }

    public TileBase GetRandomTile()
    {
        return Tiles[Random.Range(0, Tiles.Count)];
    }
}
