using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileSetSliced : TileSet
{
    // For order which piece is which, look at order of WallSet1.png
    public TileBase Surrounded;
    public TileBase Center1;
    public TileBase Center2;
    public TileBase Center3;

    public TileBase Center4;
    public TileBase T1;
    public TileBase T2;
    public TileBase T3;

    public TileBase Single;
    public TileBase Center0;
    public TileBase T0;
    public TileBase Corner1;

    public TileBase End;
    public TileBase Straight;
    public TileBase Corner0;
   
    public override List<TileBase> GetTiles()
    {
        return new List<TileBase>()
        {
            Surrounded, Single, Straight, End, Corner0, Corner1, T0, T1, T2, T3, Center0, Center1, Center2, Center3, Center4
        };
    }
}
