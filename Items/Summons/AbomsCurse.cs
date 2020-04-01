using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Summons
{
    public class AbomsCurse : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abominationn's Curse");
        }

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
            item.value = Item.buyPrice(0, 8);
        }

        public override bool UseItem(Player player)
        {
            int abom = NPC.FindFirstNPC(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
            if (abom > -1 && Main.npc[abom].active)
            {
                Main.npc[abom].Transform(mod.NPCType("AbomBoss"));
                if (Main.netMode == 0)
                    Main.NewText("Abominationn has awoken!", 175, 75, 255);
                else if (Main.netMode == 2)
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Abominationn has awoken!"), new Color(175, 75, 255));
            }
            else
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("AbomBoss"));
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

        public override void AddRecipes() //make this harder again when changed to abom's gift
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoblinBattleStandard);
            recipe.AddIngredient(ItemID.PirateMap);
            recipe.AddIngredient(ItemID.PumpkinMoonMedallion);
            recipe.AddIngredient(ItemID.NaughtyPresent);
            recipe.AddIngredient(ItemID.SnowGlobe);
            recipe.AddIngredient(ItemID.DD2ElderCrystal);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(null, "LunarCrystal");

            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
