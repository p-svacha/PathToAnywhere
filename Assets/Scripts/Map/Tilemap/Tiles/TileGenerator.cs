using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileGenerator
{
    public static TileSetSimple GenerateSimpleTilset(TilemapGenerator generator, Texture2D texture, TileSetData data, int tileSize)
    {
        TileSetSimple tileset = new TileSetSimple(data);

        int cols = texture.width / tileSize;
        int rows = texture.height / tileSize;

        for(int y = 0; y < rows; y++)
        {
            for(int x = 0; x < cols; x++)
            {
                Tile tile = GetTileAt(texture, tileSize, x, y);
                tileset.Tiles.Add(tile);
            }
        }

        return tileset;
    }

    public static TileSetSlicedFull GenerateSlicedTileset(TilemapGenerator generator, Texture2D texture, TileSetData data, int tileSize, List<BaseFeatureType> connectionTypes)
    {
        TileSetSlicedFull tileset = new TileSetSlicedFull(connectionTypes, data);
        AddTilesToSlicedSet(generator, tileset, texture, tileSize);
        return tileset;
    }

    private static void AddTilesToSlicedSet(TilemapGenerator generator, TileSetSlicedFull tileset, Texture2D texture, int tileSize)
    {
        tileset.Surrounded.Add(0, GetTileAt(texture, tileSize, 1, 6));
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterEmpty, 0, 4, 6);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.Single, 0, 3, 7);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.Straight, 0, 5, 6);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.Straight, 90, 4, 5);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.End, 0, 4, 4);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.End, 90, 6, 6);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.End, 180, 4, 7);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.End, 270, 3, 6);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerEmpty, 0, 1, 3);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerEmpty, 90, 1, 4);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerEmpty, 180, 0, 4);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerEmpty, 270, 0, 3);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerFull, 0, 2, 5);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerFull, 90, 2, 7);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerFull, 180, 0, 7);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CornerFull, 270, 0, 5);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TEmpty, 0, 6, 7);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TEmpty, 90, 7, 7);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TEmpty, 180, 5, 7);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TEmpty, 270, 7, 6);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullA, 0, 6, 4);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullA, 90, 6, 3);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullA, 180, 5, 5);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullA, 270, 5, 2);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullB, 0, 6, 5);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullB, 90, 5, 3);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullB, 180, 5, 4);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.THalfFullB, 270, 6, 2);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TFull, 0, 2, 6);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TFull, 90, 1, 7);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TFull, 180, 0, 6);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.TFull, 270, 1, 5);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterQuarterFull, 0, 1, 1);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterQuarterFull, 90, 1, 2);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterQuarterFull, 180, 0, 2);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterQuarterFull, 270, 0, 1);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterHalfFullStraight, 0, 2, 0);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterHalfFullStraight, 90, 1, 0);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterHalfFullStraight, 180, 2, 1);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterHalfFullStraight, 270, 0, 0);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterHalfFullCorners, 0, 3, 2);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.CenterHalfFullCorners, 90, 2, 2);

        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.Center3QuartersFull, 0, 3, 3);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.Center3QuartersFull, 90, 3, 4);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.Center3QuartersFull, 180, 2, 4);
        AddTileToSlicedSet(generator, tileset, texture, tileSize, tileset.Center3QuartersFull, 270, 2, 3);
    }

    private static void AddTileToSlicedSet(TilemapGenerator generator, TileSetSlicedFull tileset, Texture2D texture, int tileSize, Dictionary<int, TileBase> slicedTiles, int rotation, int x, int y)
    {
        TileBase tile = GetTileAt(texture, tileSize, x, y);
        slicedTiles.Add(rotation, tile);      
    }

    public static Tile GetTileAt(Texture2D texture, int tileSize, int x, int y)
    {
        Tile tile = new Tile();
        tile.name = texture.name + "_" + x + "_" + y;

        tile.sprite = Sprite.Create(texture, new Rect(x * tileSize, y * tileSize, tileSize, tileSize), new Vector2(0.5f, 0.5f), TilemapGenerator.TilePixelSize, 1, SpriteMeshType.Tight, Vector4.zero);
        return tile;
    }

    private static Tile GetOverlayTileAt(int tileSize, Texture2D baseTexture, int baseX, int baseY, Texture2D overlayTexture, int overlayX, int overlayY)
    {
        Texture2D tileTexture = new Texture2D(tileSize, tileSize);
        tileTexture.filterMode = FilterMode.Point;
        for(int y = 0; y < tileSize; y++)
        {
            for(int x = 0; x < tileSize; x++)
            {
                Color baseColor = baseTexture.GetPixel(baseX * tileSize + x, baseY * tileSize + y);
                Color overlayColor = overlayTexture.GetPixel(overlayX * tileSize + x, overlayY * tileSize + y);

                Color tileTextureColor = overlayColor;
                if (overlayColor.a == 0) tileTextureColor = baseColor;

                tileTexture.SetPixel(x, y, tileTextureColor);
            }
        }
        tileTexture.Apply();

        Tile tile = new Tile();
        tile.sprite = Sprite.Create(tileTexture, new Rect(0, 0, tileSize, tileSize), new Vector2(0.5f, 0.5f), tileSize, 1, SpriteMeshType.Tight, Vector4.zero);
        return tile;
    }

}
