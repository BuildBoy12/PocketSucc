using PlayableScps;

namespace PocketSucc.Patches
{
    using Exiled.API.Features;
    using HarmonyLib;

    [HarmonyPatch(typeof(Scp939), nameof(Scp939.ServerAttack))]
    internal static class Scp939HurtPlayerPatch
    {
        private static bool Prefix(Scp939 __instance)
        {
            return !Player.Get(__instance.Hub).IsInPocketDimension || Plugin.Instance.Config.ScpsDamageInPocket;
        }
    }
}