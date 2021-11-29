namespace PocketSucc.Patches
{
    using PlayableScps;
    using HarmonyLib;

    [HarmonyPatch(typeof(Scp173), nameof(Scp173.ServerKillPlayer))]
    internal static class Scp173HurtPlayerPatch
    {
        private static bool Prefix(Scp173 __instance)
        {
            return !Exiled.API.Features.Player.Get(__instance.Hub).IsInPocketDimension || Plugin.Instance.Config.ScpsDamageInPocket;
        }
    }
}