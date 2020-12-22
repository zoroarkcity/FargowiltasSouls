using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
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

        int framecounter;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            framecounter++;
            if(framecounter > 2)
            {
                Main.itemFrame[whoAmI]++;
                if (Main.itemFrame[whoAmI] > 10)
                    Main.itemFrame[whoAmI] = 0;

                framecounter = 0;
            }
            ExtraUtilities.DrawItem(whoAmI, Main.itemTexture[item.type], rotation, 11, lightColor); //item draws in wrong position by default so this is necessary
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            ExtraUtilities.DrawItem(whoAmI, mod.GetTexture("Items/Summons/MutantsCurse_glow"), rotation, 11, Color.White);
        }
    }
}