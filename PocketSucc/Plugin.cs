// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

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
        private Harmony harmony;
        private EventHandlers eventHandlers;

        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            eventHandlers = new EventHandlers(this);
            Scp106Handlers.CreatingPortal += eventHandlers.OnCreatingPortal;
            PlayerHandlers.EscapingPocketDimension += eventHandlers.OnEscapingPocketDimension;
            PlayerHandlers.FailingEscapePocketDimension += eventHandlers.OnFailingEscapePocketDimension;
            PlayerHandlers.Hurting += eventHandlers.OnHurting;
            ServerHandlers.RoundStarted += eventHandlers.OnRoundStarted;
            Scp106Handlers.Teleporting += eventHandlers.OnTeleporting;

            harmony = new Harmony($"pocketsucc.{DateTime.UtcNow.Ticks}");
            harmony.PatchAll();

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Scp106Handlers.CreatingPortal -= eventHandlers.OnCreatingPortal;
            PlayerHandlers.EscapingPocketDimension -= eventHandlers.OnEscapingPocketDimension;
            PlayerHandlers.FailingEscapePocketDimension -= eventHandlers.OnFailingEscapePocketDimension;
            PlayerHandlers.Hurting -= eventHandlers.OnHurting;
            ServerHandlers.RoundStarted -= eventHandlers.OnRoundStarted;
            Scp106Handlers.Teleporting -= eventHandlers.OnTeleporting;
            eventHandlers = null;

            harmony.UnpatchAll(harmony.Id);
            harmony = null;

            Instance = null;

            base.OnDisabled();
        }
    }
}