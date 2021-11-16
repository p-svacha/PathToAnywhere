using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileGenerator
{
    public static TileSetSimple GenerateSimpleTilset(Texture2D texture, TileSetData data, int tileSize, int rows, int cols)
    {
        TileSetSimple tileset = new TileSetSimple(data);

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

    public static TileSetSimple GenerateAnimatedSimpleTileset(Texture2D texture, TileSetData data, int tileSize, int numTiles, int animationLength, float animationSpeed)
    {
        TileSetSimple tileset = new TileSetSimple(data);

        for (int i = 0; i < numTiles; i++)
        {
            AnimatedTile tile = GetAnimatedTile(texture, tileSize, animationLength, animationSpeed, 0, i, 1, 0);
            tileset.Tiles.Add(tile);
        }
        return tileset;
    }

    public static TileSetSliced GenerateSlicedTileset(Texture2D texture, TileSetData data, int tileSize, List<BaseFeatureType> connectionTypes)
    {
        TileSetSliced tileset = new TileSetSliced(connectionTypes, data);
        AddTilesToSlicedSet(tileset, texture, tileSize);
        return tileset;
    }

    public static TileSetSliced GenerateAnimatedSlicedTileset(Texture2D texture, TileSetData data, int tileSize, List<BaseFeatureType> connectionTypes, float animationSpeed)
    {
        TileSetSliced tileset = new TileSetSliced(connectionTypes, data);
        int animationLength = texture.width / 512;
        AddAnimatedTilesToSlicedSet(tileset, texture, tileSize, animationLength, animationSpeed);
        return tileset;
    }

    private static void AddTilesToSlicedSet(TileSetSliced tileset, Texture2D texture, int tileSize)
    {
        AddTileToSlicedSet(texture, tileSize, tileset.Surrounded, 0, 1, 6);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterEmpty, 0, 4, 6);
        AddTileToSlicedSet(texture, tileSize, tileset.Single, 0, 3, 7);

        AddTileToSlicedSet(texture, tileSize, tileset.Straight, 0, 5, 6);
        AddTileToSlicedSet(texture, tileSize, tileset.Straight, 90, 4, 5);

        AddTileToSlicedSet(texture, tileSize, tileset.End, 0, 4, 4);
        AddTileToSlicedSet(texture, tileSize, tileset.End, 90, 6, 6);
        AddTileToSlicedSet(texture, tileSize, tileset.End, 180, 4, 7);
        AddTileToSlicedSet(texture, tileSize, tileset.End, 270, 3, 6);

        AddTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 0, 1, 3);
        AddTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 90, 1, 4);
        AddTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 180, 0, 4);
        AddTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 270, 0, 3);

        AddTileToSlicedSet(texture, tileSize, tileset.CornerFull, 0, 2, 5);
        AddTileToSlicedSet(texture, tileSize, tileset.CornerFull, 90, 2, 7);
        AddTileToSlicedSet(texture, tileSize, tileset.CornerFull, 180, 0, 7);
        AddTileToSlicedSet(texture, tileSize, tileset.CornerFull, 270, 0, 5);

        AddTileToSlicedSet(texture, tileSize, tileset.TEmpty, 0, 6, 7);
        AddTileToSlicedSet(texture, tileSize, tileset.TEmpty, 90, 7, 7);
        AddTileToSlicedSet(texture, tileSize, tileset.TEmpty, 180, 5, 7);
        AddTileToSlicedSet(texture, tileSize, tileset.TEmpty, 270, 7, 6);

        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 0, 6, 4);
        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 90, 6, 3);
        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 180, 5, 5);
        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 270, 5, 2);

        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 0, 6, 5);
        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 90, 5, 3);
        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 180, 5, 4);
        AddTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 270, 6, 2);

        AddTileToSlicedSet(texture, tileSize, tileset.TFull, 0, 2, 6);
        AddTileToSlicedSet(texture, tileSize, tileset.TFull, 90, 1, 7);
        AddTileToSlicedSet(texture, tileSize, tileset.TFull, 180, 0, 6);
        AddTileToSlicedSet(texture, tileSize, tileset.TFull, 270, 1, 5);

        AddTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 0, 1, 1);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 90, 1, 2);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 180, 0, 2);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 270, 0, 1);

        AddTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 0, 2, 0);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 90, 1, 0);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 180, 2, 1);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 270, 0, 0);

        AddTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullCorners, 0, 3, 2);
        AddTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullCorners, 90, 2, 2);

        AddTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 0, 3, 3);
        AddTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 90, 3, 4);
        AddTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 180, 2, 4);
        AddTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 270, 2, 3);
    }

    private static void AddAnimatedTilesToSlicedSet(TileSetSliced tileset, Texture2D texture, int tileSize, int animationLength, float animationSpeed)
    {
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Surrounded, 0, 1, 6, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterEmpty, 0, 4, 6, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Single, 0, 3, 7, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Straight, 0, 5, 6, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Straight, 90, 4, 5, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.End, 0, 4, 4, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.End, 90, 6, 6, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.End, 180, 4, 7, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.End, 270, 3, 6, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 0, 1, 3, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 90, 1, 4, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 180, 0, 4, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerEmpty, 270, 0, 3, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerFull, 0, 2, 5, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerFull, 90, 2, 7, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerFull, 180, 0, 7, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CornerFull, 270, 0, 5, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TEmpty, 0, 6, 7, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TEmpty, 90, 7, 7, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TEmpty, 180, 5, 7, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TEmpty, 270, 7, 6, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 0, 6, 4, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 90, 6, 3, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 180, 5, 5, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullA, 270, 5, 2, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 0, 6, 5, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 90, 5, 3, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 180, 5, 4, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.THalfFullB, 270, 6, 2, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TFull, 0, 2, 6, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TFull, 90, 1, 7, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TFull, 180, 0, 6, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.TFull, 270, 1, 5, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 0, 1, 1, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 90, 1, 2, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 180, 0, 2, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterQuarterFull, 270, 0, 1, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 0, 2, 0, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 90, 1, 0, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 180, 2, 1, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullStraight, 270, 0, 0, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullCorners, 0, 3, 2, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.CenterHalfFullCorners, 90, 2, 2, animationLength, animationSpeed);

        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 0, 3, 3, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 90, 3, 4, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 180, 2, 4, animationLength, animationSpeed);
        AddAnimatedTileToSlicedSet(texture, tileSize, tileset.Center3QuartersFull, 270, 2, 3, animationLength, animationSpeed);
    }

    private static void AddTileToSlicedSet(Texture2D texture, int tileSize, Dictionary<int, TileBase> slicedTiles, int rotation, int x, int y)
    {
        TileBase tile = GetTileAt(texture, tileSize, x, y);
        slicedTiles.Add(rotation, tile);      
    }

    private static void AddAnimatedTileToSlicedSet(Texture2D texture, int tileSize, Dictionary<int, TileBase> slicedTiles, int rotation, int x, int y, int animationLength, float animationSpeed)
    {
        AnimatedTile tile = GetAnimatedTile(texture, tileSize, animationLength, animationSpeed, x, y, 8, 0);
        slicedTiles.Add(rotation, tile);
    }

    public static AnimatedTile GetAnimatedTile(Texture2D texture, int tileSize, int animationLength, float animationSpeed, int startX, int startY, int deltaX, int deltaY)
    {
        AnimatedTile tile = ScriptableObject.CreateInstance<AnimatedTile>();
        List<Sprite> frames = new List<Sprite>();
        for(int i = 0; i < animationLength; i++)
        {
            Sprite frame = Sprite.Create(texture, new Rect((startX + i * deltaX) * tileSize, (startY + i * deltaY) * tileSize, tileSize, tileSize), new Vector2(0.5f, 0.5f), MapGenerator.TilePixelSize, 1, SpriteMeshType.Tight, Vector4.zero);
            frames.Add(frame);
        }
        tile.m_AnimatedSprites = frames.ToArray();
        tile.m_MinSpeed = animationSpeed;
        tile.m_MaxSpeed = animationSpeed;
        return tile;
    }

    public static Tile GetTileAt(Texture2D texture, int tileSize, int x, int y)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.name = texture.name + "_" + x + "_" + y;

        tile.sprite = Sprite.Create(texture, new Rect(x * tileSize, y * tileSize, tileSize, tileSize), new Vector2(0.5f, 0.5f), MapGenerator.TilePixelSize, 1, SpriteMeshType.Tight, Vector4.zero);
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
