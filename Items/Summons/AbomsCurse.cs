using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.NPCs.AbomBoss;
using Fargowiltas.Items.Tiles;
using FargowiltasSouls.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace FargowiltasSouls.Items.Summons
{
    public class AbomsCurse : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abominationn's Curse");

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 10));
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Purple;
            item.maxStack = 999;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
            item.value = Item.buyPrice(gold: 8);
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.alpha = 255;
            item.noUseGraphic = true;
        }

        public override bool UseItem(Player player)
        {
            int abom = NPC.FindFirstNPC(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));

            if (abom > -1 && Main.npc[abom].active)
            {
                // TODO: Localization.
                string message = "Abominationn has awoken!";

                Main.npc[abom].Transform(ModContent.NPCType<AbomBoss>());

                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(message, 175, 75, 255);
                else if (Main.netMode == NetmodeID.Server)
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(175, 75, 255));
            }
            else
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<AbomBoss>());

            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            ExtraUtilities.DrawItem(whoAmI, Main.itemTexture[item.type], rotation, 10, lightColor);
            return true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            ExtraUtilities.DrawItem(whoAmI, mod.GetTexture("Items/Summons/AbomsCurse_glow"), rotation, 10, Color.White);
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.spriteBatch.Draw(Main.itemTexture[item.type], position, frame, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f); //manually draw item icon because item alpha is set to 255
        }

        public override void AddRecipes() // Make this harder again when changed to abom's gift
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoblinBattleStandard);
            recipe.AddIngredient(ItemID.PirateMap);
            recipe.AddIngredient(ItemID.PumpkinMoonMedallion);
            recipe.AddIngredient(ItemID.NaughtyPresent);
            recipe.AddIngredient(ItemID.SnowGlobe);
            recipe.AddIngredient(ItemID.DD2ElderCrystal);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(ModContent.TileType<CrucibleCosmosSheet>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}