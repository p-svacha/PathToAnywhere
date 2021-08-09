using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public PlayerController PlayerController;

    public override void Awake()
    {
        base.Awake();
        PlayerController = GetComponentInChildren<PlayerController>();
    }
}
