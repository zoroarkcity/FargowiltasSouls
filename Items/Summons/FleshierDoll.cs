using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Summons
{
	public class FleshierDoll : ModItem
	{
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fleshier Doll");
        }

		public override void SetDefaults()
		{
            item.width = 20;
            item.height = 20;
            item.rare = 1;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.maxStack = 20;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.position.Y / 16 > Main.maxTilesY - 200 && !NPC.AnyNPCs(NPCID.WallofFlesh);
        }

        public override bool UseItem(Player player)
        {
            if (Main.hardMode)
            {
                for (int i = 0; i < 30; i++)
                    NPC.SpawnOnPlayer(player.whoAmI, NPCID.TheHungryII);
                player.AddBuff(BuffID.TheTongue, 30);
            }
            NPC.SpawnWOF(player.Center);
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}
