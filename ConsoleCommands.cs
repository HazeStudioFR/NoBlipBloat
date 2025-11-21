using Rage;
using Rage.Attributes;

namespace NoBlipBloat;

public static class ConsoleCommands
{
    [ConsoleCommand(Description = "Removes blips created by other plugins.", Name = "CleanupBlipBloats")]
    public static void CleanupBlips([ConsoleCommandParameter(Name = "Include LSPDFR Station Blips", Description = "if true, will delete lspdfr station blips from map. Jail not included.")] bool removeLSPDFRBlips)
    {
        foreach (var blip in BlipWorld.GetAllBlips())
        {
            if (!blip.Exists())
                return;

            if (!removeLSPDFRBlips && blip.Sprite == BlipSprite.PoliceStation)
                return;

            blip.Delete();
        }
    }
}