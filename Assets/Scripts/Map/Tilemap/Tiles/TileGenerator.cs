using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileGenerator
{
    public static TileSetSimple GenerateSimpleTilset(TilemapGenerator generator, Texture2D texture, TileType type, TileSetData data, int tileSize)
    {
        TileSetSimple tileset = new TileSetSimple(data, type);

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

        generator.TileSets.Add(type, tileset);

        return tileset;
    }

    public static TileSetSliced GenerateSlicedTileset(Texture2D texture, int tileSize)
    {
        TileSetSliced tileset = new TileSetSliced();
        AssignTilesToSlicedSet(tileset, texture, tileSize);
        return tileset;
    }

    public static TileSetSliced GenerateSlicedTileset(TilemapGenerator generator, Texture2D texture, TileType type, TileSetData data, int tileSize)
    {
        TileSetSliced tileset = new TileSetSliced(data, type);
        AssignTilesToSlicedSet(tileset, texture, tileSize);
        generator.TileSets.Add(type, tileset);

        return tileset;
    }

    private static void AssignTilesToSlicedSet(TileSetSliced tileset, Texture2D texture, int tileSize)
    {
        tileset.Surrounded = GetTileAt(texture, tileSize, 0, 3);
        tileset.Center1 = GetTileAt(texture, tileSize, 1, 3);
        tileset.Center2 = GetTileAt(texture, tileSize, 2, 3);
        tileset.Center3 = GetTileAt(texture, tileSize, 3, 3);

        tileset.Center4 = GetTileAt(texture, tileSize, 0, 2);
        tileset.T1 = GetTileAt(texture, tileSize, 1, 2);
        tileset.T2 = GetTileAt(texture, tileSize, 2, 2);
        tileset.T3 = GetTileAt(texture, tileSize, 3, 2);

        tileset.Single = GetTileAt(texture, tileSize, 0, 1);
        tileset.Center0 = GetTileAt(texture, tileSize, 1, 1);
        tileset.T0 = GetTileAt(texture, tileSize, 2, 1);
        tileset.Corner1 = GetTileAt(texture, tileSize, 3, 1);

        tileset.End = GetTileAt(texture, tileSize, 0, 0);
        tileset.Straight = GetTileAt(texture, tileSize, 1, 0);
        tileset.Corner0 = GetTileAt(texture, tileSize, 2, 0);
    }

    public static TileSetSimple GenerateSimpleOverlayTileset(TilemapGenerator generator, Texture2D baseTexture, Texture2D overlayTexture, TileType type, TileSetData data, int tileSize)
    {
        TileSetSimple tileset = new TileSetSimple(data, type);

        int baseCols = baseTexture.width / tileSize;
        int baseRows = baseTexture.height / tileSize;

        int overlayCols = overlayTexture.width / tileSize;
        int overlayRows = overlayTexture.height / tileSize;

        for (int yb = 0; yb < baseRows; yb++)
        {
            for (int xb = 0; xb < baseCols; xb++)
            {
                for(int yo = 0; yo < overlayRows; yo++)
                {
                    for(int xo = 0; xo < overlayCols; xo++)
                    {
                        Tile tile = GetOverlayTileAt(tileSize, baseTexture, xb, yb, overlayTexture, xo, yo);
                        tileset.Tiles.Add(tile);
                    }
                }
            }
        }

        generator.TileSets.Add(type, tileset);

        Debug.Log("Created overlay tileset with " + tileset.Tiles.Count + " tile variants");

        return tileset;
    }

    public static Tile GetTileAt(Texture2D texture, int tileSize, int x, int y)
    {
        Tile tile = new Tile();
        tile.sprite = Sprite.Create(texture, new Rect(x * tileSize, y * tileSize, tileSize, tileSize), new Vector2(0.5f, 0.5f), tileSize, 1, SpriteMeshType.Tight, Vector4.zero);
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
