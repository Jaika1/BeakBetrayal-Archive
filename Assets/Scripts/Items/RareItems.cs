using UnityEngine;

public class UnholyArmaments : IPickupItem
{
    public string Name => "Unholy Armaments";

    public string Description => "Major improvement to all stats";

    public string Lore => "Macuahuitl of misery...";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/T_Icon_UnholyArmaments");

    public ItemRarity Rarity => ItemRarity.Rare;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        //Health = 50,
        //MaxHealth = 50,
        AttackCooldown = 0.90f,
        Damage = 8,
        Acceleration = 48.0f,
        SpeedCap = 48.0f,
        ProjectileSpeed = 1.6f
    };
}

public class CizinChallenge : IPickupItem
{
    public string Name => "Cizin's Challenge";

    public string Description => "Kill with 1 shot, die to 1 shot";

    public string Lore => "The fate of thou hangs only in the balance...";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/T_Icon_CiznsChallenge");

    public ItemRarity Rarity => ItemRarity.Rare;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        Damage = 10000,
        InstantDeath = true,
        AttackCooldown = 1.7f,
        ProjectileSpeed = 9.0f
    };
}

public class SentientEgg : IPickupItem
{
    public string Name => "Sentient Eggs";

    public string Description => "Eggs will seek out players";

    public string Lore => "I'm sorry, little one...";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/T_Icon_SentientEgg");

    public ItemRarity Rarity => ItemRarity.Rare;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        TrackingRadius = 7.0f
    };
}