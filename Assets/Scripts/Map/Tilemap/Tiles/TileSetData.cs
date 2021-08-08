using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetData
{
    public float SpeedModifier;
    public bool Passable;

    public TileSetData(bool passable, float speedModifier)
    {
        Passable = passable;
        SpeedModifier = speedModifier;
    }
}
