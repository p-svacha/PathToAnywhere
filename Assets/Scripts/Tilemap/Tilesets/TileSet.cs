using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileSet : ScriptableObject
{
    public abstract List<TileBase> GetTiles();
}
