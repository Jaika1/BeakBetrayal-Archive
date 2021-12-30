using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IPickupItem
{
    string Name { get; }
    string Description { get; }
    string Lore { get; }
    Sprite Sprite { get; }
    ItemRarity Rarity { get; }
    ItemModifiers Modifiers { get; }
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare
}

public class ItemModifiers
{
    public static IEnumerable<FieldInfo> ModifierFields { get; } = typeof(ItemModifiers).GetFields();

    /// <summary>
    /// Amount to adjust the health of the player by.
    /// </summary>
    [ItemModifierType(ItemModifierType.Additive)]
    public int Health = 0;

    /// <summary>
    /// Amount to adjust the max health of the player by.
    /// </summary>
    [ItemModifierType(ItemModifierType.Additive)]
    public int MaxHealth = 0;

    /// <summary>
    /// Multiplier to adjust the fire rate by.
    /// </summary>
    [ItemModifierType(ItemModifierType.Multiplicative)]
    public float AttackCooldown = 1.0f;

    /// <summary>
    /// Amount to adjust the damage by.
    /// </summary>
    [ItemModifierType(ItemModifierType.Additive)]
    public int Damage = 0;

    /// <summary>
    /// Amount to adjust the entities acceleration by.
    /// </summary>
    [ItemModifierType(ItemModifierType.Additive)]
    public float Acceleration = 0.0f;

    /// <summary>
    /// Amount to adjust the entities top speed by.
    /// </summary>
    [ItemModifierType(ItemModifierType.Additive)]
    public float SpeedCap = 0.0f;

    /// <summary>
    /// Amount to adjust the speed of the entities attacking projectiles.
    /// </summary>
    [ItemModifierType(ItemModifierType.Additive)]
    public float ProjectileSpeed = 0.0f;

    /// <summary>
    /// Proximity to the projectiles fired before they track.
    /// </summary>
    [ItemModifierType(ItemModifierType.Additive)]
    public float TrackingRadius = 0.0f;

    public bool InstantDeath = false;
}

public class ItemModifierTypeAttribute : Attribute
{
    public ItemModifierType ModifierType;

    public ItemModifierTypeAttribute(ItemModifierType type)
    {
        ModifierType = type;
    }
}

public enum ItemModifierType
{
    Additive,
    Multiplicative
}