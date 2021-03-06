using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Infinite
{
    public class Settlement : Structure
    {
        public string Name;
        public Region Region;
        public List<Structure> Structures;
        public List<Vector2Int> PathPositions;

        public Settlement(Vector2Int origin, string name, Region region) : base(origin)
        {
            Name = name;
            Region = region;
        }

        public void InitSettlement()
        {
            foreach (Structure s in Structures) s.Settlement = this;
            foreach (Vector2Int path in PathPositions) BaseFeatureTypes.Add(path, BaseFeatureType.Path);
        }

        public override void PlaceStructure(InfiniteMapGenerator generator)
        {
            base.PlaceStructure(generator);
            foreach (Structure s in Structures) s.PlaceStructure(generator);
        }

        public override List<Vector2Int> GetCollisionTiles()
        {
            List<Vector2Int> collisionTiles = new List<Vector2Int>();
            foreach (Structure s in Structures) collisionTiles.AddRange(s.GetCollisionTiles());
            collisionTiles.AddRange(PathPositions);
            return collisionTiles;
        }
    }
}
