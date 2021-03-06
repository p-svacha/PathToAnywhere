using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetSimple : TileSet
{
    public List<TileBase> Tiles;

    public TileSetSimple() { }

    public TileSetSimple(TileSetData data) : base(data)
    {
        Tiles = new List<TileBase>();
    }

    public override void PlaceTile(MapGenerator generator, Tilemap tilemap, Vector3Int position)
    {
        tilemap.SetTile(position, GetRandomTile());
    }

    public TileBase GetRandomTile()
    {
        return Tiles[Random.Range(0, Tiles.Count)];
    }

}
