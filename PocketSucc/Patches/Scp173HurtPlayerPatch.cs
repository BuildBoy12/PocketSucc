// -----------------------------------------------------------------------
// <copyright file="Scp173HurtPlayerPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace PocketSucc.Patches
{
#pragma warning disable SA1313
    using HarmonyLib;
    using PlayableScps;

    /// <summary>
    /// Patches <see cref="Scp173.ServerKillPlayer"/> to respect <see cref="Config.ScpsDamageInPocket"/>.
    /// </summary>
    [HarmonyPatch(typeof(Scp173), nameof(Scp173.ServerKillPlayer))]
    internal static class Scp173HurtPlayerPatch
    {
        private static bool Prefix(Scp173 __instance)
        {
            return !Exiled.API.Features.Player.Get(__instance.Hub).IsInPocketDimension || Plugin.Instance.Config.ScpsDamageInPocket;
        }
    }
}