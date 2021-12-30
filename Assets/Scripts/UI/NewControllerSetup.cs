using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class NewControllerSetup : MonoBehaviour
{
    public bool ResetListOnAwake = false;
    public bool FreezeTimeOnDisconnect = true;
    public string DisconnectedText = "Not Connected";
    public Text[] InfoText = new Text[4];
    public UnityEvent FireOnPlayerReady;
    public UnityEvent FireOnBackOut;

    void Awake()
    {
        if (ResetListOnAwake)
        {
            GameNFO.EntityNFOs = new EntityInformation[GameNFO.MaxPlayers];
            for (int i = 0; i < GameNFO.MaxPlayers; ++i)
            {
                GameNFO.EntityNFOs[i] = new EntityInformation();
            }
            EntityInformation.CurrentHumanPlayers = 0;
        }

        Array.ForEach(InfoText, x=>x.text = DisconnectedText);
        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
    }

    private void Update()
    {
        ReadOnlyArray<Gamepad> pads = Gamepad.all;
        if (EntityInformation.CurrentHumanPlayers < 4)
        {
            for (int pi = 0; pi < pads.Count; ++pi)
            {
                Gamepad pad = pads[pi];

                if (GameNFO.EntityNFOs.Count(x => x.PlayerGamepad?.deviceId == pad.deviceId) == 0)
                {
                    if (pad.aButton.wasPressedThisFrame)
                    {
                        int firstOpenSlotIndex = Array.FindIndex(GameNFO.EntityNFOs, x => x.PlayerGamepad == null);
                        GameNFO.EntityNFOs[firstOpenSlotIndex].PlayerGamepad = pad;
                        InfoText[firstOpenSlotIndex].text = $"[{pad.deviceId}] {pad.displayName}";
                        EntityInformation.CurrentHumanPlayers++;
                    }
                }
                else
                {

                }
            }
        }

        if ((GameNFO.EntityNFOs[0].PlayerGamepad?.startButton.wasPressedThisFrame).GetValueOrDefault())
        {
            GameNFO.SortPlayerList();
            for (int i = 0; i < GameNFO.HumanPlayerLimit; ++i)
            {
                Gamepad pad = GameNFO.EntityNFOs[i].PlayerGamepad;
                InfoText[i].text = pad != null ? $"[{pad.deviceId}] {pad.displayName}" : DisconnectedText;
            }
            for (byte i = 0; i < GameNFO.MaxPlayers; ++i)
            {
                GameNFO.EntityNFOs[i].EntityId = i;
            }
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
            FireOnPlayerReady.Invoke();
        }

        for (int i = 0; i < pads.Count; ++i)
        {
            Gamepad pad = pads[i];
            if (pad == null) continue;
            if (pad.bButton.wasPressedThisFrame)
            {
                if (GameNFO.EntityNFOs.Count(x=>x.PlayerGamepad?.deviceId == pad.deviceId) > 0)
                {
                    DisconnectGamepad(pad);
                }
                else
                {
                    FireOnBackOut.Invoke();
                }
            }
        }
    }

    private void InputSystem_onDeviceChange(InputDevice device, InputDeviceChange change)
    {
        Gamepad pad = (Gamepad)device;
        if (pad == null) return;
        if (change == InputDeviceChange.Removed)
        {
            DisconnectGamepad(pad);
        }
    }

    void DisconnectGamepad(Gamepad pad)
    {
        int padIndex = Array.FindIndex(GameNFO.EntityNFOs, x => x.PlayerGamepad?.deviceId == pad.deviceId);
        if (padIndex != -1)
        {
            if (FreezeTimeOnDisconnect) Time.timeScale = 0.0f;
            InfoText[padIndex].text = DisconnectedText;
            GameNFO.EntityNFOs[padIndex].PlayerGamepad = null;
            EntityInformation.CurrentHumanPlayers--;
            gameObject.SetActive(true);
        }
    }
}
