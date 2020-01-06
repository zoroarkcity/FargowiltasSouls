using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    public class SinisterIcon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sinister Icon");
            Tooltip.SetDefault(@"'Most definitely not alive'
Prevents Masochist Mode-induced natural boss spawns
Increases spawn rate
Graze projectiles to gain up to 25% increased crit damage
Crit damage bonus decreases over time and is fully lost on hit");
            DisplayName.AddTranslation(GameCulture.Chinese, "邪恶画像");
            Tooltip.AddTranslation(GameCulture.Chinese, @"'肯定不是活着的'
阻止受虐模式导致的Boss自然生成
提高刷怪速率");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 4;
            item.value = Item.sellPrice(0, 3);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.SinisterIcon))
                player.GetModPlayer<FargoPlayer>().SinisterIcon = true;

            player.GetModPlayer<FargoPlayer>().Graze = true;
        }
    }
}
