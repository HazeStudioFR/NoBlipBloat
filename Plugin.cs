using System;
using Rage;
using Rage.Attributes;
using Rage.Native;

[assembly: Plugin("NoBlipBloat", Description = "Removes all the unnecessary blips.", Author = "HazeStudios", PrefersSingleInstance = true, SupportUrl = "https://discord.gg/mV9kaACXkM")]

namespace NoBlipBloat;

internal static class Plugin
{
    private static GameFiber _fiber;


    public static void Main()
    {
        AppDomain.CurrentDomain.DomainUnload += (_, _) => OnUnload();
        _fiber = new GameFiber(OnLoad, "NoBlipBloat");
        _fiber.Start();
    }

    private static void OnLoad()
    {
        NativeFunction.Natives.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME("blip_controller");
        while (_fiber.IsAlive)
            // This is here to prevent RPH from unloading the plugin, which then would cause the blip_controller to load again.
            GameFiber.Yield();
    }

    private static void OnUnload()
    {
        NativeFunction.Natives.REQUEST_SCRIPT("blip_controller");
        while (!NativeFunction.Natives.HAS_SCRIPT_LOADED<bool>("blip_controller"))
        {
            GameFiber.Yield();
            NativeFunction.Natives.REQUEST_SCRIPT("blip_controller");
        }

        NativeFunction.Natives.START_NEW_SCRIPT("blip_controller", 1424);
        NativeFunction.Natives.SET_SCRIPT_AS_NO_LONGER_NEEDED("blip_controller");
        if (_fiber.IsAlive) _fiber.Abort();
    }
}