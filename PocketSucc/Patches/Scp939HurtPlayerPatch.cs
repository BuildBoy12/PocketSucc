// -----------------------------------------------------------------------
// <copyright file="Scp939HurtPlayerPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace PocketSucc.Patches
{
#pragma warning disable SA1313
    using Exiled.API.Features;
    using HarmonyLib;
    using PlayableScps;

    /// <summary>
    /// Patches <see cref="Scp939.ServerAttack"/> to respect <see cref="Config.ScpsDamageInPocket"/>.
    /// </summary>
    [HarmonyPatch(typeof(Scp939), nameof(Scp939.ServerAttack))]
    internal static class Scp939HurtPlayerPatch
    {
        private static bool Prefix(Scp939 __instance)
        {
            return !Player.Get(__instance.Hub).IsInPocketDimension || Plugin.Instance.Config.ScpsDamageInPocket;
        }
    }
}