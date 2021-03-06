using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.Infinite
{
    public class Building : Structure
    {
        public BaseFeatureType WallType;
        public BaseFeatureType FloorType;
        public RoofType RoofType;

        public Color DebugColor;

        public Dictionary<Vector2Int, TileBase> RoofTiles; // Tiles that are rendered on the roof tilemap but only when player is outside of the building
        public List<Vector2Int> InsideTiles;

        public Building(Vector2Int origin, BaseFeatureType wallType, BaseFeatureType floorType, RoofType roofType) : base(origin)
        {
            WallType = wallType;
            FloorType = floorType;
            RoofType = roofType;

            DebugColor = ColorManager.GetRandomRedishColor();
            RoofTiles = new Dictionary<Vector2Int, TileBase>();
            InsideTiles = new List<Vector2Int>();
        }

        public void SetDrawRoof(InfiniteMapGenerator generator, bool draw)
        {
            foreach (KeyValuePair<Vector2Int, TileBase> kvp in RoofTiles)
            {
                if (draw) generator.SetRoofTile(kvp.Key, kvp.Value);
                else generator.SetRoofTile(kvp.Key, null);
            }
        }

        public override void PlaceStructure(InfiniteMapGenerator generator)
        {
            base.PlaceStructure(generator);
            SetDrawRoof(generator, true);

            /*
            // Place character
            Vector2Int characterPosition = GetRandomInsidePosition();
            NPC character = model.CharacterGenerator.GenerateNPC(characterPosition);
            character.Home = this;
            */
        }

        public Vector2Int GetRandomInsidePosition()
        {
            return InsideTiles[Random.Range(0, InsideTiles.Count)];
        }

        public override List<Vector2Int> GetCollisionTiles()
        {
            return BaseTiles;
        }
    }
}
