using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration.Infinite
{
    public abstract class Region
    {
        public InfiniteMapGenerator MapGenerator;
        public Vector2Int Id;
        public RegionType Type;
        public Color Color;
        public bool FullyLoaded;
        public List<TilemapChunk> Chunks;
        public List<Vector2Int> TilePositions;

        // Region elements
        public List<Tree> Trees;
        public List<Building> Buildings; // Buildings that do not belong to a settlement

        protected float SettlementChance;
        private Settlement Settlement;

        public Region(InfiniteMapGenerator generator, Vector2Int id)
        {
            MapGenerator = generator;
            Id = id;
            Color = ColorManager.GetRandomGreenishColor();
            Chunks = new List<TilemapChunk>();
            TilePositions = new List<Vector2Int>();
            Trees = new List<Tree>();
            Buildings = new List<Building>();
        }

        public void OnLoadingComplete()
        {
            FullyLoaded = true;
            GenerateLayout();
            GenerateSettlement();
            PlaceStructures();
        }

        /// <summary>
        /// All tiles within the region are known when this method is called.
        /// Creates metadata for all tiles within the region without yet placing them.
        /// </summary>
        protected abstract void GenerateLayout();

        /// <summary>
        /// Generates the metadata for a settlement on this region without yet placing it.
        /// </summary>
        private void GenerateSettlement()
        {
            if (Random.value < SettlementChance) Settlement = SettlementGenerator.GenerateSettlement(MapGenerator, this);
        }

        /// <summary>
        /// Places all structures (trees, buildings, etc.) of the region into the map.
        /// </summary>
        private void PlaceStructures()
        {
            foreach (Building b in Buildings) b.PlaceStructure(MapGenerator);
            foreach (Tree t in Trees) t.PlaceStructure(MapGenerator);
            if (Settlement != null) Settlement.PlaceStructure(MapGenerator);
        }
    }
}
