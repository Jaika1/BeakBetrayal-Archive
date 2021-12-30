using UnityEngine;

public class Boots : IPickupItem
{
    public string Name => "Boots";

    public string Description => "Major run speed improvement!";

    public string Lore => "Use these soles to farm more souls...";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/Boots");

    public ItemRarity Rarity => ItemRarity.Uncommon;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        SpeedCap = 64.0f,
    };
}

public class AncientMotor : IPickupItem
{
    public string Name => "Ancient Motor";

    public string Description => "Major attack speed improvement!";

    public string Lore => "Turbotastic!";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/T_Icon_Gears");

    public ItemRarity Rarity => ItemRarity.Uncommon;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        AttackCooldown = 0.85f,
    };
}

public class IronEgg : IPickupItem
{
    public string Name => "Metal Egg";

    public string Description => "Major damage improvement!";

    public string Lore => "Eat lead, sucker!";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/T_Icon_MetalEgg");

    public ItemRarity Rarity => ItemRarity.Uncommon;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        Damage = 8,
    };
}

public class EggRockets : IPickupItem
{
    public string Name => "Egg Rockets!";

    public string Description => "Major bullet speed improvement!";

    public string Lore => "Paint the walls yellow!";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/T_Icon_EggRockets");

    public ItemRarity Rarity => ItemRarity.Uncommon;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        ProjectileSpeed = 3.6f,
    };
}