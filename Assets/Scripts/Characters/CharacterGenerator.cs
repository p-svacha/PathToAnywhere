using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public GameModel Model;

    public Texture2D CharacterBodies;
    public Texture2D CharacterHeads;

    public const string PlayerLayerName = "Player";

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

    private Character GenerateCharacter(Vector2Int gridPosition, bool isPlayer = false)
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
        else controller = bodyParts.AddComponent<NPCController>();

        // Body
        Color bodyColor = ColorManager.GetRandomColor();
        GameObject characterBodyObject = new GameObject("Body");
        characterBodyObject.transform.SetParent(bodyParts.transform);
        BodyPart characterBody = characterBodyObject.AddComponent<BodyPart>();
        List<Sprite> bodySprites = CharacterBodySprites[Random.Range(0, CharacterBodySprites.Count)];
        characterBody.Init(bodyColor, bodySprites[0], bodySprites[1], bodySprites[2]);

        // Head
        Color headColor = ColorManager.GetRandomSkinColor();
        GameObject characterHeadObject = new GameObject("Head");
        characterHeadObject.transform.SetParent(bodyParts.transform);
        BodyPart characterHead = characterHeadObject.AddComponent<BodyPart>();
        Sprite headSprite = CharacterHeadSprites[Random.Range(0, CharacterHeadSprites.Count)];
        characterHead.Init(headColor, headSprite, headSprite, headSprite);

        // Character
        Character character = null;
        if (isPlayer)
        {
            Player player = characterObject.AddComponent<Player>();
            player.Init(Model, gridPosition, (PlayerController) controller, movePoint.transform, characterBody, characterHead);
            player.Name = "Player";
            character = player;
        }
        else
        {
            NPC npc = characterObject.AddComponent<NPC>();
            npc.Init(Model, gridPosition, (NPCController) controller, movePoint.transform, characterBody, characterHead);
            npc.Name = NameGenerator.GenerateName(NameGenerationType.Character);
            character = npc;
        }

        return character;
    }

    public Player GeneratePlayer(Vector2Int gridPosition)
    {
        return (Player)GenerateCharacter(gridPosition, isPlayer: true);
    }
    public NPC GenerateNPC(Vector2Int gridPosition)
    {
        return (NPC)GenerateCharacter(gridPosition, isPlayer: false);
    }
}
