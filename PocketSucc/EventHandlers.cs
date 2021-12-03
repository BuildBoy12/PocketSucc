namespace PocketSucc
{
    using Exiled.API.Extensions;
    using Exiled.Events.EventArgs;
    using MEC;
    using UnityEngine;

    /// <summary>
    /// Where all EXILED events are handled.
    /// </summary>
    public static class EventHandlers
    {
        private static Vector3 LastPortalPosition => Methods.Scp106Portal != null ? Methods.Scp106Portal.transform.position + (Vector3.up * 4) : RoleType.FacilityGuard.GetRandomSpawnProperties().Item1;

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp106.OnCreatingPortal(CreatingPortalEventArgs)"/>
        public static void OnCreatingPortal(CreatingPortalEventArgs ev)
        {
            Methods.CurrentSuccs = 0;
            Timing.KillCoroutines(Methods.WaitCoroutine);
            Methods.WaitCoroutine = Timing.RunCoroutine(Methods.WaitForPortalActivated(4f));
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnEscapingPocketDimension(EscapingPocketDimensionEventArgs)"/>
        public static void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            if (ev.Player.IsScpOr035())
            {
                ev.TeleportPosition = LastPortalPosition;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs)"/>
        public static void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
        {
            if (ev.Player.IsScpOr035())
            {
                ev.IsAllowed = false;
                ev.Player.Position = LastPortalPosition;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnHurting(HurtingEventArgs)"/>
        public static void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker == null)
                return;
            
            if (ev.Attacker.IsScpOr035() 
                && ev.Attacker.IsInPocketDimension 
                && !Plugin.Instance.Config.ScpsDamageInPocket)
            {
                ev.IsAllowed = false;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted()"/>
        public static void OnRoundStarted()
        {
            Methods.ImmuneObjects.Clear();
            Timing.KillCoroutines(Methods.PositionCoroutine);
            Methods.PositionCoroutine = Timing.RunCoroutine(Methods.CheckPositions(), Segment.FixedUpdate);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp106.OnTeleporting(TeleportingEventArgs)"/>
        public static void OnTeleporting(TeleportingEventArgs ev)
        {
            if (Methods.Scp106Portal.transform.position == Vector3.zero)
                ev.IsAllowed = false;
        }
    }
}