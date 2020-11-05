using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Misc
{
    public class AbomBag : ModItem
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.AbomBoss.AbomBoss>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("Right click to open");
            DisplayName.AddTranslation(GameCulture.Chinese, "突变体的摸彩袋");
            Tooltip.AddTranslation(GameCulture.Chinese, "右键打开");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = 11;
        }

        public override void OpenBossBag(Player player)
        {
            player.QuickSpawnItem(mod.ItemType("MutantScale"), Main.rand.Next(11) + 10);

            if (SoulConfig.Instance.PatreonFishron && Main.rand.Next(100) < 3)
                player.QuickSpawnItem(mod.ItemType("StaffOfUnleashedOcean"));
        }
    }
}
