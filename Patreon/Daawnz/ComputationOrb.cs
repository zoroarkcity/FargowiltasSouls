using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Patreon.Daawnz
{
    public class ComputationOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Computation Orb");
            Tooltip.SetDefault(
@"'Within the core, a spark of hope remains.'
Non -magic/summon attacks deal 25% extra damage but are affected by Mana Sickness
Non-magic/summon weapons require 10 mana to use");
            DisplayName.AddTranslation(GameCulture.Chinese, "演算宝珠");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"非魔法攻击将额外造成25%伤害, 并消耗10法力");

        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 8;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PatreonPlayer modPlayer = player.GetModPlayer<PatreonPlayer>();
            modPlayer.CompOrb = true;
        }
    }
}
