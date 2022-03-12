// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace PocketSucc
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;

    /// <inheritdoc cref="IConfig"/>
    public sealed class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the amount of seconds between checking the positions of objects to teleport.
        /// </summary>
        [Description("The amount of seconds between checking the positions of objects to teleport.")]
        public float RefreshRate { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets a value indicating whether the trap will be active after the warhead has detonated.
        /// </summary>
        [Description("Whether the trap will be active after the warhead has detonated.")]
        public bool TrapAfterWarhead { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether grenades can be teleported by Scp106's portal.
        /// </summary>
        [Description("Whether grenades can be teleported by Scp106's portal.")]
        public bool TeleportGrenades { get; set; } = true;

        /// <summary>
        /// Gets or sets the range of the pocket trap against grenades.
        /// </summary>
        [Description("The range of the pocket trap against grenades.")]
        public float GrenadeRange { get; set; } = 2.5f;

        /// <summary>
        /// Gets or sets the vertical range of the pocket trap against grenades.
        /// </summary>
        [Description("The vertical range of the pocket trap against grenades.")]
        public float GrenadeVerticalRange { get; set; } = 4f;

        /// <summary>
        /// Gets or sets the types of grenades which cannot be teleported.
        /// </summary>
        [Description("The types of grenades which cannot be teleported.")]
        public List<GrenadeType> BlacklistedGrenades { get; set; } = new List<GrenadeType>
        {
            GrenadeType.Scp018,
        };

        /// <summary>
        /// Gets or sets a value indicating whether items can be teleported by Scp106's portal.
        /// </summary>
        [Description("Whether items can be teleported by Scp106's portal.")]
        public bool TeleportItems { get; set; } = true;

        /// <summary>
        /// Gets or sets the range of the pocket trap against items.
        /// </summary>
        [Description("The range of the pocket trap against items.")]
        public float ItemRange { get; set; } = 2.5f;

        /// <summary>
        /// Gets or sets the vertical range of the pocket trap against items.
        /// </summary>
        [Description("The vertical range of the pocket trap against items.")]
        public float ItemVerticalRange { get; set; } = 4f;

        /// <summary>
        /// Gets or sets the types of items which cannot be teleported.
        /// </summary>
        [Description("The types of items which cannot be teleported.")]
        public List<ItemType> BlacklistedItems { get; set; } = new List<ItemType>
        {
            ItemType.Coin,
            ItemType.Flashlight,
        };

        /// <summary>
        /// Gets or sets a value indicating whether players can be teleported by Scp106's portal.
        /// </summary>
        [Description("Whether players can be teleported by Scp106's portal.")]
        public bool TeleportPlayers { get; set; } = true;

        /// <summary>
        /// Gets or sets the range of the pocket trap against players.
        /// </summary>
        [Description("The range of the pocket trap against players.")]
        public float PlayerRange { get; set; } = 2.5f;

        /// <summary>
        /// Gets or sets the amount of players a portal will take before it closes.
        /// </summary>
        [Description("The amount of players a portal will take before it closes.")]
        public int MaximumTeleports { get; set; } = 5;

        /// <summary>
        /// Gets or sets the amount of seconds after a user falls into the portal that any user can fall again.
        /// </summary>
        [Description("The amount of seconds after a user falls into the portal that any user can fall again.")]
        public float GlobalCooldown { get; set; } = 1.5f;

        /// <summary>
        /// Gets or sets the amount of seconds after a user falls into the portal that they can fall again.
        /// </summary>
        [Description("The amount of seconds after a user falls into the portal that they can fall again.")]
        public float UserCooldown { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the amount of damage to be dealt to players that enter via the portal.
        /// </summary>
        [Description("The amount of damage to be dealt to players that enter via the portal.")]
        public int Damage { get; set; } = 30;

        /// <summary>
        /// Gets or sets which roles cannot be sent to the pocket dimension.
        /// </summary>
        [Description("Which roles cannot be sent to the pocket dimension.")]
        public List<RoleType> BlacklistedRoles { get; set; } = new List<RoleType>
        {
            RoleType.Scp106,
            RoleType.Scp079,
            RoleType.Spectator,
        };

        /// <summary>
        /// Gets or sets a value indicating whether SCP subjects can deal damage while inside of the pocket dimension.
        /// </summary>
        [Description("Whether SCP subjects can deal damage while inside of the pocket dimension.")]
        public bool ScpsDamageInPocket { get; set; } = false;
    }
}