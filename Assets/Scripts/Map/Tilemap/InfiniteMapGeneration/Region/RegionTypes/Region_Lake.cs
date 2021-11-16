using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Infinite
{
    public class Region_Lake : Region
    {
        public Region_Lake(InfiniteMapGenerator generator, Vector2Int id) : base(generator, id)
        {
            Type = RegionType.Lake;
        }

        protected override void GenerateLayout()
        {
            foreach (Vector2Int pos in TilePositions)
            {
                MapGenerator.SetBaseSurfaceType(pos, BaseSurfaceType.Sand);
                MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Water);
            }
        }
    }
}
