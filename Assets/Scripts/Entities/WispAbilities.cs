using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region old

//public interface WispAbilitys
//{

//    string AbilityName { get; }
//    string Description { get; }
//    float AbilityDuration { get; }
//    AbilityModifiers Modifers { get; }
//}

//public class AbilityModifiers 
//{
//    /// <summary>
//    /// slow the players speed
//    /// </summary>
//    [AbitlityModifiersType(AbilityModifersType.Multiplicative)]
//    public float SpeedModifier = 0.0f;
//    // how would i do say 30% slow speed?

//    /// <summary>
//    /// "jam guns" stops player from shooting
//    /// </summary>
//    [AbitlityModifiersType(AbilityModifersType.Subtractive)]


//    /// <summary>
//    /// "damage Vunerability" MORE DAMAGE
//    /// </summary>
//    [AbitlityModifiersType(AbilityModifersType.Multiplicative)]

//}

//public class AbitlityModifiersTypeAttribute : Attribute
//{
//    public AbilityModifersType ModifierType;

//    public AbitlityModifiersTypeAttribute(AbilityModifersType type)
//    {
//        ModifierType = type;
//    }
//}

//public enum AbilityModifersType
//{
//    Additive,
//    Subtractive,
//    Multiplicative
//}

#endregion

public class WispAbilties
{
    public const float SpeedModifier = 0.6f;
    public float SpeedAbilityDuration = 0f;
    public const float SpeedAbilityEffect = 8.0f;
    public const float SpeedAbilityCoolDown = 20.0f;

    public float FireAbilityDuration = 0f;
    public const float FireAbilityEffect = 5.0f;
    public const float FireAbilityCoolDown = 20.0f;

    public const float VulnerabilityModifier = 1.3f;
    public float VulnerabilityAbilityDuration = 0f;
    public const float VulnerabilityAbilityEffect = 10.0f;
    public const float VulnerabilityAbilityCoolDown = 25.0f;

    public const float FoVValue = 45f;
    public float FoVAbilityDuration = 0f;
    public bool FoVAbilityInEffect = false;
    public const float FoVAbilityEffect = 5.0f;
    public const float FoVAbilityCoolDown = 20.0f;
}


//Ability one: Player Slow:
//-40% move speed, lasts 5 seconds.
//Ability two: Gun Jam:
//Player cannot fire for 2-4? seconds.
//Ability three: Damage Vulnerability:
//Player takes 30%(?) More damage from enemies for 4 seconds.