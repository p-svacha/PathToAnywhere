using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public GameModel Model;

    public Texture2D CharacterBodiesTexture;
    public Texture2D CharacterHeadsTexture;
    public Texture2D CharacterHairsTexture;

    public const string PlayerLayerName = "Player";

    private const int BodyPixelSize = 128;
    private List<List<Sprite>> CharacterBodySprites;
    private Dictionary<Direction, Vector3> BodyPositions = new Dictionary<Direction, Vector3>()
    {
        {Direction.N, new Vector3(0f, 0f, 0f) },
        {Direction.E, new Vector3(0f, 0f, 0f) },
        {Direction.S, new Vector3(0f, 0f, 0f) },
        {Direction.W, new Vector3(0f, 0f, 0f) }
    };

    private const int HeadPixelSize = 64;
    private List<List<Sprite>> CharacterHeadSprites;
    private Dictionary<Direction, Vector3> HeadPositions = new Dictionary<Direction, Vector3>()
    {
        {Direction.N, new Vector3(0f, 0.5f, 0f) },
        {Direction.E, new Vector3(0.1f, 0.5f, 0f) },
        {Direction.S, new Vector3(0f, 0.5f, 0f)},
        {Direction.W, new Vector3(-0.1f, 0.5f, 0f)}
    };

    private const int HairPixelSize = 64;
    private List<List<Sprite>> CharacterHairSprites;

    public void Init(GameModel model)
    {
        Model = model;

        CharacterBodySprites = LoadBodyPartAtlas(CharacterBodiesTexture, BodyPixelSize, BodyPixelSize);
        CharacterHeadSprites = LoadBodyPartAtlas(CharacterHeadsTexture, HeadPixelSize, BodyPixelSize);
        CharacterHairSprites = LoadBodyPartAtlas(CharacterHairsTexture, HairPixelSize, BodyPixelSize);
    }

    private List<List<Sprite>> LoadBodyPartAtlas(Texture2D atlas, int spritePixelSize, int referencePixelSize)
    {
        List<List<Sprite>> sprites = new List<List<Sprite>>();
        int numSprites = atlas.height / spritePixelSize;
        for (int i = 0; i < numSprites; i++)
        {
            List<Sprite> partSprites = new List<Sprite>();
            partSprites.Add(Sprite.Create(atlas, new Rect(0, i * spritePixelSize, spritePixelSize, spritePixelSize), new Vector2(0.5f, 0.5f), referencePixelSize, 1, SpriteMeshType.Tight, Vector4.zero));
            partSprites.Add(Sprite.Create(atlas, new Rect(spritePixelSize, i * spritePixelSize, spritePixelSize, spritePixelSize), new Vector2(0.5f, 0.5f), referencePixelSize, 1, SpriteMeshType.Tight, Vector4.zero));
            partSprites.Add(Sprite.Create(atlas, new Rect(2 * spritePixelSize, i * spritePixelSize, spritePixelSize, spritePixelSize), new Vector2(0.5f, 0.5f), referencePixelSize, 1, SpriteMeshType.Tight, Vector4.zero));
            sprites.Add(partSprites);
        }
        return sprites;
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

        float bodyScale = 1f * BodyPixelSize / BodyPixelSize;
        float headScale = 1f * HeadPixelSize / BodyPixelSize;

        BodyPart body = AddRandomBodyPart(ColorManager.GetRandomColor(), bodyParts, CharacterBodySprites, bodyScale, BodyPositions); // Body
        BodyPart head = AddRandomBodyPart(ColorManager.GetRandomSkinColor(), bodyParts, CharacterHeadSprites, headScale, HeadPositions); // Head
        BodyPart hair = AddRandomBodyPart(ColorManager.GetRandomColor(), bodyParts, CharacterHairSprites, headScale, HeadPositions); // Hair

        CharacterAppearance appearance = new CharacterAppearance(body, head, hair);

        // Character
        Character character = null;
        if (isPlayer)
        {
            Player player = characterObject.AddComponent<Player>();
            player.Init(Model, gridPosition, (PlayerController) controller, movePoint.transform, appearance);
            player.Name = "Player";
            character = player;
        }
        else
        {
            NPC npc = characterObject.AddComponent<NPC>();
            npc.Init(Model, gridPosition, (NPCController) controller, movePoint.transform, appearance);
            npc.Name = NameGenerator.GenerateName(NameGenerationType.Character);
            character = npc;
        }

        return character;
    }

    private BodyPart AddRandomBodyPart(Color color, GameObject bodyParts, List<List<Sprite>> sprites, float spriteScale, Dictionary<Direction, Vector3> spritePositions)
    {
        GameObject bodyPartObject = new GameObject("BodyPart");
        bodyPartObject.transform.SetParent(bodyParts.transform);
        BodyPart bodyPart = bodyPartObject.AddComponent<BodyPart>();
        List<Sprite> chosenSprites = sprites[Random.Range(0, sprites.Count)];
        bodyPart.Init(color, chosenSprites[0], chosenSprites[1], chosenSprites[2], spriteScale, spritePositions);
        return bodyPart;
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
