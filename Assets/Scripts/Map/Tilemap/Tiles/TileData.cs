using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public TileSet[] TileSets;

    public float SpeedModifier;
    public bool Passable;

    public TileData(bool passable, float speedModifier)
    {
        Passable = passable;
        SpeedModifier = speedModifier;
    }
}
