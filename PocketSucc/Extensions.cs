namespace PocketSucc
{
    using Exiled.API.Features;

    public static class Extensions
    {
        public static bool IsScpOr035(this Player player) => player.IsScp || player.Is035();

        public static bool Is035(this Player player) => player.SessionVariables.ContainsKey("IsScp035");
    }
}