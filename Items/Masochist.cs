using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items
{
    public class Masochist : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mutant's Gift");
            Tooltip.SetDefault("'Use this to turn on/off Eternity Mode'");
            DisplayName.AddTranslation(GameCulture.Chinese, "突变体的礼物");
            Tooltip.AddTranslation(GameCulture.Chinese, "'用开/关受虐模式'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.rare = 1;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.consumable = false;
        }

        public override bool UseItem(Player player)
        {
            bool bossExists = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].active && Main.npc[i].boss)
                {
                    bossExists = true;
                    break;
                }
            }

            if (!bossExists)
            {
                FargoSoulsWorld.MasochistMode = !FargoSoulsWorld.MasochistMode;
                Main.expertMode = true;

                string text = FargoSoulsWorld.MasochistMode ? "Eternity Mode initiated!" : "Eternity Mode deactivated!";
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(text, 175, 75, 255);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(175, 75, 255));
                    NetMessage.SendData(MessageID.WorldData); //sync world
                }

                if (FargoSoulsWorld.MasochistMode && !NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Deviantt")))
                    NPC.SpawnOnPlayer(player.whoAmI, ModLoader.GetMod("Fargowiltas").NPCType("Deviantt"));

                Main.PlaySound(SoundID.Roar, (int)player.position.X, (int)player.position.Y, 0);

                /*if (FargoSoulsWorld.MasochistMode && !SkyManager.Instance["FargowiltasSouls:MutantBoss2"].IsActive())
                    SkyManager.Instance.Activate("FargowiltasSouls:MutantBoss2");*/
            }
            return true;
        }
    }
}