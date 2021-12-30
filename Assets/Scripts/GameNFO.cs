using System;
using System.Collections.Generic;

/// <summary>
/// A public class that contains major game information that is to be kept between scenes (such as score).
/// </summary>
public static class GameNFO
{
    /// <summary>
    /// A list of every single item that was found in the assembly.
    /// </summary>
    public static List<IPickupItem> PickupItems;
    /// <summary>
    /// A list of every single common item that was found in the assembly.
    /// </summary>
    public static List<IPickupItem> CommonItems;
    /// <summary>
    /// A list of every single uncommon item that was found in the assembly.
    /// </summary>
    public static List<IPickupItem> UncommonItems;
    /// <summary>
    /// A list of every single rare item that was found in the assembly.
    /// </summary>
    public static List<IPickupItem> RareItems;
    /// <summary>
    /// The maximum amount of players that can play the game.
    /// </summary>
    public const int HumanPlayerLimit = 4;
    /// <summary>
    /// How many human and non-human players will be in the game.
    /// </summary>
    public static int MaxPlayers = 8;
    /// <summary>
    /// The current round being played.
    /// </summary>
    public static int RoundNum = 0;
    /// <summary>
    /// The amount of rounds to play.
    /// </summary>
    public static int RoundCount = 3;
    /// <summary>
    /// Time that must pass before teleportation occurs
    /// </summary>
    public static float FinalTeleportSeconds = 120.0f;
    /// <summary>
    /// A list containing information about each entity.
    /// </summary>
    public static EntityInformation[] EntityNFOs;

    public static void SortPlayerList()
    {
        Array.Sort(EntityNFOs, (x, y) =>
        {
            if (x.PlayerGamepad != null && y.PlayerGamepad == null) return -1;
            if (x.PlayerGamepad == null && y.PlayerGamepad != null) return 1;
            return 0;
        });
    }
    public static void SortPlayerListByScore()
    {
        Array.Sort(EntityNFOs, (x, y) =>
        {
            return Math.Sign(y.Score - x.Score);
        });
    }
}
