using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WispController : MonoBehaviour
{
    public EntityInformation EntityNfo;
    [HideInInspector]
    public Entity TargetEntity;
    public Image[] AbilityImages = new Image[4];
    public AudioClip UseSound;
    private float AbilityCooldown = 0.0f;

    void Update()
    {
        if (AbilityCooldown <= 0.0f)
        {
            Gamepad pad = EntityNfo.PlayerGamepad;
            if (pad == null) return;
            if (!pad.added) return;
            if (pad.yButton.wasPressedThisFrame)
            {
                // Ability 1
                TargetEntity.WispModifiers.SpeedAbilityDuration = WispAbilties.SpeedAbilityEffect;
                AbilityCooldown = WispAbilties.SpeedAbilityCoolDown;
                Array.ForEach(AbilityImages, x=>x.color = new Color(x.color.r, x.color.g, x.color.b, 0.5f));
                AudioSource.PlayClipAtPoint(UseSound, Vector3.zero);
            }
            if (pad.bButton.wasPressedThisFrame)
            {
                // Ability 2    
                TargetEntity.WispModifiers.FireAbilityDuration = WispAbilties.FireAbilityEffect;
                AbilityCooldown = WispAbilties.FireAbilityCoolDown;
                Array.ForEach(AbilityImages, x => x.color = new Color(x.color.r, x.color.g, x.color.b, 0.5f));
                AudioSource.PlayClipAtPoint(UseSound, Vector3.zero);
            }
            if (pad.aButton.wasPressedThisFrame)
            {
                // Ability 3
                TargetEntity.WispModifiers.VulnerabilityAbilityDuration = WispAbilties.VulnerabilityAbilityEffect;
                AbilityCooldown = WispAbilties.VulnerabilityAbilityCoolDown;
                Array.ForEach(AbilityImages, x => x.color = new Color(x.color.r, x.color.g, x.color.b, 0.5f));
                AudioSource.PlayClipAtPoint(UseSound, Vector3.zero);
            }
            if (pad.xButton.wasPressedThisFrame)
            {
                // Ability 4
                TargetEntity.WispModifiers.FoVAbilityDuration = WispAbilties.FoVAbilityEffect;
                AbilityCooldown = WispAbilties.FoVAbilityCoolDown;
                Array.ForEach(AbilityImages, x => x.color = new Color(x.color.r, x.color.g, x.color.b, 0.5f));
                AudioSource.PlayClipAtPoint(UseSound, Vector3.zero);
            }
        }
        else
        {
            AbilityCooldown -= Time.deltaTime;
            if (AbilityCooldown <= 0.0f)
            {
                Array.ForEach(AbilityImages, x => x.color = new Color(x.color.r, x.color.g, x.color.b, 1.0f));
                AudioSource.PlayClipAtPoint(UseSound, Vector3.zero);
            }
        }
    }
}
