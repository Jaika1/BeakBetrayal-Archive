using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityInformation
{
    public static int CurrentHumanPlayers = 0;

    public byte EntityId;
    public Gamepad PlayerGamepad = null;

    public int Score = 0;
}
