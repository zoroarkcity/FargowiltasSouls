using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Misc
{
    public class DeviBag : ModItem
    {
        public override int BossBagNPC => ModContent.NPCType<NPCs.DeviBoss.DeviBoss>();

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
            player.QuickSpawnItem(mod.ItemType("DeviatingEnergy"), Main.rand.Next(11) + 10);

        }
    }
}
