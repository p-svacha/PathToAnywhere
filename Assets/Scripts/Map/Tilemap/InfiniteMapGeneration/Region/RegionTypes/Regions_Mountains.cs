using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.Infinite
{
    public class Regions_Mountains : Region
    {
        public Regions_Mountains(InfiniteMapGenerator generator, Vector2Int id) : base(generator, id)
        {
            Type = RegionType.Mountain;
        }

        protected override void GenerateLayout()
        {
            foreach (Vector2Int pos in TilePositions)
            {
                MapGenerator.SetBaseFeatureType(pos, BaseFeatureType.Mountain);
            }
        }
    }
}
