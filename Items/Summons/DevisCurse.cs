using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Summons
{
	public class DevisCurse : ModItem
	{
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deviantt's Curse");
		}

        /*public override bool Autoload(ref string name)
        {
            return false;
        }*/

        public override void SetDefaults()
		{
            item.width = 20;
            item.height = 20;
            item.rare = 11;
            item.maxStack = 999;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.consumable = true;
            item.value = Item.buyPrice(0, 2);
        }

        public override bool UseItem(Player player)
        {
            int mutant = NPC.FindFirstNPC(ModLoader.GetMod("Fargowiltas").NPCType("Deviantt"));
                if (mutant > -1 && Main.npc[mutant].active)
                {
                    Main.npc[mutant].Transform(mod.NPCType("DeviBoss"));
                    if (Main.netMode == 0)
                        Main.NewText("Deviantt has awoken!", 175, 75, 255);
                    else if (Main.netMode == 2)
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Deviantt has awoken!"), new Color(175, 75, 255));
                }
                else
                {
                    NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("DeviBoss"));
                }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = Main.DiscoColor;
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel);
            recipe.AddIngredient(ItemID.Lens);
            recipe.AddIngredient(ItemID.RottenChunk);
            recipe.AddIngredient(ItemID.Stinger);
            //recipe.AddIngredient(ItemID.Bone);
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient(mod.ItemType("CrackedGem"), 5);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Gel);
            recipe.AddIngredient(ItemID.Lens);
            recipe.AddIngredient(ItemID.Vertebrae);
            recipe.AddIngredient(ItemID.Stinger);
            //recipe.AddIngredient(ItemID.Bone);
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient(mod.ItemType("CrackedGem"), 5);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
