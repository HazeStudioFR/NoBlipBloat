using System;
using System.Collections.Generic;
using Rage;
using Rage.Native;

namespace NoBlipBloat;

internal static class BlipWorld
{
    internal static IEnumerable<Blip> GetAllBlips()
    {
        foreach (BlipSprite sprite in Enum.GetValues(typeof(BlipSprite)))
        {
            int handle = NativeFunction.Natives.GET_FIRST_BLIP_INFO_ID<int>((int)sprite);
            while (NativeFunction.Natives.DOES_BLIP_EXIST<bool>(handle))
            {
                var blip = World.GetBlipByHandle((uint)handle);
                if (blip != null) yield return blip;
                handle = NativeFunction.Natives.GET_NEXT_BLIP_INFO_ID<int>((int)sprite);
            }
        }
    }
}