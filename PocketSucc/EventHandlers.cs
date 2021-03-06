// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace PocketSucc
{
    using Exiled.API.Extensions;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Where all EXILED events are handled.
    /// </summary>
    public class EventHandlers
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlers"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        private static Vector3 LastPortalPosition => Methods.Scp106Portal != null ? Methods.Scp106Portal.transform.position + (Vector3.up * 4) : RoleType.FacilityGuard.GetRandomSpawnProperties().Item1;

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp106.OnCreatingPortal(CreatingPortalEventArgs)"/>
        public void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            Methods.CurrentSuccs = 0;
            Timing.KillCoroutines(Methods.WaitCoroutine);
            Methods.WaitCoroutine = Timing.RunCoroutine(Methods.WaitForPortalActivated(4f));
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnEscapingPocketDimension(EscapingPocketDimensionEventArgs)"/>
        public void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                ev.TeleportPosition = LastPortalPosition;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs)"/>
        public void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                ev.IsAllowed = false;
                ev.Player.Position = LastPortalPosition;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnHurting(HurtingEventArgs)"/>
        public void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker == null)
                return;

            if (ev.Attacker.IsScp
                && ev.Attacker.IsInPocketDimension
                && !plugin.Config.ScpsDamageInPocket)
            {
                ev.IsAllowed = false;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted()"/>
        public void OnRoundStarted()
        {
            Methods.ImmuneObjects.Clear();
            Timing.KillCoroutines(Methods.PositionCoroutine);
            Methods.PositionCoroutine = Timing.RunCoroutine(Methods.CheckPositions(), Segment.FixedUpdate);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp106.OnTeleporting(TeleportingEventArgs)"/>
        public void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Methods.Scp106Portal.transform.position == Vector3.zero)
                ev.IsAllowed = false;
        }
    }
}