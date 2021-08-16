using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public GameModel Model;

    public Texture2D CharacterBodies;
    public Texture2D CharacterHeads;

    private const string PlayerLayerName = "Player";

    private const int BodyPixelSize = 128;
    private List<List<Sprite>> CharacterBodySprites;

    private const int HeadPixelSize = 64;
    private List<Sprite> CharacterHeadSprites;

    public void Init(GameModel model)
    {
        Model = model;

        CharacterBodySprites = new List<List<Sprite>>();
        CharacterHeadSprites = new List<Sprite>();

        int numBodies = CharacterBodies.height / BodyPixelSize;
        for(int i = 0; i < numBodies; i++)
        {
            List<Sprite> bodySprites = new List<Sprite>();
            bodySprites.Add(Sprite.Create(CharacterBodies, new Rect(0, i * BodyPixelSize, BodyPixelSize, BodyPixelSize), new Vector2(0.5f, 0.5f), BodyPixelSize, 1, SpriteMeshType.Tight, Vector4.zero));
            bodySprites.Add(Sprite.Create(CharacterBodies, new Rect(BodyPixelSize, i * BodyPixelSize, BodyPixelSize, BodyPixelSize), new Vector2(0.5f, 0.5f), BodyPixelSize, 1, SpriteMeshType.Tight, Vector4.zero));
            bodySprites.Add(Sprite.Create(CharacterBodies, new Rect(2 * BodyPixelSize, i * BodyPixelSize, BodyPixelSize, BodyPixelSize), new Vector2(0.5f, 0.5f), BodyPixelSize, 1, SpriteMeshType.Tight, Vector4.zero));
            CharacterBodySprites.Add(bodySprites);
        }

        int numHeads = CharacterHeads.width / HeadPixelSize;
        for(int i = 0; i < numHeads; i++)
        {
            CharacterHeadSprites.Add(Sprite.Create(CharacterHeads, new Rect(i * HeadPixelSize, 0, HeadPixelSize, HeadPixelSize), new Vector2(0.5f, 0.5f), BodyPixelSize, 1, SpriteMeshType.Tight, Vector4.zero));
        }
    }

    public Character GenerateCharacter(Vector2Int gridPosition, bool isPlayer = false)
    {
        GameObject characterObject = new GameObject("Character");

        // Move Point
        GameObject movePoint = new GameObject("MovePoint");
        movePoint.transform.SetParent(characterObject.transform);

        // Body Parts
        GameObject bodyParts = new GameObject("BodyParts");
        bodyParts.transform.SetParent(characterObject.transform);
        CharacterController controller;
        if(isPlayer) controller = bodyParts.AddComponent<PlayerController>();
        else controller = bodyParts.AddComponent<CharacterController>();

        // Body
        Color bodyColor = ColorManager.GetRandomColor();
        GameObject characterBodyObject = new GameObject("Body");
        characterBodyObject.transform.SetParent(bodyParts.transform);
        BodyPart characterBody = characterBodyObject.AddComponent<BodyPart>();
        List<Sprite> bodySpritePrefabs = CharacterBodySprites[Random.Range(0, CharacterBodySprites.Count)];
        List<SpriteRenderer> bodySprites = new List<SpriteRenderer>();
        foreach (Sprite bodySpritePrefab in bodySpritePrefabs)
        {
            GameObject bodyRendererObject = new GameObject("BodyRenderer");
            bodyRendererObject.transform.SetParent(characterBodyObject.transform);
            SpriteRenderer bodyRenderer = bodyRendererObject.AddComponent<SpriteRenderer>();
            bodyRenderer.sprite = bodySpritePrefab;
            bodyRenderer.color = bodyColor;
            bodyRenderer.sortingLayerName = PlayerLayerName;
            bodySprites.Add(bodyRenderer);
        }
        characterBody.Init(bodySprites[0], bodySprites[1], bodySprites[2]);

        // Head
        Color headColor = ColorManager.GetRandomSkinColor();
        GameObject characterHeadObject = new GameObject("Head");
        characterHeadObject.transform.SetParent(bodyParts.transform);
        BodyPart characterHead = characterHeadObject.AddComponent<BodyPart>();
        Sprite headSpritePrefab = CharacterHeadSprites[Random.Range(0, CharacterHeadSprites.Count)];
        List<SpriteRenderer> headSprites = new List<SpriteRenderer>();
        for(int i = 0; i < 3; i++)
        {
            GameObject headRendererObject = new GameObject("HeadRenderer");
            headRendererObject.transform.SetParent(characterHeadObject.transform);
            SpriteRenderer headRenderer = headRendererObject.AddComponent<SpriteRenderer>();
            headRenderer.sprite = headSpritePrefab;
            headRenderer.color = headColor;
            headRenderer.sortingLayerName = PlayerLayerName;
            headSprites.Add(headRenderer);
        }
        characterHead.Init(headSprites[0], headSprites[1], headSprites[2]);

        // Character
        Character character = null;
        if (isPlayer)
        {
            Player player = characterObject.AddComponent<Player>();
            player.Init(Model, gridPosition, (PlayerController) controller, movePoint.transform, characterBody, characterHead);
            character = player;
        }
        else
        {
            character = characterObject.AddComponent<Character>();
            character.Init(Model, gridPosition, controller, movePoint.transform, characterBody, characterHead);
        }

        return character;
    }

    public Player GeneratePlayer(Vector2Int gridPosition)
    {
        return (Player)GenerateCharacter(gridPosition, isPlayer: true);
    }
}
