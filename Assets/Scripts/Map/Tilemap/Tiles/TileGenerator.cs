using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileGenerator
{
    public static TileSetSimple GenerateSimpleTilset(TilemapGenerator generator, Texture2D texture, TileType type, TileData data, int spliceSize)
    {
        TileSetSimple tileset = new TileSetSimple(data, type);

        int cols = texture.width / spliceSize;
        int rows = texture.height / spliceSize;

        for(int y = 0; y < rows; y++)
        {
            for(int x = 0; x < cols; x++)
            {
                Tile tile = GetTileAt(texture, spliceSize, x, y);
                tileset.Tiles.Add(tile);
            }
        }

        generator.TileSets.Add(type, tileset);

        return tileset;
    }

    public static TileSetSliced GenerateSlicedTileset(TilemapGenerator generator, Texture2D texture, TileType type, TileData data, int spliceSize)
    {
        TileSetSliced tileset = new TileSetSliced(data, type);

        tileset.Surrounded = GetTileAt(texture, spliceSize, 0, 3);
        tileset.Center1 = GetTileAt(texture, spliceSize, 1, 3);
        tileset.Center2 = GetTileAt(texture, spliceSize, 2, 3);
        tileset.Center3 = GetTileAt(texture, spliceSize, 3, 3);

        tileset.Center4 = GetTileAt(texture, spliceSize, 0, 2);
        tileset.T1 = GetTileAt(texture, spliceSize, 1, 2);
        tileset.T2 = GetTileAt(texture, spliceSize, 2, 2);
        tileset.T3 = GetTileAt(texture, spliceSize, 3, 2);

        tileset.Single = GetTileAt(texture, spliceSize, 0, 1);
        tileset.Center0 = GetTileAt(texture, spliceSize, 1, 1);
        tileset.T0 = GetTileAt(texture, spliceSize, 2, 1);
        tileset.Corner1 = GetTileAt(texture, spliceSize, 3, 1);

        tileset.End = GetTileAt(texture, spliceSize, 0, 0);
        tileset.Straight = GetTileAt(texture, spliceSize, 1, 0);
        tileset.Corner0 = GetTileAt(texture, spliceSize, 2, 0);

        generator.TileSets.Add(type, tileset);

        return tileset;
    }

    private static Tile GetTileAt(Texture2D texture, int spliceSize, int x, int y)
    {
        Tile tile = new Tile();
        tile.sprite = Sprite.Create(texture, new Rect(x * spliceSize, y * spliceSize, spliceSize, spliceSize), new Vector2(0.5f, 0.5f), spliceSize, 1, SpriteMeshType.Tight, Vector4.zero);
        return tile;
    }
}
