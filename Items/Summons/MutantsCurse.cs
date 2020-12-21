using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using FargowiltasSouls.Utilities;

namespace FargowiltasSouls.Items.Summons
{
    public class MutantsCurse : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mutant's Curse");
            Tooltip.SetDefault("'At least this way, you don't need that doll'");
            DisplayName.AddTranslation(GameCulture.Chinese, "突变体的诅咒");
            Tooltip.AddTranslation(GameCulture.Chinese, "'至少不需要用娃娃了'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(3, 11));
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
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.consumable = true;
            item.value = Item.buyPrice(1);
            item.alpha = 255; //returning false on predraw prevents item from animating at all, this was simplest method i could think of to work around that
            item.noUseGraphic = true;
        }

        public override bool UseItem(Player player)
        {
            int mutant = NPC.FindFirstNPC(ModLoader.GetMod("Fargowiltas").NPCType("Mutant"));

            if (mutant > -1 && Main.npc[mutant].active)
            {
                Main.npc[mutant].Transform(mod.NPCType("MutantBoss"));
                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText("Mutant has awoken!", 175, 75, 255);
                else if (Main.netMode == NetmodeID.Server)
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Mutant has awoken!"), new Color(175, 75, 255));
            }
            else
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("MutantBoss"));
            }

            return true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            ExtraUtilities.DrawItem(whoAmI, Main.itemTexture[item.type], rotation, 11, lightColor); //item draws in wrong position by default so this is necessary
            return true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            ExtraUtilities.DrawItem(whoAmI, mod.GetTexture("Items/Summons/MutantsCurse_glow"), rotation, 11, Color.White);
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.spriteBatch.Draw(Main.itemTexture[item.type], position, frame, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f); //manually draw item icon because item alpha is set to 255
        }
    }
}