namespace PocketSucc
{
    using System;
    using Exiled.API.Features;
    using HarmonyLib;
    using PlayerHandlers = Exiled.Events.Handlers.Player;
    using Scp106Handlers = Exiled.Events.Handlers.Scp106;
    using ServerHandlers = Exiled.Events.Handlers.Server;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private static Harmony harmony;

        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc/>
        public override string Author { get; } = "Build";

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(3, 7, 2);

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            Scp106Handlers.CreatingPortal += EventHandlers.OnCreatingPortal;
            PlayerHandlers.EscapingPocketDimension += EventHandlers.OnEscapingPocketDimension;
            PlayerHandlers.FailingEscapePocketDimension += EventHandlers.OnFailingEscapePocketDimension;
            PlayerHandlers.Hurting += EventHandlers.OnHurting;
            ServerHandlers.RoundStarted += EventHandlers.OnRoundStarted;
            Scp106Handlers.Teleporting += EventHandlers.OnTeleporting;

            harmony = new Harmony($"pocketsucc.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Scp106Handlers.CreatingPortal -= EventHandlers.OnCreatingPortal;
            PlayerHandlers.EscapingPocketDimension -= EventHandlers.OnEscapingPocketDimension;
            PlayerHandlers.FailingEscapePocketDimension -= EventHandlers.OnFailingEscapePocketDimension;
            PlayerHandlers.Hurting -= EventHandlers.OnHurting;
            ServerHandlers.RoundStarted -= EventHandlers.OnRoundStarted;
            Scp106Handlers.Teleporting -= EventHandlers.OnTeleporting;

            harmony.UnpatchAll();
            harmony = null;

            Instance = null;

            base.OnDisabled();
        }
    }
}