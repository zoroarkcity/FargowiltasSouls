using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Summons
{
    public class AbominationnVoodooDoll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abominationn Voodoo Doll");
            Tooltip.SetDefault("Summons Abominationn to your town\n'You are a terrible person'");
            DisplayName.AddTranslation(GameCulture.Chinese, "憎恶巫毒娃娃");
            Tooltip.AddTranslation(GameCulture.Chinese, "你可真是个坏东西");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = 11;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.maxStack = 20;
            item.value = Item.sellPrice(0, 1);
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (item.lavaWet)
            {
                if (Main.netMode != 1)
                {
                    int abominationn = NPC.FindFirstNPC(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
                    int mutant = NPC.FindFirstNPC(ModLoader.GetMod("Fargowiltas").NPCType("Mutant"));
                    if (abominationn > -1 && Main.npc[abominationn].active)
                    {
                        Main.npc[abominationn].StrikeNPC(9999, 0f, 0);
                        if (mutant > -1 && Main.npc[mutant].active)
                        {
                            Main.npc[abominationn].StrikeNPC(9999, 0f, 0);
                            if (mutant > -1 && Main.npc[mutant].active)
                            {
                                Main.npc[mutant].Transform(mod.NPCType("MutantBoss"));
                                if (Main.netMode == 0)
                                    Main.NewText("Yharim has been enraged by the death of his brother!", 175, 75, 255);
                                else if (Main.netMode == 2)
                                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Yharim has been enraged by the death of his brother!"), new Color(175, 75, 255));
                            }

                        }
                    }
                }
                item.TurnToAir();
            }
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(Main.DiscoR, 51, 255 - (int)(Main.DiscoR * 0.4));
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MutantScale", 5);
            recipe.AddIngredient(ItemID.GuideVoodooDoll);
            recipe.AddTile(ModContent.TileType<CrucibleCosmosSheet>());

            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
