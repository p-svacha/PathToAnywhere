using System.Collections;
using System.Collections.Generic;
using MapGeneration.Finite;
using UnityEngine;

namespace MapGeneration
{
    public class FiniteMapGenerator : MapGenerator
    {
        public int Width;
        public int Height;

        public TileInfo[,] Tiles;

        private TileInfo OutOfBoundsTile;

        public FiniteMapGenerator() : base()
        {
            OutOfBoundsTile = new TileInfo(new Vector2Int(-1, -1));
        }

        public void GenerateMap(FiniteMapGenerationSettings settings)
        {
            Width = settings.Width;
            Height = settings.Height;
            Tiles = new TileInfo[settings.Width, settings.Height];

            for(int y = 0; y < Height; y++)
            {
                for(int x = 0; x < Width; x++)
                {
                    TileInfo tile = new TileInfo(new Vector2Int(x, y));
                    float perlin1 = Mathf.PerlinNoise(5000f + x * 0.1f, 90000f + y * 0.1f);
                    if(perlin1 <= 0.5f) tile.BaseSurfaceType = BaseSurfaceType.Sand;
                    else if(perlin1 <= 2f) tile.BaseSurfaceType = BaseSurfaceType.Grass1;

                    float perlin2 = Mathf.PerlinNoise(50000f + x * 0.1f, 9000f + y * 0.1f);
                    if (perlin2 <= 0.5f) tile.BaseSurfaceType = BaseSurfaceType.Water;

                    Tiles[x, y] = tile;
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    PlaceTile(x, y);
                }
            }
        }

        public override TileInfo GetTile(Vector2Int cellCoordinates)
        {
            if (cellCoordinates.x < 0 || cellCoordinates.x >= Width || cellCoordinates.y < 0 || cellCoordinates.y >= Height) return OutOfBoundsTile;
            return Tiles[cellCoordinates.x, cellCoordinates.y];
        }
    }
}
