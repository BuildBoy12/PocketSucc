namespace PocketSucc
{
    using System;
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using MEC;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class Methods
    {
        /// <summary>
        /// Gets or sets the amount of succed players the current portal has.
        /// </summary>
        public static int CurrentSuccs { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="CoroutineHandle"/> to hold <see cref="CheckPositions"/>.
        /// </summary>
        public static CoroutineHandle PositionCoroutine { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CoroutineHandle"/> to hold <see cref="WaitForPortalActivated"/>.
        /// </summary>
        public static CoroutineHandle WaitCoroutine { get; set; }

        /// <summary>
        /// Gets the <see cref="GameObject"/> of Scp106's portal.
        /// </summary>
        public static GameObject Scp106Portal { get; private set; }

        /// <summary>
        /// Gets a list of <see cref="GameObject"/>s which cannot be teleported.
        /// </summary>
        public static List<GameObject> ImmuneObjects { get; } = new List<GameObject>();

        private static bool IsPortalActivated { get; set; }

        private static Vector3 PdEnter { get; } = Vector3.down * 1997f;

        private static Vector3 PdExit { get; } = Vector3.down * 1992f;

        private static Config Config => Plugin.Instance.Config;

        /// <summary>
        /// Checks the distance between objects and the <see cref="Scp106Portal"/> to see if it should be moved.
        /// </summary>
        /// <returns>A delay.</returns>
        public static IEnumerator<float> CheckPositions()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(Config.RefreshRate);
                if (Warhead.IsDetonated && !Config.TrapAfterWarhead)
                    yield break;
                
                if (Scp106Portal != null)
                {
                    if (!IsPortalActivated)
                        continue;

                    if (Config.TeleportGrenades)
                        CheckGrenades();

                    if (Config.TeleportItems)
                        CheckItems();

                    if (Config.TeleportPlayers)
                        CheckPlayers();
                }
                else
                {
                    Scp106Portal = GameObject.Find("SCP106_PORTAL");
                }
            }
        }

        /// <summary>
        /// Puts a cooldown on the teleportation of all objects.
        /// </summary>
        /// <param name="seconds">The amount of seconds the cooldown should last.</param>
        /// <returns>A delay.</returns>
        public static IEnumerator<float> WaitForPortalActivated(float seconds)
        {
            IsPortalActivated = false;
            yield return Timing.WaitForSeconds(seconds);
            IsPortalActivated = true;
        }

        private static void CheckGrenades()
        {
            foreach (Grenade grenade in Object.FindObjectsOfType<Grenade>())
            {
                if (ImmuneObjects.Contains(grenade.gameObject)
                    || (grenade is FragGrenade && Config.BlacklistedGrenades.Contains(GrenadeType.FragGrenade))
                    || (grenade is FlashGrenade && Config.BlacklistedGrenades.Contains(GrenadeType.Flashbang))
                    || (grenade is Scp018Grenade && Config.BlacklistedGrenades.Contains(GrenadeType.Scp018)))
                    continue;

                Vector3 grenadePos = grenade.transform.position;
                Vector3 portalPos = Scp106Portal.transform.position;

                if (Math.Abs(grenadePos.x - portalPos.x) <= Config.GrenadeRange
                    && Math.Abs(grenadePos.y - portalPos.y) <= Config.GrenadeVerticalRange
                    && Math.Abs(grenadePos.z - portalPos.z) <= Config.GrenadeRange)
                {
                    Timing.RunCoroutine(PortalAnimation(grenade.gameObject));
                }

                if (Vector3.Distance(grenadePos, PdExit) <= Config.GrenadeRange)
                {
                    ReverseAnimation(grenade.gameObject);
                }
            }
        }

        private static void CheckItems()
        {
            foreach (Pickup pickup in Pickup.Instances)
            {
                if (pickup == null || !pickup.gameObject || ImmuneObjects.Contains(pickup.gameObject))
                    continue;
                
                if (Config.BlacklistedItems != null && Config.BlacklistedItems.Contains(pickup.itemId))
                    continue;

                try
                {
                    Vector3 pickupPos = pickup.transform.position;
                    Vector3 portalPos = Scp106Portal.transform.position;

                    if (Math.Abs(pickupPos.x - portalPos.x) <= Config.ItemRange
                        && Math.Abs(pickupPos.y - portalPos.y) <= Config.ItemVerticalRange
                        && Math.Abs(pickupPos.z - portalPos.z) <= Config.ItemRange)
                    {
                        Timing.RunCoroutine(PortalAnimation(pickup.gameObject));
                    }

                    if (Vector3.Distance(pickupPos, PdExit) <= Config.ItemRange)
                    {
                        ReverseAnimation(pickup.gameObject);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        private static void CheckPlayers()
        {
            foreach (Player player in Player.List)
            {
                if (player == null
                    || string.IsNullOrEmpty(player.UserId)
                    || !player.IsAlive
                    || (Config.BlacklistedRoles.Contains(player.Role.ToString(), StringComparison.OrdinalIgnoreCase) && !player.Is035())
                    || (Config.BlacklistedRoles.Contains("Scp035", StringComparison.OrdinalIgnoreCase) && player.Is035()))
                    continue;

                if (Vector3.Distance(player.Position, Scp106Portal.transform.position) <= Config.PlayerRange)
                {
                    Timing.RunCoroutine(PortalAnimation(player), Segment.FixedUpdate);
                }
            }
        }

        private static void ReverseAnimation(GameObject gameObject)
        {
            ImmuneObjects.Add(gameObject);
            gameObject.transform.position = Scp106Portal.transform.position;
        }

        private static IEnumerator<float> PortalAnimation(Player player)
        {
            Scp106PlayerScript scp106PlayerScript = player.GameObject.GetComponent<Scp106PlayerScript>();
            
            if (scp106PlayerScript.goingViaThePortal)
                yield break;

            scp106PlayerScript.goingViaThePortal = true;

            bool inGodMode = player.IsGodModeEnabled;
            player.IsGodModeEnabled = true;
            player.ReferenceHub.fpc.NetworkforceStopInputs = true;
            for (int i = 0; i < 50; i++)
            {
                var pos = player.Position;
                pos.y -= i * 0.01f;
                player.Position = pos;
                yield return 0f;
            }

            player.Position = PdEnter;
            player.IsGodModeEnabled = inGodMode;
            player.ReferenceHub.fpc.NetworkforceStopInputs = false;
            if (++CurrentSuccs >= Config.MaximumTeleports)
            {
                scp106PlayerScript.DeletePortal();
            }
            
            WaitCoroutine = Timing.RunCoroutine(WaitForPortalActivated(Config.GlobalCooldown));
            if (Warhead.IsDetonated)
            {
                player.Kill(DamageTypes.Pocket);
                yield break;
            }
            
            if (!player.IsScpOr035())
            {
                player.Hurt(Config.Damage, DamageTypes.Pocket);
                player.EnableEffect<Corroding>();
            }

            yield return Timing.WaitForSeconds(Config.UserCooldown);
            scp106PlayerScript.goingViaThePortal = false;
        }

        private static IEnumerator<float> PortalAnimation(GameObject gameObject)
        {
            if (ImmuneObjects.Contains(gameObject))
                yield break;

            bool isGrenade = gameObject.TryGetComponent(out Grenade grenade);
            Vector3 velocity = Vector3.zero;
            if (isGrenade)
            {
                velocity = grenade.rb.velocity;
                grenade.rb.velocity = Vector3.zero;
            }

            while (gameObject.transform.position.y > Scp106Portal.transform.position.y)
            {
                var pos = gameObject.transform.position;
                pos.y -= 0.1f;
                gameObject.transform.position = pos;
                yield return 0f;
            }

            gameObject.transform.position = PdEnter;
            if (isGrenade)
            {
                grenade.rb.velocity = velocity;
            }
        }
    }
}