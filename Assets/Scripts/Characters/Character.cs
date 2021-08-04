using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Character : MonoBehaviour
{
    public GameModel Model;

    public TileData CurrentTile; // The data of the exact tile the character is on at this moment
    public Vector2Int GridPosition; // The position of the character after he is done moving

    public float MovementSpeed;
    public CharacterController Controller;

    void Awake()
    {
        Controller = GetComponentInChildren<CharacterController>();
    }

    public void Init(GameModel model, int x, int y)
    {
        Model = model;
        GridPosition = new Vector2Int(x, y);
    }
}
