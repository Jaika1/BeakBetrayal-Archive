using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DebugMenu : MonoBehaviour
{
    private Gamepad pad;
    private int playerIndex = 0;
    public Text CurrentPlayerText;

    void Awake()
    {
        //Destroy this if we're not inside of the editor.
#if !UNITY_EDITOR
        Destroy(gameObject);
#endif
        if (Gamepad.current.yButton.isPressed)
        {
            EntityInformation.CurrentHumanPlayers = 1;
            for (int i = 0; i < EntityInformation.CurrentHumanPlayers; ++i)
            {
                GameNFO.EntityNFOs[i].PlayerGamepad = new Gamepad();
            }
        }
        else if(Gamepad.current.bButton.isPressed)
        {
            EntityInformation.CurrentHumanPlayers = 2;
            for (int i = 0; i < EntityInformation.CurrentHumanPlayers; ++i)
            {
                GameNFO.EntityNFOs[i].PlayerGamepad = new Gamepad();
            }
        }
        else if (Gamepad.current.aButton.isPressed)
        {
            EntityInformation.CurrentHumanPlayers = 3;
            for (int i = 0; i < EntityInformation.CurrentHumanPlayers; ++i)
            {
                GameNFO.EntityNFOs[i].PlayerGamepad = new Gamepad();
            }
        }
        else if (Gamepad.current.xButton.isPressed)
        {
            EntityInformation.CurrentHumanPlayers = 4;
            for (int i = 0; i < EntityInformation.CurrentHumanPlayers; ++i)
            {
                GameNFO.EntityNFOs[i].PlayerGamepad = new Gamepad();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pad = Gamepad.all[0];
        GameNFO.EntityNFOs[playerIndex].PlayerGamepad = pad;
        CurrentPlayerText.text = $"Player: {playerIndex + 1}";
    }

    // Update is called once per frame
    void Update()
    {
        pad = Gamepad.all[0];
        if (pad.leftShoulder.wasPressedThisFrame)
        {
            GameNFO.EntityNFOs[playerIndex].PlayerGamepad = new Gamepad();
            playerIndex--;
            if (playerIndex < 0) playerIndex = EntityInformation.CurrentHumanPlayers - 1;

            UpdateFields();
        }
        else if (pad.rightShoulder.wasPressedThisFrame)
        {
            GameNFO.EntityNFOs[playerIndex].PlayerGamepad = new Gamepad();
            playerIndex++;
            if (playerIndex >= EntityInformation.CurrentHumanPlayers) playerIndex = 0;

            UpdateFields();
        }
    }

    void UpdateFields()
    {
        GameNFO.EntityNFOs[playerIndex].PlayerGamepad = pad;
        CurrentPlayerText.text = $"Player: {playerIndex + 1}";
    }
}
