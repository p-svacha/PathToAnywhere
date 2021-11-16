using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapGeneration.Infinite
{
    public static class SettlementGenerator
    {
        private static Dictionary<Vector2Int, Direction> Connectors;

        private static float PATH_SPLIT_CHANCE = 0.05f;

        /// <summary>
        /// Generates the metadata for a settlement without yet placing it.
        /// The settlement will be placed within the region considering already existing buildings but removing trees.
        /// </summary>
        public static Settlement GenerateSettlement(InfiniteMapGenerator generator, Region region)
        {
            if (!region.FullyLoaded) throw new System.Exception("Cannot create a settlement in a region that is not fully loaded.");

            Connectors = new Dictionary<Vector2Int, Direction>();

            // Start with a town square with a connector on each side
            Vector2Int origin = region.TilePositions[Random.Range(0, region.TilePositions.Count)];

            Settlement newSettlement = new Settlement(origin, NameGenerator.GenerateName(NameGenerationType.Settlement), region);
            newSettlement.Structures = new List<Structure>();
            newSettlement.PathPositions = new List<Vector2Int>();

            int townSquareRadius = Random.Range(1, 4);
            for (int y = origin.y - townSquareRadius; y <= origin.y + townSquareRadius; y++)
            {
                for (int x = origin.x - townSquareRadius; x <= origin.x + townSquareRadius; x++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (region.TilePositions.Contains(pos)) newSettlement.PathPositions.Add(pos);
                }
            }
            Connectors.Add(origin + new Vector2Int(0, townSquareRadius), Direction.N);
            Connectors.Add(origin + new Vector2Int(townSquareRadius, 0), Direction.E);
            Connectors.Add(origin + new Vector2Int(0, -townSquareRadius), Direction.S);
            Connectors.Add(origin + new Vector2Int(-townSquareRadius, 0), Direction.W);



            while (Connectors.Count > 0)
            {
                Vector2Int connectorPosition = Connectors.Keys.ToList()[Random.Range(0, Connectors.Values.Count)];
                Direction connectorDirection = Connectors[connectorPosition];
                Connectors.Remove(connectorPosition);

                float rng = Random.value;
                if (rng < 0.6f)
                {
                    Building b = AddBuilding(generator, connectorPosition, connectorDirection);
                    if (!b.IsColliding(newSettlement) && b.IsFullyWithinRegion(region)) newSettlement.Structures.Add(b);
                }
                else AddPath(newSettlement, connectorPosition, connectorDirection);
            }

            newSettlement.InitSettlement();

            // Remove trees that are on path or buildings
            List<Tree> treesToRemove = region.Trees.Where(x => x.IsColliding(newSettlement)).ToList();
            foreach (Tree t in treesToRemove) region.Trees.Remove(t);


            return newSettlement;
        }

        private static void AddPath(Settlement settlement, Vector2Int source, Direction dir)
        {
            int length = Random.Range(3, 7);
            bool abort = false;
            for (int i = 1; i <= length; i++)
            {
                Vector2Int pathPos = GetTileInDirection(source, dir, i);
                if (settlement.Region.TilePositions.Contains(pathPos) && !settlement.PathPositions.Contains(pathPos))
                {
                    settlement.PathPositions.Add(pathPos);
                    if (Random.value < PATH_SPLIT_CHANCE) // Split right
                    {
                        AddPath(settlement, pathPos, GetNextClockwiseDirection(dir));
                    }
                    if (Random.value < PATH_SPLIT_CHANCE) // Split left
                    {
                        AddPath(settlement, pathPos, GetNextCounterClockwiseDirection(dir));
                    }
                }
                else
                {
                    abort = true;
                    break;
                }
            }
            Vector2Int endPos = GetTileInDirection(source, dir, length);
            if (!abort) Connectors[endPos] = dir;
        }

        /// <summary>
        /// Adds a random building to the settlement.
        /// The source point represents the piece of path in front of the door.
        /// </summary>
        private static Building AddBuilding(InfiniteMapGenerator generator, Vector2Int source, Direction dir)
        {
            Vector2Int doorPos = GetTileInDirection(source, dir, 1);

            int rightLength = Random.Range(1, 5); // Size of the building when you go right after the door
            int forwardLength = Random.Range(2, 9); // Size of the building when you go forward after the door
            int leftLength = Random.Range(1, 5); // Size of the building when you go left after the door

            Vector2Int rightFrontCorner = GetTileInDirection(doorPos, GetNextClockwiseDirection(dir), rightLength);
            Vector2Int leftFrontCorner = GetTileInDirection(doorPos, GetNextCounterClockwiseDirection(dir), leftLength);
            Vector2Int rightBackCorner = GetTileInDirection(rightFrontCorner, dir, forwardLength);
            Vector2Int leftBackCorner = GetTileInDirection(leftFrontCorner, dir, forwardLength);

            Vector2Int southWestCorner = dir == Direction.N ? leftFrontCorner : dir == Direction.E ? rightFrontCorner : dir == Direction.S ? rightBackCorner : leftBackCorner;
            Vector2Int northEastCorner = dir == Direction.N ? rightBackCorner : dir == Direction.E ? leftBackCorner : dir == Direction.S ? leftFrontCorner : rightFrontCorner;
            Vector2Int dimensions = (northEastCorner - southWestCorner) + Vector2Int.one;
            Debug.Log(dimensions.ToString() + " from " + southWestCorner.ToString());

            BuildingGenerationSettings settings = new BuildingGenerationSettings(doorPos, southWestCorner, dimensions, BaseFeatureType.Wall, BaseFeatureType.Floor, RoofType.DefaultRoof);
            Building b = BuildingGenerator.GenerateBuilding(generator, settings);

            return b;
        }

        private static Vector2Int GetTileInDirection(Vector2Int origin, Direction dir, int distance)
        {
            if (dir == Direction.N) return origin + new Vector2Int(0, distance);
            if (dir == Direction.E) return origin + new Vector2Int(distance, 0);
            if (dir == Direction.S) return origin + new Vector2Int(0, -distance);
            if (dir == Direction.W) return origin + new Vector2Int(-distance, 0);
            throw new System.Exception("Direction not handled");
        }

        private static Direction GetNextClockwiseDirection(Direction dir)
        {
            if (dir == Direction.N) return Direction.E;
            if (dir == Direction.E) return Direction.S;
            if (dir == Direction.S) return Direction.W;
            if (dir == Direction.W) return Direction.N;
            throw new System.Exception("Direction not handled");
        }

        private static Direction GetNextCounterClockwiseDirection(Direction dir)
        {
            if (dir == Direction.N) return Direction.W;
            if (dir == Direction.E) return Direction.N;
            if (dir == Direction.S) return Direction.E;
            if (dir == Direction.W) return Direction.S;
            throw new System.Exception("Direction not handled");
        }
    }
}
