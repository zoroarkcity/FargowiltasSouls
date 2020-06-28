using System;
using System.Collections.Generic;
using FargowiltasSouls.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Patreon
{
    public class PatreonGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.damage > 0 && player.GetModPlayer<PatreonPlayer>().CompOrb && !item.magic && !item.summon)
            {
                if (player.statMana >= 10)
                {
                    player.statMana -= 10;
                    player.manaRegenDelay = 300;
                }
                else //not enough mana to use items
                {
                    return false;
                }
            }

            return true;
        }
    }
}