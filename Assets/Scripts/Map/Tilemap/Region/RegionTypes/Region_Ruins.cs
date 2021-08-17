using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region_Ruins : Region
{
    public Region_Ruins(GameModel model, Vector2Int id) : base(model, id)
    {
        Type = RegionType.Ruins;
    }

    protected override void GenerateLayout()
    {
        foreach (Vector2Int pos in TilePositions)
        {
            SetRandomTypesFor(pos);
        }
    }

    private void SetRandomTypesFor(Vector2Int pos)
    {
        float rng = Random.value;
        if (rng < 0.25f) MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Grass);
        else if (rng < 0.5f) MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Sand);
        else if (rng < 0.75f) MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Dirt);
        else MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Mountain);
    }
}
