using UnityEngine;

public class Feather : IPickupItem
{
    public string Name => "Feather";

    public string Description => "Run speed up!";

    public string Lore => "The founding feathers...";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/Feather");

    public ItemRarity Rarity => ItemRarity.Common;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        SpeedCap = 24.0f,
    };
}

public class AncientGear : IPickupItem
{
    public string Name => "Ancient Gear";

    public string Description => "Attack speed up!";

    public string Lore => "When bullets fly, feathers will fall...";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/AncientGear");

    public ItemRarity Rarity => ItemRarity.Common;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        AttackCooldown = 0.92f,
    };
}

public class HarderShells : IPickupItem
{
    public string Name => "Harder Shells";

    public string Description => "Damage up!";

    public string Lore => "Hard-boiled and heavy!";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/HarderShells");

    public ItemRarity Rarity => ItemRarity.Common;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        Damage = 4,
    };
}

public class Gunpowder : IPickupItem
{
    public string Name => "Gunpowder";

    public string Description => "Bullet speed up!";

    public string Lore => "Sure hope this thing doesn't explode!";

    public Sprite Sprite { get; } = Resources.Load<Sprite>("ItemSprites/Gunpowder");

    public ItemRarity Rarity => ItemRarity.Common;

    public ItemModifiers Modifiers => new ItemModifiers()
    {
        ProjectileSpeed = 1.2f,
    };
}